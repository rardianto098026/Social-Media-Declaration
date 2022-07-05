using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Annual_Compliance_Declaration.Startup))]
namespace Annual_Compliance_Declaration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
