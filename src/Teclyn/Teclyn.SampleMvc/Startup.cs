using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Teclyn.SampleMvc.Startup))]
namespace Teclyn.SampleMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
