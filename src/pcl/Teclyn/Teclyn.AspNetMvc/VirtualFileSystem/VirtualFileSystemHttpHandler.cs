using System.Web;

namespace Teclyn.AspNetMvc.VirtualFileSystem
{
    public class VirtualFileSystemHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            
        }

        public bool IsReusable => false;
    }
}