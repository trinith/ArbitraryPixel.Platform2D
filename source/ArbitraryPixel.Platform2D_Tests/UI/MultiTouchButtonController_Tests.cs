using System;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.UI.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class MultiTouchButtonController_Tests
    {
        private IButton _mockButton;

        private RectangleF _bounds = new RectangleF(50, 100, 200, 100);

        private MultiTouchButtonController _sut;

        [TestInitialize]
        public void Init()
        {
            _mockButton = Substitute.For<IButton>();
            _mockButton.Bounds.Returns(_bounds);
            _mockButton.Host.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(x => x[0]);
        }

        private void Construct()
        {
            _sut = new MultiTouchButtonController(_mockButton);
        }

        #region Property Tests
        [TestMethod]
        public void StateShouldDefaultToUnpressed()
        {
            Construct();

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }

        [TestMethod]
        public void IsDisposedShouldDefaultToFalse()
        {
            Construct();

            Assert.IsFalse(_sut.IsDisposed);
        }

        [TestMethod]
        public void TargetButtonShouldReturnConstructorParameter()
        {
            Construct();

            Assert.AreSame(_mockButton, _sut.TargetButton);
        }
        #endregion

        #region Dispose Tests
        [TestMethod]
        public void DisposeWhenNotTrackingTouchShouldNotCallInputManagerClearConsumer()
        {
            Construct();

            _sut.Dispose();

            _mockButton.Host.InputManager.Received(0).ClearConsumer(Arg.Any<int>());
        }

        [TestMethod]
        public void DisposeWhenTrackingTouchShouldClearConsumer()
        {
            Construct();
            _mockButton.Host.InputManager.ShouldConsumeInput(123, _sut).Returns(true);
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(123, TouchLocationState.Pressed, _bounds.Centre) })));
            _sut.Update(new GameTime());

            _sut.Dispose();

            _mockButton.Host.InputManager.Received(1).ClearConsumer(123);
        }
        #endregion

        #region Update Tests - Initial Touch
        [TestMethod]
        public void UpdateShouldGetSurfaceState()
        {
            Construct();

            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.Received(1).GetSurfaceState();
        }

        [TestMethod]
        public void UpdateWithNewConsumableTouchInBoundsShouldSetConsumer()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);

            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.Received(1).SetConsumer(222, _sut);
        }

        [TestMethod]
        public void UpdateWithNewConsumableTouchInBoundsShouldTriggerOnTouchedEvent()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            var subscriber = Substitute.For<EventHandler<ButtonEventArgs>>();
            _sut.Touched += subscriber;

            _sut.Update(new GameTime());

            subscriber.Received(1)(_sut, Arg.Is<ButtonEventArgs>(x => x.Location == _bounds.Centre));
        }

        [TestMethod]
        public void UpdateWithNewConsumableTouchInBoundsShouldSetStateToPressed()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);

            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Pressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWithNewConsumableTouchOutOfBoundsShouldNotSetConsumer()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre + new Vector2(_bounds.Width + 5, 0)) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);

            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.Received(0).SetConsumer(Arg.Any<int>(), Arg.Any<object>());
        }

        [TestMethod]
        public void UpdateWithNewConsumableTouchOutOfBoundsShouldNotTriggerOnTouchedEvent()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre + new Vector2(_bounds.Width + 5, 0)) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            var subscriber = Substitute.For<EventHandler<ButtonEventArgs>>();
            _sut.Touched += subscriber;

            _sut.Update(new GameTime());

            subscriber.Received(0)(Arg.Any<object>(), Arg.Any<ButtonEventArgs>());
        }

        [TestMethod]
        public void UpdateWithNewConsumableTouchOutOfBoundsShouldNotSetStateToPressed()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre + new Vector2(_bounds.Width + 5, 0)) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);

            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWithUnconsumableTouchShouldNotSetConsumer()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(false);

            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.Received(0).SetConsumer(Arg.Any<int>(), Arg.Any<object>());
        }

        [TestMethod]
        public void UpdateWithUnconsumableTouchShouldNotTriggerOnTouchedEvent()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(false);
            var subscriber = Substitute.For<EventHandler<ButtonEventArgs>>();
            _sut.Touched += subscriber;

            _sut.Update(new GameTime());

            subscriber.Received(0)(Arg.Any<object>(), Arg.Any<ButtonEventArgs>());
        }

        [TestMethod]
        public void UpdateWithStaleTouchShouldNotCheckConsumer()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Moved, _bounds.Centre) })));

            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.Received(0).ShouldConsumeInput(Arg.Any<int>(), Arg.Any<object>());
        }
        #endregion

        #region Update Tests - After Touch
        [TestMethod]
        public void UpdateAfterTouchWithValidReleasedTouchShouldTriggerOnReleasedEvent()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            var subscriber = Substitute.For<EventHandler<ButtonEventArgs>>();
            _sut.Released += subscriber;
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Released, _bounds.Centre) })));
            _sut.Update(new GameTime());

            subscriber.Received(1)(_sut, Arg.Is<ButtonEventArgs>(x => x.Location == _bounds.Centre));
        }

        [TestMethod]
        public void UpdateAfterTouchWithValidReleasedTouchShouldClearConsumer()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            var subscriber = Substitute.For<EventHandler<ButtonEventArgs>>();
            _sut.Released += subscriber;
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Released, _bounds.Centre) })));
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.Received(1).ClearConsumer(222);
        }

        [TestMethod]
        public void UpdateAfterTouchWithValidReleasedTouchShouldSetStateToUnpressed()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Released, _bounds.Centre) })));
            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }

        [TestMethod]
        public void UpdateAfterTouchWithValidReleasedTouchInBoundsShouldTriggerOnTappedEvent()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            var subscriber = Substitute.For<EventHandler<ButtonEventArgs>>();
            _sut.Tapped += subscriber;
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Released, _bounds.Centre) })));
            _sut.Update(new GameTime());

            subscriber.Received(1)(_sut, Arg.Is<ButtonEventArgs>(x => x.Location == _bounds.Centre));
        }

        [TestMethod]
        public void UpdateAfterTouchWithValidReleasedTouchOutOfBoundsShouldNotTriggerOnTappedEvent()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            var subscriber = Substitute.For<EventHandler<ButtonEventArgs>>();
            _sut.Tapped += subscriber;
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Released, _bounds.Centre + new Vector2(_bounds.Width, 0)) })));
            _sut.Update(new GameTime());

            subscriber.Received(0)(Arg.Any<object>(), Arg.Any<ButtonEventArgs>());
        }

        [TestMethod]
        public void UpdateAfterTouchWithValidNonReleasedTouchInBoundsShouldSetStateToPressed_FromPressed()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Pressed, _sut.State);
        }

        [TestMethod]
        public void UpdateAfterTouchWithValidNonReleasedTouchInBoundsShouldSetStateToPressed_FromUnpressed()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre + new Vector2(_bounds.Width, 0)) })));
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Pressed, _sut.State);
        }

        [TestMethod]
        public void UpdateAfterTouchWithValidNonReleasedTouchOutOfBoundsShouldSetStateToUnPressed()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre + new Vector2(_bounds.Width, 0)) })));
            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }

        [TestMethod]
        public void UpdateAfterTouchWithInvalidTouchShouldClearConsumer()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { })));
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.Received(1).ClearConsumer(222);
        }

        [TestMethod]
        public void UpdateAfterTouchWithInvalidTouchShouldSetStateToUnpressed()
        {
            Construct();
            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { new TouchLocation(222, TouchLocationState.Pressed, _bounds.Centre) })));
            _mockButton.Host.InputManager.ShouldConsumeInput(222, _sut).Returns(true);
            _sut.Update(new GameTime());

            _mockButton.Host.InputManager.GetSurfaceState().Returns(new SurfaceState(new TouchCollection(new TouchLocation[] { })));
            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }
        #endregion
    }
}
