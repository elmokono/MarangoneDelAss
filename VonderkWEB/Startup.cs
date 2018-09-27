using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VonderkWEB.Startup))]
namespace VonderkWEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
