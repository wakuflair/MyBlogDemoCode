using Stylet;
using StyletBookStore.Pages;
using StyletIoC;

namespace StyletBookStore
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }

        protected override void OnStart()
        {
            Stylet.Logging.LogManager.Enabled = true;
        }
    }
}
