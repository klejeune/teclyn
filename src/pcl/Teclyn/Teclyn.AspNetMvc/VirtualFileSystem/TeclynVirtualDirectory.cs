using System.Web.Hosting;

namespace Teclyn.AspNetMvc.VirtualFileSystem
{
    public class TeclynVirtualDirectory : VirtualDirectory
    {
        public TeclynVirtualDirectory(string virtualPath) : base(virtualPath) { }

        public bool Exists
        {
            get { return false; }
        }

        public override System.Collections.IEnumerable Children
        {
            get { return new object[0]; }
        }

        public override System.Collections.IEnumerable Directories
        {
            get { return new object[0]; }
        }

        public override System.Collections.IEnumerable Files
        {
            get { return new object[0]; }
        }
    }
}