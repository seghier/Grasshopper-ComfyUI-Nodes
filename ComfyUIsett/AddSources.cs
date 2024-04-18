using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace ComfyUIsett
{
    public class AddSources : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        /// 
        public AddSources()
          : base("ComfyUI AddSources", "ComfyUIAddSrc", "Add Sources to all inputs and output of ComfyUI Settings component",
                   "Params", "ComfyUI")
        {

        }

        private bool keepInput = false;


        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendSeparator(menu);
            ToolStripMenuItem keep = Menu_AppendItem(menu, "Disable update", KeepInputs, true, keepInput);
        }

        private void KeepInputs(Object sender, EventArgs e)
        {
            keepInput = !keepInput;
            ExpireSolution(true);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Add", "Add", "Add Sources", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        /// 

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool addsource = false;
            if (!DA.GetData(0, ref addsource))
                return;
            GH_Document gdoc = Instances.ActiveCanvas.Document;
            if (!keepInput && addsource)
                gdoc.ScheduleSolution(5, SolutionCallback);
        }

        private List<string> comfyinputs = new List<string>() { "GHString", "GHFile", "GHInteger", "GHFloat", "GHBool", "GHPrompt", "APIjsonfile" };

        private void SolutionCallback(GH_Document doc)
        {
            foreach (IGH_DocumentObject obj in doc.Objects)
            {
                if (obj.Name == "ComfyUI Settings")
                {
                    IGH_Component targetComponent = obj as IGH_Component;
                    if (targetComponent.Params.Input[0].Sources.Count == 0)
                    {
                        // First input is empty or null, return without adding sources
                        return;
                    }

                    int sourcescount = GetTotalSourceCount(targetComponent);

                    if (sourcescount > 0)
                    {
                        doc.RemoveObjects(SourceObjs(doc), true);
                    }

                    // Iterate over all inputs of the target component except for the first one
                    for (int i = 1; i < targetComponent.Params.Input.Count; i++)
                    {
                        // Add new objects based on input type
                        string nickname = targetComponent.Params.Input[i].NickName;
                        if (targetComponent.Params.Input[i].NickName.Contains("GHString"))
                        {
                            var panel = AddPanel(targetComponent, i, nickname, true, 60, 260);
                            doc.AddObject(panel, false);
                            targetComponent.Params.Input[i].AddSource(panel);
                        }
                        else if (targetComponent.Params.Input[i].NickName.Contains("GHPrompt"))
                        {
                            var panel = AddPanel(targetComponent, i, nickname,false, 100, 420);
                            doc.AddObject(panel, false);
                            targetComponent.Params.Input[i].AddSource(panel);
                        }
                        else if (targetComponent.Params.Input[i].NickName.Contains("GHBool"))
                        {
                            var toggle = AddToggle(targetComponent, i, nickname);
                            doc.AddObject(toggle, false);
                            targetComponent.Params.Input[i].AddSource(toggle);
                        }
                        else if (targetComponent.Params.Input[i].NickName.Contains("GHFile"))
                        {
                            var filepath = AddFilePath(targetComponent, i, nickname);
                            doc.AddObject(filepath, false);
                            targetComponent.Params.Input[i].AddSource(filepath);
                        }
                        else if (targetComponent.Params.Input[i].NickName.Contains("APIjsonfile"))
                        {
                            var filepath = AddFilePath(targetComponent, i, "APIjsonfile");
                            doc.AddObject(filepath, false);
                            targetComponent.Params.Input[i].AddSource(filepath);
                        }
                        else if (targetComponent.Params.Input[i].NickName.Contains("GHInteger") || targetComponent.Params.Input[i].NickName.Contains("GHFloat"))
                        {
                            var slid = AddSlider(targetComponent, i);
                            doc.AddObject(slid, false);
                            targetComponent.Params.Input[i].AddSource(slid);
                        }
                    }

                    var existingOutputPanel = doc.Objects.OfType<GH_Panel>().FirstOrDefault(panel => panel.Name == "ComfyUIParameters");
                    if (existingOutputPanel != null)
                    {
                        doc.RemoveObject(existingOutputPanel, true);
                    }

                    if (targetComponent.Params.Input.Count > 1)
                    {
                        // Add new output panel
                        var outpanel = AddOutPanel(targetComponent);
                        if (outpanel != null)
                            doc.AddObject(outpanel, false);
                    }

                    break;
                }
            }
        }

        int GetTotalSourceCount(IGH_Component targetComponent)
        {
            int totalSourceCount = 0;

            for (int i = 1; i < targetComponent.Params.Input.Count; i++)
            {
                totalSourceCount += targetComponent.Params.Input[i].Sources.Count;
            }
            return totalSourceCount;
        }

        List<IGH_DocumentObject> SourceObjs(GH_Document doc)
        {
            var objs = new List<IGH_DocumentObject>();
            foreach (IGH_DocumentObject obj in doc.Objects)
            {
                foreach (string nickname in comfyinputs)
                {
                    if (obj.Name.Contains(nickname))
                        objs.Add(obj);
                }

            }
            return objs;
        }

        /*/
        Grasshopper.Kernel.Special.GH_NumberSlider AddSlider(IGH_Component targetComponent, int min, int decim, int i, int val, string nickname)
        {
            Grasshopper.Kernel.Special.GH_NumberSlider slid = new Grasshopper.Kernel.Special.GH_NumberSlider();
            slid.NickName = nickname;
            slid.Name = nickname;
            slid.CreateAttributes();
            slid.Attributes.Pivot = new System.Drawing.PointF(
              (float)targetComponent.Attributes.DocObject.Attributes.Bounds.Left - slid.Attributes.Bounds.Width - 130,
              (float)targetComponent.Params.Input[i].Attributes.InputGrip.Y - 10);
            slid.Slider.Maximum = 10;
            slid.Slider.Minimum = min;
            slid.Slider.DecimalPlaces = decim;
            slid.SetSliderValue(val);
            return slid;
        }
        /*/
        Grasshopper.Kernel.Special.GH_NumberSlider AddSlider(IGH_Component targetComponent, int i)
        {
            string nickname = targetComponent.Params.Input[i].NickName;
            Grasshopper.Kernel.Special.GH_NumberSlider slid = new Grasshopper.Kernel.Special.GH_NumberSlider();
            slid.NickName = nickname;
            slid.Name = nickname;
            slid.CreateAttributes();
            slid.Attributes.Pivot = new System.Drawing.PointF(
                (float)targetComponent.Attributes.DocObject.Attributes.Bounds.Left - slid.Attributes.Bounds.Width - 130,
                (float)targetComponent.Params.Input[i].Attributes.InputGrip.Y - 10);

            // Set slider properties based on whether nickname contains specific keywords
            if (nickname.Contains("cfg"))
            {
                slid.Slider.Minimum = 0;
                slid.Slider.Maximum = 10;
                slid.Slider.DecimalPlaces = 1;
                slid.SetSliderValue(8);
            }
            else if (nickname.Contains("seed"))
            {
                slid.Slider.Minimum = -1;
                slid.Slider.Maximum = 10000000000000;
                slid.Slider.DecimalPlaces = 0;
                slid.SetSliderValue(-1);
            }
            else if (nickname.Contains("denoise"))
            {
                slid.Slider.Minimum = 0;
                slid.Slider.Maximum = 1;
                slid.Slider.DecimalPlaces = 2;
                slid.SetSliderValue(1);
            }
            else if (nickname.Contains("steps"))
            {
                slid.Slider.Minimum = 0;
                slid.Slider.Maximum = 100;
                slid.Slider.DecimalPlaces = 0;
                slid.SetSliderValue(10);
            }
            else if (nickname.Contains("width") || nickname.Contains("height"))
            {
                slid.Slider.Minimum = 100;
                slid.Slider.Maximum = 10000;
                slid.Slider.DecimalPlaces = 0;
                slid.SetSliderValue(256);
            }
            else // Default case
            {
                slid.Slider.Minimum = 1;
                slid.Slider.Maximum = 10;
                slid.Slider.DecimalPlaces = 0;
                slid.SetSliderValue(1);
            }

            return slid;
        }

        Grasshopper.Kernel.Special.GH_Panel AddPanel(IGH_Component targetComponent, int i, string nickname, bool enable, int height, int offset)
        {
            Grasshopper.Kernel.Special.GH_Panel panel = new Grasshopper.Kernel.Special.GH_Panel();
            panel.NickName = nickname;
            panel.Name = nickname;
            panel.Attributes = new Grasshopper.Kernel.Special.GH_PanelAttributes(panel);
            panel.Properties.Multiline = enable;
            panel.Properties.DrawPaths = false;
            panel.Properties.DrawIndices = false;
            if (nickname.Contains("positive"))
            {
                panel.SetUserText("grasshopper in the woods, realistic, 4k");
                panel.Properties.Colour = System.Drawing.Color.FromArgb(50, 170, 255);
            }
            else if (nickname.Contains("negative"))
            {
                panel.SetUserText("text, watermark");
                panel.Properties.Colour = System.Drawing.Color.FromArgb(255, 130, 130);
            }
            else if (nickname.Contains("sampler"))
            {
                panel.SetUserText("dpmpp_sde");
            }
            else if (nickname.Contains("scheduler"))
            {
                panel.SetUserText("karras");
            }
            else
            {
                panel.SetUserText("");
            }
            panel.Attributes.Pivot = new System.Drawing.PointF(
              (float)targetComponent.Attributes.DocObject.Attributes.Bounds.Left - panel.Attributes.Bounds.Width - offset,
              (float)targetComponent.Params.Input[i].Attributes.InputGrip.Y);
            panel.Attributes.Bounds = new System.Drawing.RectangleF(
              panel.Attributes.Bounds.Location,
              new System.Drawing.SizeF(panel.Attributes.Bounds.Width, height));
            return panel;
        }

        Grasshopper.Kernel.Parameters.Param_FilePath AddFilePath(IGH_Component targetComponent, int i, string nickname)
        {
            Grasshopper.Kernel.Parameters.Param_FilePath filepath = new Grasshopper.Kernel.Parameters.Param_FilePath();
            filepath.NickName = nickname;
            filepath.Name = nickname;
            filepath.CreateAttributes();
            filepath.AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(i), 0, "filepath");
            filepath.Attributes.Pivot = new System.Drawing.PointF(
              (float)targetComponent.Attributes.DocObject.Attributes.Bounds.Left - filepath.Attributes.Bounds.Width - 50,
              (float)targetComponent.Params.Input[i].Attributes.InputGrip.Y);
            return filepath;
        }

        Grasshopper.Kernel.Special.GH_BooleanToggle AddToggle(IGH_Component targetComponent, int i, string nickname)
        {
            Grasshopper.Kernel.Special.GH_BooleanToggle toggle = new Grasshopper.Kernel.Special.GH_BooleanToggle();
            toggle.NickName = nickname;
            toggle.Name = nickname;
            toggle.Attributes = new Grasshopper.Kernel.Special.GH_BooleanToggleAttributes(toggle);
            toggle.Attributes.Pivot = new System.Drawing.PointF(
              (float)targetComponent.Attributes.DocObject.Attributes.Bounds.Left - toggle.Attributes.Bounds.Width - 25,
              (float)targetComponent.Params.Input[i].Attributes.InputGrip.Y - 10);
            return toggle;
        }

        Grasshopper.Kernel.Special.GH_Panel AddOutPanel(IGH_Component targetComponent)
        {
            // Check if the target component has at least one output parameter
            if (targetComponent.Params.Output.Count > 0)
            {
                var firstOutput = targetComponent.Params.Output[0];

                // Check if a panel is connected to the first output parameter
                bool hasPanel = false;
                foreach (IGH_DocumentObject ighdoc in firstOutput.Recipients)
                {
                    if (ighdoc is Grasshopper.Kernel.Special.GH_Panel)
                    {
                        hasPanel = true;
                        break;
                    }
                }

                // If no panel is connected, create and connect a new panel
                if (!hasPanel)
                {
                    var height = targetComponent.Attributes.Bounds.Height;
                    Grasshopper.Kernel.Special.GH_Panel panel = new Grasshopper.Kernel.Special.GH_Panel();
                    panel.Name = "ComfyUIParameters";
                    panel.NickName = "ComfyUI Parameters";
                    panel.Attributes = new Grasshopper.Kernel.Special.GH_PanelAttributes(panel);
                    panel.Attributes.Bounds = new System.Drawing.RectangleF(0, 0, 300, height);
                    panel.Properties.Multiline = true;
                    panel.Properties.DrawPaths = false;
                    panel.Properties.DrawIndices = true;
                    panel.AddSource(firstOutput);
                    var pivot = firstOutput.Attributes.Pivot;
                    panel.Attributes.Pivot = new System.Drawing.PointF(pivot.X + 60, pivot.Y - (height / 2));

                    return panel;
                }
            }

            // Return null if no panel is added
            return null;
        }

        public override bool Write(GH_IWriter writer)
        {

            writer.SetBoolean("KeepInputs", keepInput);

            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("KeepInputs"))
                keepInput = reader.GetBoolean("KeepInputs");

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
                return Properties.Resources.src;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{1CD9DB16-E94D-49B4-9D93-07EAF442BC6F}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}
