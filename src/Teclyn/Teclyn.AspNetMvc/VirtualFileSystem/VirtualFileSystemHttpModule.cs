using System.Linq;
using System.Web;

namespace Teclyn.AspNetMvc.VirtualFileSystem
{
    public class VirtualFileSystemHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            //HttpApplication.RegisterModule(this.GetType());

            context.PostMapRequestHandler += Context_PostMapRequestHandler;
        }

        private static string[] staticFiles = new[]
        {
            "~/teclyn/vendor/bootstrap/bootstrap.min.js",
        };

        public static void Context_PostMapRequestHandler(object sender, System.EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            if (staticFiles.Contains(context.Request.Url.ToString()))
            {
                IHttpHandler myHandler = new VirtualFileSystemHttpHandler();
                context.Handler = myHandler;
            }
        }

        public void Dispose()
        {
        }
    }
}