using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class SingleTouchButtonController_AfterTouch_Tests : ButtonControllerBase_Tests
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();

            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());
        }

        #region Update - Touch Persists Tests
        [TestMethod]
        public void UpdateWhenStillTouchedInBoundsShouldHaveStateOfPressed()
        {
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre + new Vector2(2, 2), true));
            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Pressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWhenStillTouchedInBoundsShouldNotFireTouchEvent()
        {
            _sut.Touched += _subscriber;
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre + new Vector2(2, 2), true));
            _sut.Update(new GameTime());

            _subscriber.Received(0)(_sut, Arg.Any<ButtonEventArgs>());
        }
        #endregion

        #region Update - Release Tests
        [TestMethod]
        public void UpdateWhenTouchMovesOutOfBoundsShouldSetStateToUnpressed()
        {
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, true));

            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWhenTouchMovesBackIntoBoundsAfterLeavingShouldSetStateToPressed()
        {
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, true));
            _sut.Update(new GameTime());

            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Pressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWhenReleasedOutOfBoundsShouldSetStateToUnpressed()
        {
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, false));

            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWhenReleasedOutOfBoundsShouldFireReleasedEvent()
        {
            _sut.Released += _subscriber;
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, false));

            _sut.Update(new GameTime());

            _subscriber.Received(1)(_sut, Arg.Any<ButtonEventArgs>());
        }

        [TestMethod]
        public void UpdateWhenReleasedOutOfBoundsShouldFireReleasedEventWithLastKnownLocation()
        {
            bool eventOK = false;
            _sut.Released += _subscriber;
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, false));
            _subscriber.When(x => x(_sut, Arg.Any<ButtonEventArgs>())).Do(
                x =>
                {
                    ButtonEventArgs args = x[1] as ButtonEventArgs;
                    Assert.AreEqual<Vector2>(_sut.TargetButton.Bounds.Centre, args.Location);

                    eventOK = true;
                }
            );

            _sut.Update(new GameTime());

            Assert.IsTrue(eventOK);
        }

        [TestMethod]
        public void UpdateWhenReleasedOutOfBoundsShouldNotFireTappedEvent()
        {
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, false));
            // Force an update here to push the zero into the previous location. Call it a quirk of the engine since we won't actually get a location when releasing ;)
            _sut.Update(new GameTime());

            _sut.Tapped += _subscriber;
            _sut.Update(new GameTime());

            _subscriber.Received(0)(_sut, Arg.Any<ButtonEventArgs>());
        }

        [TestMethod]
        public void UpdateWhenReleasedInBoundsShouldSetStateToUnpressed()
        {
            var surfaceState = new SurfaceState(_sut.TargetButton.Bounds.Centre, false);
            _mockSurfaceInputManager.GetSurfaceState().Returns(surfaceState);

            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWhenReleasedInBoundsShouldFireTappedEvent()
        {
            _sut.Tapped += _subscriber;
            var surfaceState = new SurfaceState(_sut.TargetButton.Bounds.Centre, false);
            _mockSurfaceInputManager.GetSurfaceState().Returns(surfaceState);

            _sut.Update(new GameTime());

            _subscriber.Received(1)(_sut, Arg.Any<ButtonEventArgs>());
        }

        [TestMethod]
        public void UpdateWhenReleasedInBoundsShouldFireTappedEventWithExpectedLocation()
        {
            bool eventOK = false;
            _sut.Tapped += _subscriber;
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, false));
            _subscriber.When(x => x(_sut, Arg.Any<ButtonEventArgs>())).Do(
                x =>
                {
                    ButtonEventArgs args = x[1] as ButtonEventArgs;
                    Assert.AreEqual<Vector2>(_sut.TargetButton.Bounds.Centre, args.Location);

                    eventOK = true;
                }
            );

            _sut.Update(new GameTime());

            Assert.IsTrue(eventOK);
        }
        #endregion

        #region Update - Release
        [TestMethod]
        public void UpdateWhenReleasedShouldClearInputManagerConsumer()
        {
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, false));

            _sut.Update(new GameTime());

            _mockSurfaceInputManager.Received(1).ClearConsumer();
        }
        #endregion
    }
}
