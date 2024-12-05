using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Common.Screen;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.UI.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class ButtonControllerBase_Tests
    {
        protected EventHandler<ButtonEventArgs> _subscriber = null;

        protected IButton _mockButton;

        protected IEngine _mockEngine = null;
        protected ISurfaceInputManager _mockSurfaceInputManager = null;
        protected IScreenManager _mockScreenManager = null;

        protected RectangleF _bounds = RectangleF.Empty;
        protected ButtonControllerBase _sut = null;

        [TestInitialize]
        public void Initialize()
        {
            _subscriber = Substitute.For<EventHandler<ButtonEventArgs>>();

            _mockSurfaceInputManager = Substitute.For<ISurfaceInputManager>();
            _mockScreenManager = Substitute.For<IScreenManager>();

            _mockEngine = Substitute.For<IEngine>();
            _mockEngine.InputManager.Returns(_mockSurfaceInputManager);
            _mockEngine.ScreenManager.Returns(_mockScreenManager);

            _mockSurfaceInputManager.IsActive.Returns(true);
            _mockSurfaceInputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(x => (Vector2)x[0]);

            _bounds = new RectangleF(100, 100, 100, 100);

            _mockButton = Substitute.For<IButton>();
            _mockButton.Host.Returns(_mockEngine);
            _mockButton.Bounds.Returns(_bounds);

            _sut = new SingleTouchButtonController(_mockButton);

            OnInitialized();
        }

        /// <summary>
        /// Runs after a test has initialized.
        /// </summary>
        protected virtual void OnInitialized()
        {
        }

        // NOTE: Do not write tests cases in this class... it's just a base class to stage tests for button base.
        // If you do, you'll end up creating a test case for every class that extends this one ;)
    }
}
