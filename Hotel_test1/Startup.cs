using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hotel_test1.Startup))]
namespace Hotel_test1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
