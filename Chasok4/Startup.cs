using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chasok4.Startup))]
namespace Chasok4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
