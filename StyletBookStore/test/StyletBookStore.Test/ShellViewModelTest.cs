using System;
using System.Linq.Expressions;
using Moq;
using Stylet;
using StyletBookStore.Pages;
using StyletIoC;
using Xunit;

namespace StyletBookStore.Test
{
    public class ShellViewModelTest
    {
        /// <summary>
        /// 为了测试ShellViewModel.OnViewLoaded方法而创建的类
        /// </summary>
        public class ShellViewModelForTest : ShellViewModel
        {
            public ShellViewModelForTest(IContainer container, IWindowManager windowManager) : base(container, windowManager)
            {
            }

            public void LoadView()
            {
                base.OnViewLoaded();
            }
        }

        private readonly IContainer _container;
        private readonly Mock<IChildDelegate> _mockChildDelegate;
        private readonly Expression<Func<IWindowManager, bool?>> _showDialogExpr = wm => wm.ShowDialog(It.IsAny<LoginViewModel>());

        public ShellViewModelTest()
        {
            // 使用Moq虚拟IWindowManager
            var mockWindowManager = new Mock<IWindowManager>();
            mockWindowManager.Setup(_showDialogExpr).Returns(false);

            // 使用Moq虚拟IChildDelegate
            _mockChildDelegate = new Mock<IChildDelegate>();

            // 向Stylet的IoC中注册服务
            var builder = new StyletIoCBuilder();
            builder.Bind<LoginViewModel>().ToSelf();
            builder.Bind<ShellViewModelForTest>().ToSelf();
            builder.Bind<IWindowManager>().ToInstance(mockWindowManager.Object);    // 注册IWindowManager
            _container = builder.BuildContainer();
        }

        #region 测试功能点: 点击登录窗口右上角的"X"按钮,整个应用程序退出.

        /// <summary>
        /// 强制退出了登录窗口, 主窗口也应该关闭
        /// </summary>
        [Fact]
        public void ExitTest()
        {
            // Arrange
            var vm = _container.Get<ShellViewModelForTest>();
            vm.Parent = _mockChildDelegate.Object;

            // Act
            vm.LoadView();

            // Assert
            _mockChildDelegate.Verify(cd => cd.CloseItem(vm, null), Times.Once);    // 应该关闭窗口,并返回true
        }

        #endregion
    }
}