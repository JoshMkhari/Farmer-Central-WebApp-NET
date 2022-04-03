using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ST1109348.Startup))]
namespace ST1109348
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
