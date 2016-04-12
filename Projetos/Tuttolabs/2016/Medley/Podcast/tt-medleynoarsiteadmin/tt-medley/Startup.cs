using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(tt_medley.Startup))]
namespace tt_medley
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
