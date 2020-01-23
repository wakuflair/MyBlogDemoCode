using Stylet;
using StyletBookStore.Pages;
using StyletBookStore.Services;
using StyletIoC;

namespace StyletBookStore
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
            builder.Bind<IBookService>().To<BookService>();
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }
    }
}
