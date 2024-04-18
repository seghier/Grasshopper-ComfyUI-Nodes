using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComfyUIsett
{
    public class ComfyUIComponent : GH_Component, IGH_VariableParameterComponent
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ComfyUIComponent()
          : base("ComfyUI Settings", "ComfyUISets",
            "ComfyUI Settings",
            "Params", "ComfyUI")
        {
            VariableParameterMaintenance();
            NewCount();
        }

        private bool resetEnabled = false;

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendSeparator(menu);
            ToolStripMenuItem delete = Menu_AppendItem(menu, "Delete inputs", DeleteInputs, true, resetEnabled);
        }

        private void DeleteInputs(Object sender, EventArgs e)
        {
            resetEnabled = !resetEnabled;
            ExpireSolution(true);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Inputs", "Inputs", "ComfyUI inputs", GH_ParamAccess.list);
            Params.Input[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Parameters", "Parameters", "ComfyUI parameters", GH_ParamAccess.list);
            pManager.AddTextParameter("Arguments", "Arguments", "Command arguments", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        /// 
        public int paramdiff = 0;
        List<string> names = new List<string>();

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Document gdoc = Instances.ActiveCanvas.Document;

            List<string> inputs = new List<string>();
            int i = 0;

            if (!DA.GetDataList(0, inputs))
            {
                if (resetEnabled)
                {
                    gdoc.ScheduleSolution(5, CallBackRemove);
                    inputs = count;
                }
                else
                    return;
            }

            foreach (string name in inputs)
            {
                bool containsComfyInput = false; // Flag to check if the current input contains a comfy input

                foreach (string c_input in comfyinputs)
                {
                    if (name.Contains(c_input))
                    {
                        containsComfyInput = true;
                        break; // Exit the inner loop early if a comfy input is found
                    }
                }

                // If the current input doesn't contain any comfy input, return
                if (!containsComfyInput)
                {
                    return;
                }
            }

            names = inputs;
            i = inputs.Count;

            //check outputs count
            int N = Params.Input.Count;
            int target = 1 + i;

            paramdiff = N - target;

            if (paramdiff != 0 && inputs != null)
            {
                gdoc.ScheduleSolution(0, CallBack);
            }

            inputsValues.Clear(); // Clear the list before adding new values
            // Retrieve data from input parameters excluding the first one
            for (int j = 1; j < Params.Input.Count; j++)
            {
                if (Params.Input[j] is IGH_Param stringParam)
                {
                    foreach (var data in stringParam.VolatileData.AllData(true))
                    {
                        if (data != null)
                        {
                            inputsValues.Add(data.ToString());
                        }
                    }
                }
            }

            string arguments = string.Join(" ", NewList(inputs, inputsValues));

            DA.SetDataList(0, NewList(inputs, inputsValues));
            DA.SetData(1, arguments);
        }


        List<string> count = new List<string>();
        List<string> inputsValues = new List<string>();
        private List<string> comfyinputs = new List<string>() { "GHString", "GHFile", "GHLoadImage", "GHSaveImage", "GHInteger", "GHFloat", "GHBool", "GHPrompt", "APIjsonfile" };

        List<string> NewList(List<string> inputs_, List<string> values_)
        {
            List<string> newinputs = new List<string>();

            // Check if either inputs_ or values_ is null, or if they have different counts
            if (inputs_ == null || values_ == null || inputs_.Count != values_.Count)
            {
                // Handle the error case here, such as throwing an exception or returning an empty list
                return newinputs;
            }

            // Iterate over the minimum count of inputs_ and values_
            int count = Math.Min(inputs_.Count, values_.Count);
            for (int i = 0; i < count; i++)
            {
                string newinput;
                if (inputs_[i].Contains("seed"))
                {
                    newinput = "--" + inputs_[i] + " " + GetSeed();
                }
                else if (inputs_[i].Contains("GHPrompt") || inputs_[i].Contains("GHFile") || inputs_[i].Contains("GHImage") || inputs_[i].Contains("GHSaveImage") || inputs_[i] == "APIjsonfile")
                {
                    newinput = "--" + inputs_[i] + " " + '"' + values_[i] + '"';
                }
                else
                {
                    newinput = "--" + inputs_[i] + " " + values_[i];
                }
                newinputs.Add(newinput);
            }

            return newinputs;
        }

        string GetSeed()
        {
            Random random = new Random();
            int randomseed = random.Next();

            string seed = "";

            foreach (var param in Params.Input)
            {
                if (param.NickName.Contains("seed"))
                {
                    if (param is IGH_Param stringParam)
                    {
                        foreach (var data in stringParam.VolatileData.AllData(true))
                        {
                            if (data != null)
                            {
                                if (data.ToString() == "-1")
                                {
                                    seed = randomseed.ToString();
                                }
                                else
                                {
                                    seed = data.ToString();
                                }
                            }
                        }
                    }
                }
            }
            return seed;
        }


        public void CallBack(GH_Document gH_Document)
        {
            //remove unnecessary inputs
            for (int i = 0; i < paramdiff; i++)
            {
                Params.UnregisterInputParameter(Params.Input[Params.Input.Count - 1]);
                Params.OnParametersChanged();
            }

            //add necessary inputs
            for (int i = 0; i < -paramdiff; i++)
            {
                Param_String inputText = new Param_String();
                inputText.Access = GH_ParamAccess.item;
                inputText.Optional = true;
                Params.RegisterInputParam(inputText, Params.Input.Count);
                Params.OnParametersChanged();
            }

            VariableParameterMaintenance();
            this.ExpireSolution(true);

        }

        public void CallBackRemove(GH_Document gH_Document)
        {
            if (SourceObjs(gH_Document) != null)
                gH_Document.RemoveObjects(SourceObjs(gH_Document), true);
        }

        public void NewCount()
        {
            for (int i = 0; i < 0; i++)
            {
                count.Add(i.ToString());
            }
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return new Param_GenericObject();
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
            while (names.Count < this.Params.Input.Count - 1)
            {
                names.Add(""); // Add default value
            }

            for (int i = 1; i < this.Params.Input.Count; i++)
            {
                this.Params.Input[i].Name = names[i - 1]; // Use i to index names
                this.Params.Input[i].NickName = names[i - 1]; // Use i to index names
                this.Params.Input[i].Description = names[i - 1]; // Use i to index names
            }
        }

        
        List<IGH_DocumentObject> SourceObjs(GH_Document doc)
        {
            var objs = new List<IGH_DocumentObject>();
            foreach (IGH_DocumentObject obj in doc.Objects)
            {
                foreach (string nickname in comfyinputs)
                {
                    if (obj.Name.Contains(nickname) || obj.Name == "ComfyUIParameters")
                        objs.Add(obj);
                }

            }
            return objs;
        }

        public override bool Write(GH_IWriter writer)
        {
            // Write the names list to the GH_IWriter
            writer.SetString("Names", string.Join(",", names.ToArray()));
            writer.SetBoolean("DeleteInputs", resetEnabled);

            // Call the base Write method to write other data
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("DeleteInputs"))
                resetEnabled = reader.GetBoolean("DeleteInputs");
            // Read the names list from the GH_IReader
            string namesString = reader.GetString("Names");
            names = new List<string>(namesString.Split(','));

            // Call the base Read method to read other data
            return base.Read(reader);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                return Properties.Resources.comfy;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{93F1BABB-EC00-4408-BCD7-AF6636ABAFB9}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}