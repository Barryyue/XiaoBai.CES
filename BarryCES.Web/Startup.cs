using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BarryCES.Web.Startup))]
namespace BarryCES.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
