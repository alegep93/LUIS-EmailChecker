using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LUIS_EmailCheckerMVC.Startup))]
namespace LUIS_EmailCheckerMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
