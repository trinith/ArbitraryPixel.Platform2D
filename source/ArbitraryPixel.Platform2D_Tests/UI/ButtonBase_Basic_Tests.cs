using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class ButtonBase_Basic_Tests : ButtonBase_Tests
    {
        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullHostShouldThrowException()
        {
            _sut = new ButtonBase(null, _bounds);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithEmptyBoundsShouldThrowException()
        {
            _sut = new ButtonBase(_mockEngine, RectangleF.Empty);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void TagShouldReturnSetValue()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _sut.Tag = mockTexture;
            Assert.AreEqual<ITexture2D>(mockTexture, (ITexture2D)_sut.Tag);
        }
        #endregion

        #region Update - Misc Tests
        [TestMethod]
        public void DefaultStateShouldBeUnpressed()
        {
            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWithNoTouchShouldHaveStateAsUnpressed()
        {
            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWithTouchOutsideBoundsShouldHaveStateAsUnpressed()
        {
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(Vector2.Zero, true));

            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }
        #endregion

        #region Update - Touch Tests
        [TestMethod]
        public void UpdateWithTouchInsideBoundsShouldHaveStatePressed()
        {
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));

            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Pressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWithTouchInsideBoundsShouldFireOnTouchedEvent()
        {
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Touched += _subscriber;

            _sut.Update(new GameTime());

            _subscriber.Received(1)(_sut, Arg.Any<ButtonEventArgs>());
        }

        [TestMethod]
        public void UpdateWithTouchInsideBoundsShouldFireOnTouchedEventWithExpectedLocation()
        {
            bool eventOK = false;

            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _sut.Touched += _subscriber;
            _subscriber.When(x => x(Arg.Any<object>(), Arg.Any<ButtonEventArgs>())).Do(
                x =>
                {
                    ButtonEventArgs args = x[1] as ButtonEventArgs;
                    Assert.AreEqual<Vector2>(_bounds.Centre, args.Location);
                    eventOK = true;
                }
            );

            _sut.Update(new GameTime());

            Assert.IsTrue(eventOK);
        }
        #endregion

        #region Consumer Tests
        [TestMethod]
        public void UpdateShouldCheckIfSelfIsConsumer()
        {
            _sut.Update(new GameTime());

            _mockEngine.InputManager.Received(1).ShouldConsumeInput(_sut);
        }

        [TestMethod]
        public void UpdateWithTouchInsideBoundsButNotValidConsumerShouldNotSetStatePressed()
        {
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(false);
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));

            _sut.Update(new GameTime());

            Assert.AreEqual<ButtonState>(ButtonState.Unpressed, _sut.State);
        }

        [TestMethod]
        public void UpdateWithTouchInsideBoundsShouldSetConsumer()
        {
            _mockSurfaceInputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));

            _sut.Update(new GameTime());

            _mockSurfaceInputManager.Received(1).SetConsumer(_sut);
        }
        #endregion

        #region Dispose Tests
        [TestMethod]
        public void DisposeWhenSetToInputConsumerShouldClearConsumer()
        {
            _mockEngine.InputManager.ShouldConsumeInput(_sut).Returns(true);

            _sut.Dispose();

            _mockEngine.InputManager.Received(1).ClearConsumer();
        }

        [TestMethod]
        public void DisposeShouldCheckIfSelfCanConsumeInput()
        {
            _sut.Dispose();

            var r = _mockEngine.InputManager.Received(1).ShouldConsumeInput(_sut);
        }

        [TestMethod]
        public void DisposeWhenDisposedShouldNotPerformConsumeInputCheck()
        {
            _sut.Dispose();
            _mockEngine.InputManager.ClearReceivedCalls();

            _sut.Dispose();

            var r = _mockEngine.InputManager.Received(0).ShouldConsumeInput(Arg.Any<object>());
        }
        #endregion
    }
}
