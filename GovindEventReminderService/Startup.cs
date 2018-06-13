using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GovindEventReminderService.Startup))]
namespace GovindEventReminderService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
