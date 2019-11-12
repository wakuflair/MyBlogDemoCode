using Stylet;
using StyletBookStore.Pages.Books;
using StyletBookStore.Pages.Home;
using StyletIoC;

namespace StyletBookStore.Pages
{
    public class ShellViewModel : Conductor<Screen>
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
            // 显示首页
            var homeViewModel = _container.Get<HomeViewModel>();
            ActivateItem(homeViewModel);

            // 显示登录窗口
            var loginViewModel = _container.Get<LoginViewModel>();
            var result = _windowManager.ShowDialog(loginViewModel);
            if (result != true)
            {
                RequestClose();
            }

            // 显示书籍
            var indexViewModel = _container.Get<IndexViewModel>();
            ActivateItem(indexViewModel);
        }
    }
}
