using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace ComfyUIsett
{
    public class ComfyUIsettInfo : GH_AssemblyInfo
    {
        public override string Name => "ComfyUIsett";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "ComfyUI Settings";

        public override Guid Id => new Guid("985dda9c-66f5-4eb3-a51a-229781f9bdd1");

        //Return a string identifying you or your company.
        public override string AuthorName => "Seghier Mohamed Abdelaziz";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "seghierdesign@gmail.com";
    }
}