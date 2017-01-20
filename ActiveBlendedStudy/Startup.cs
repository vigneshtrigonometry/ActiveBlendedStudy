using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ActiveBlendedStudy.Startup))]
namespace ActiveBlendedStudy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
