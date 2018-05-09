using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArkTradingEvolvedWeb.Startup))]
namespace ArkTradingEvolvedWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
