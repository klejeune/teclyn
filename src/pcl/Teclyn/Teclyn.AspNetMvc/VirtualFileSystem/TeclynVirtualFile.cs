using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Teclyn.AspNetMvc.VirtualFileSystem
{
    public class TeclynVirtualFile : VirtualFile
    {
        private string resourceName;

        private static string ConvertVirtualPath(string origin)
        {
            return typeof(TeclynVirtualFile).Assembly.GetName().Name
                 + ".Mvc."
                 + VirtualPathUtility.MakeRelative("~/Areas/Teclyn/Views", origin)
                     .Replace('/', '.');
        }

        public TeclynVirtualFile(string virtualPath) : base(virtualPath)
        {
            this.resourceName = ConvertVirtualPath(virtualPath);
        }

        public override Stream Open()
        {
            var manifestResourceName = this.GetType().Assembly.GetManifestResourceNames()
             .First(name => string.Equals(name, this.resourceName, StringComparison.OrdinalIgnoreCase));

            return this.GetType().Assembly.GetManifestResourceStream(manifestResourceName);
        }
        public bool Exists
        {
            get
            {
                return this.GetType().Assembly.GetManifestResourceNames()
                    .Any(manifestResourceName => string.Equals(manifestResourceName, this.resourceName, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}