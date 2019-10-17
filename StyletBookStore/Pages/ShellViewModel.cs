using Stylet;
using StyletIoC;

namespace StyletBookStore.Pages
{
    public class ShellViewModel : Screen
    {
        private readonly IContainer _container;
        private readonly IWindowManager _windowManager;

        public ShellViewModel(IContainer container, IWindowManager windowManager)
        {
            _container = container;
            _windowManager = windowManager;
        }

        protected override void OnViewLoaded()
        {
            var loginViewModel = _container.Get<LoginViewModel>();
            var result = _windowManager.ShowDialog(loginViewModel);
            if (result != true)
            {
                RequestClose();
            }
        }
    }
}
