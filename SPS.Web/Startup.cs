using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SPS.Web.Startup))]
namespace SPS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
