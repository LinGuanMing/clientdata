using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(clientdata.Startup))]
namespace clientdata
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
