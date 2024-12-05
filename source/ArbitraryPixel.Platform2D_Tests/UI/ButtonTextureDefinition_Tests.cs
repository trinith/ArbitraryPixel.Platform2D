using System;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class ButtonTextureDefinition_Tests
    {
        private ButtonTextureDefinition _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new ButtonTextureDefinition();
        }

        #region Property Default
        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_ImageNormal()
        {
            Assert.IsNull(_sut.ImageNormal);
        }

        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_ImagePressed()
        {
            Assert.IsNull(_sut.ImagePressed);
        }

        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_MaskNormal()
        {
            Assert.AreEqual<Color>(Color.White, _sut.MaskNormal);
        }

        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_MaskPressed()
        {
            Assert.AreEqual<Color>(Color.White, _sut.MaskPressed);
        }

        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_SpriteEffects()
        {
            Assert.AreEqual<SpriteEffects>(SpriteEffects.None, _sut.SpriteEffects);
        }

        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_GlobalOffset()
        {
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.GlobalOffset);
        }
        #endregion

        #region Property GetSet Tests
        [TestMethod]
        public void PropertyShouldReturnSetValue_ImageNormal()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _sut.ImageNormal = mockTexture;

            Assert.AreSame(mockTexture, _sut.ImageNormal);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_ImagePressed()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _sut.ImagePressed = mockTexture;

            Assert.AreSame(mockTexture, _sut.ImagePressed);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_MaskNormal()
        {
            _sut.MaskNormal = Color.Pink;

            Assert.AreEqual<Color>(Color.Pink, _sut.MaskNormal);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_MaskPressed()
        {
            _sut.MaskPressed = Color.Pink;

            Assert.AreEqual<Color>(Color.Pink, _sut.MaskPressed);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_SpriteEffects()
        {
            _sut.SpriteEffects = SpriteEffects.FlipHorizontally;

            Assert.AreEqual<SpriteEffects>(SpriteEffects.FlipHorizontally, _sut.SpriteEffects);
        }
        #endregion

        #region Get Method Tests
        [TestMethod]
        public void GetMethodShouldReturnExpectedValueForState_Pressed_Texture()
        {
            _sut.ImageNormal = Substitute.For<ITexture2D>();
            _sut.ImagePressed = Substitute.For<ITexture2D>();

            Assert.AreSame(_sut.ImagePressed, _sut.GetTexture(ButtonState.Pressed));
        }

        [TestMethod]
        public void GetMethodShouldReturnExpectedValueForState_Unpressed_Texture()
        {
            _sut.ImageNormal = Substitute.For<ITexture2D>();
            _sut.ImagePressed = Substitute.For<ITexture2D>();

            Assert.AreSame(_sut.ImageNormal, _sut.GetTexture(ButtonState.Unpressed));
        }

        [TestMethod]
        public void GetMethodShouldReturnExpectedValueForState_Pressed_Mask()
        {
            _sut.MaskNormal = Color.Pink;
            _sut.MaskPressed = Color.Purple;

            Assert.AreEqual<Color>(_sut.MaskPressed, _sut.GetMask(ButtonState.Pressed));
        }

        [TestMethod]
        public void GetMethodShouldReturnExpectedValueForState_Unpressed_Mask()
        {
            _sut.MaskNormal = Color.Pink;
            _sut.MaskPressed = Color.Purple;

            Assert.AreEqual<Color>(_sut.MaskNormal, _sut.GetMask(ButtonState.Unpressed));
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DrawWithNullParameterShouldThrowException_Host()
        {
            _sut.Draw(null, Substitute.For<ISpriteBatch>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DrawWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut.Draw(Substitute.For<IButton>(), null);
        }

        [TestMethod]
        public void DrawWithNullTextureShouldNotDraw()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            IButton mockButton = Substitute.For<IButton>();
            mockButton.State.Returns(ButtonState.Unpressed);

            _sut.ImageNormal = null;

            _sut.Draw(mockButton, mockSpriteBatch);

            mockSpriteBatch.Received(0).Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Arg.Any<Rectangle?>(), Arg.Any<Color>(), Arg.Any<float>(), Arg.Any<Vector2>(), Arg.Any<SpriteEffects>(), Arg.Any<float>());
        }

        [TestMethod]
        public void DrawShouldDrawWithExpectedValues()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            IButton mockButton = Substitute.For<IButton>();
            mockButton.State.Returns(ButtonState.Unpressed);
            mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));

            _sut.ImageNormal = Substitute.For<ITexture2D>();
            _sut.ImageNormal.Width.Returns(32);
            _sut.ImageNormal.Height.Returns(16);
            _sut.MaskNormal = Color.Pink;
            _sut.SpriteEffects = SpriteEffects.FlipVertically;

            RectangleF expectedBounds = new RectangleF(
                200f + 400f / 2f - 32f / 2f,
                100f + 300f / 2f - 16f / 2f,
                32,
                16
            );

            _sut.Draw(mockButton, mockSpriteBatch);

            mockSpriteBatch.Received(1).Draw(_sut.ImageNormal, expectedBounds, null, Color.Pink, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
        }

        [TestMethod]
        public void DrawWithGlobalOffsetShouldDrawWithExpectedValues()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            IButton mockButton = Substitute.For<IButton>();
            mockButton.State.Returns(ButtonState.Unpressed);
            mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));

            _sut.ImageNormal = Substitute.For<ITexture2D>();
            _sut.ImageNormal.Width.Returns(32);
            _sut.ImageNormal.Height.Returns(16);
            _sut.MaskNormal = Color.Pink;
            _sut.SpriteEffects = SpriteEffects.FlipVertically;

            RectangleF expectedBounds = new RectangleF(
                200f + 400f / 2f - 32f / 2f,
                100f + 300f / 2f - 16f / 2f,
                32,
                16
            );

            _sut.Draw(mockButton, mockSpriteBatch);

            mockSpriteBatch.Received(1).Draw(_sut.ImageNormal, expectedBounds, null, Color.Pink, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
        }
        #endregion
    }
}
