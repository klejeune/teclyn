using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Teclyn.AspNetMvc.VirtualFileSystem
{
    public class TeclynVirtualPathProvider : VirtualPathProvider
    {
        public override bool FileExists(string virtualPath)
        {
            if (IsPathVirtual(virtualPath))
            {
                var file = (TeclynVirtualFile)GetFile(virtualPath);
                return file.Exists;
            }
            else
                return Previous.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsPathVirtual(virtualPath))
                return new TeclynVirtualFile(virtualPath);
            else
                return Previous.GetFile(virtualPath);
        }

        private bool IsPathVirtual(string virtualPath)
        {
            String checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith("~/Areas/Teclyn/Views/", StringComparison.OrdinalIgnoreCase);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return null;
        }
    }
}
