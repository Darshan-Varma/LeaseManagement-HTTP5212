using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LeaseManagement.Startup))]
namespace LeaseManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
