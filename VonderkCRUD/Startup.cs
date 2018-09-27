using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using VonderkCRUD.Models;

[assembly: OwinStartupAttribute(typeof(VonderkCRUD.Startup))]
namespace VonderkCRUD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }


      


    }
}
