using System;
using System.Windows;
using Stylet;

namespace StyletBookStore.Pages
{
    public class LoginViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

            /// <summary>
            /// 用户名
            /// </summary>
            public string? UserName { get; set; }

            /// <summary>
            /// 密码
            /// </summary>
            public string? Password { get; set; }

        public LoginViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            // 设计的非常安全的用户验证机制:)
            if (UserName != "waku" || Password != "123")
            {
                _windowManager.ShowMessageBox("用户名或密码不正确", "登录失败", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            RequestClose(true);
        }

        /// <summary>
        /// 登录的防护属性
        /// </summary>
        public bool CanLogin => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
    }
}