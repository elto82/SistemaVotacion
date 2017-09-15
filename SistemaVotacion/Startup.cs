using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemaVotacion.Startup))]
namespace SistemaVotacion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
