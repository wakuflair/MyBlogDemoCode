using System;
using System.Linq.Expressions;
using System.Windows;
using Moq;
using Shouldly;
using Stylet;
using StyletBookStore.Pages;
using StyletIoC;
using Xunit;

namespace StyletBookStore.Test
{
    public class LoginViewModelTest
    {
        private readonly IContainer _container;
        private readonly Mock<IWindowManager> _mockWindowManager;
        private readonly Mock<IChildDelegate> _mockChildDelegate;
        private readonly Expression<Func<IWindowManager, MessageBoxResult>> _showMessageBoxExpr = wm => wm.ShowMessageBox("用户名或密码不正确", "登录失败", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxResult.None, null, null, null);

        public LoginViewModelTest()
        {
            // 使用Moq虚拟IWindowManager
            _mockWindowManager = new Mock<IWindowManager>();
            _mockWindowManager.Setup(_showMessageBoxExpr).Returns(MessageBoxResult.OK);

            // 使用Moq虚拟IChildDelegate
            _mockChildDelegate = new Mock<IChildDelegate>();

            // 向Stylet的IoC中注册服务
            var builder = new StyletIoCBuilder();
            builder.Bind<LoginViewModel>().ToSelf();
            builder.Bind<IWindowManager>().ToInstance(_mockWindowManager.Object);    // 注册IWindowManager
            builder.Bind<IChildDelegate>().ToInstance(_mockChildDelegate.Object);    // 注册IChildDelegate
            _container = builder.BuildContainer();
        }

        #region 测试功能点: 当"用户名"或"密码"为空时, 是不允许登录的("登录"按钮处于禁用状态).

        /// <summary>
        /// 密码未输入, 不允许点击登录
        /// </summary>
        [Fact]
        public void CanLoginTest_NoPassword()
        {
            var vm = _container.Get<LoginViewModel>();

            vm.UserName = "waku";
            vm.Password = String.Empty;
            vm.CanLogin.ShouldBe(false);
        }

        /// <summary>
        /// 用户名未输入, 不允许点击登录
        /// </summary>
        [Fact]
        public void CanLoginTest_NoUserName()
        {
            var vm = _container.Get<LoginViewModel>();

            vm.UserName = string.Empty;
            vm.Password = "123";
            vm.CanLogin.ShouldBe(false);
        }

        /// <summary>
        /// 用户名和密码都输入了, 允许点击登录
        /// </summary>
        [Fact]
        public void CanLoginTest()
        {
            var vm = _container.Get<LoginViewModel>();

            vm.UserName = "waku";
            vm.Password = "123";
            vm.CanLogin.ShouldBe(true);
        }

        #endregion

        #region 测试功能点: 用户名输入"waku", 并且密码输入"123", 点击"登录"按钮, 登录窗口关闭, 回到主窗口.

        /// <summary>
        /// 正确的用户名和密码
        /// </summary>
        [Fact]
        public void LoginTest()
        {
            var vm = _container.Get<LoginViewModel>();
            var childDelegate = _container.Get<IChildDelegate>();

            vm.UserName = "waku";
            vm.Password = "123";
            vm.Parent = childDelegate;
            vm.Login();
            _mockWindowManager.Verify(_showMessageBoxExpr, Times.Never); // 不应该显示消息框
            _mockChildDelegate.Verify(cd => cd.CloseItem(vm, true), Times.Once);    // 应该关闭窗口,并返回true
        }

        #endregion

        #region 否则显示"用户名或密码不正确"的消息框.

        /// <summary>
        /// 用户名错误
        /// </summary>
        [Fact]
        public void LoginTest_WrongUserName()
        {
            var vm = _container.Get<LoginViewModel>();
            vm.UserName = "wrong_username";
            vm.Password = "123";
            vm.Login();
            _mockWindowManager.Verify(_showMessageBoxExpr, Times.Once); // 应该显示消息框
        }

        /// <summary>
        /// 密码错误
        /// </summary>
        [Fact]
        public void LoginTest_WrongPassword()
        {
            var vm = _container.Get<LoginViewModel>();
            vm.UserName = "waku";
            vm.Password = "wrong_password";
            vm.Login();
            _mockWindowManager.Verify(_showMessageBoxExpr, Times.Once); // 应该显示消息框
        }

        #endregion

    }
}
