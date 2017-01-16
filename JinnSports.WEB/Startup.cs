using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JinnSports.WEB.Startup))]
namespace JinnSports.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}