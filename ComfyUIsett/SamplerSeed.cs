using Grasshopper.Kernel;
using System;

namespace ComfyUIsett
{
    public class SamplerSeed : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        /// 
        public SamplerSeed()
          : base("Sampler Seed", "SamplerSeed", "Sampler Seed",
                   "Params", "ComfyUI")
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Fixed seed", "Fixed seed", "Fixed seed", GH_ParamAccess.item, -1);
            pManager.AddBooleanParameter("New Seed", "New Seed", "Use Button to generate Random seed ", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Seed", "Seed", "Seed", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        /// 

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int fixedseed = -1;
            bool randomseed = false;

            if (!DA.GetData(0, ref fixedseed))
                return;

            if (!DA.GetData(1, ref randomseed))
                return;

            int newseed = 0;
            Random random = new Random();
            if (!randomseed)
            {
                newseed = random.Next();
            }

            if (fixedseed == -1)
            {
                DA.SetData(0, newseed);
                Message = newseed.ToString();
            }
            else
            {
                DA.SetData(0, fixedseed);
                Message = fixedseed.ToString();
            }
            
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
                return Properties.Resources.seed;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{8DA82A0C-2AAB-4155-A1EB-E6E489ECCF2D}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}

