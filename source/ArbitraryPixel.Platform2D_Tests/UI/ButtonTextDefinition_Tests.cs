using System;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class ButtonTextDefinition_Tests
    {
        private ButtonTextDefinition _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new ButtonTextDefinition();
        }

        #region Constructor Tests
        [TestMethod]
        public void ConstructShouldCreateTextNormal()
        {
            Assert.IsNotNull(_sut.TextNormal);
        }

        [TestMethod]
        public void ConstructShouldCreateTextPressed()
        {
            Assert.IsNotNull(_sut.TextPressed);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertySetToNullShouldThrowException_TextNormal()
        {
            _sut.TextNormal = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertySetToNullShouldThrowException_TextPressed()
        {
            _sut.TextPressed = null;
        }

        [TestMethod]
        public void TextNormalShouldReturnSetValue()
        {
            ITextDefinition mockDefinition = Substitute.For<ITextDefinition>();
            _sut.TextNormal = mockDefinition;

            Assert.AreSame(mockDefinition, _sut.TextNormal);
        }

        [TestMethod]
        public void TextPressedShouldReturnSetValue()
        {
            ITextDefinition mockDefinition = Substitute.For<ITextDefinition>();
            _sut.TextPressed = mockDefinition;

            Assert.AreSame(mockDefinition, _sut.TextPressed);
        }

        [TestMethod]
        public void GlobalOffsetShouldDefaultToVector2Zero()
        {
            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.GlobalOffset);
        }

        [TestMethod]
        public void TextAlignShouldDefaultToCentre()
        {
            Assert.AreEqual<TextLineAlignment>(TextLineAlignment.Centre, _sut.Alignment);
        }

        [TestMethod]
        public void TextAlignShouldReturnSetValue()
        {
            _sut.Alignment = TextLineAlignment.Left;

            Assert.AreEqual<TextLineAlignment>(TextLineAlignment.Left, _sut.Alignment);
        }
        #endregion

        #region Get Method Tests
        [TestMethod]
        public void GetMethodShouldReturnExpectedValueForState_Pressed_Text()
        {
            _sut.TextNormal = Substitute.For<ITextDefinition>();
            _sut.TextPressed = Substitute.For<ITextDefinition>();

            _sut.TextNormal.Text.Returns("Normal");
            _sut.TextPressed.Text.Returns("Pressed");

            Assert.AreEqual<string>("Pressed", _sut.GetText(ButtonState.Pressed));
        }

        [TestMethod]
        public void GetMethodShouldReturnExpectedValueForState_Unpressed_Text()
        {
            _sut.TextNormal = Substitute.For<ITextDefinition>();
            _sut.TextPressed = Substitute.For<ITextDefinition>();

            _sut.TextNormal.Text.Returns("Normal");
            _sut.TextPressed.Text.Returns("Pressed");

            Assert.AreEqual<string>("Normal", _sut.GetText(ButtonState.Unpressed));
        }

        [TestMethod]
        public void GetMethodShouldReturnExpectedValueForState_Pressed_Font()
        {
            _sut.TextNormal = Substitute.For<ITextDefinition>();
            _sut.TextPressed = Substitute.For<ITextDefinition>();

            _sut.TextNormal.Font.Returns(Substitute.For<ISpriteFont>());
            _sut.TextPressed.Font.Returns(Substitute.For<ISpriteFont>());

            Assert.AreEqual<ISpriteFont>(_sut.TextPressed.Font, _sut.GetFont(ButtonState.Pressed));
        }

        [TestMethod]
        public void GetMethodShouldReturnExpectedValueForState_Unpressed_Font()
        {
            _sut.TextNormal = Substitute.For<ITextDefinition>();
            _sut.TextPressed = Substitute.For<ITextDefinition>();

            _sut.TextNormal.Font.Returns(Substitute.For<ISpriteFont>());
            _sut.TextPressed.Font.Returns(Substitute.For<ISpriteFont>());

            Assert.AreEqual<ISpriteFont>(_sut.TextNormal.Font, _sut.GetFont(ButtonState.Unpressed));
        }

        [TestMethod]
        public void GetMethodShouldReturnExpectedValueForState_Pressed_Colour()
        {
            _sut.TextNormal = Substitute.For<ITextDefinition>();
            _sut.TextPressed = Substitute.For<ITextDefinition>();

            _sut.TextNormal.Colour.Returns(Color.Pink);
            _sut.TextPressed.Colour.Returns(Color.Purple);

            Assert.AreEqual<Color>(Color.Purple, _sut.GetColour(ButtonState.Pressed));
        }

        [TestMethod]
        public void GetMethodShouldReturnExpectedValueForState_Unpressed_Colour()
        {
            _sut.TextNormal = Substitute.For<ITextDefinition>();
            _sut.TextPressed = Substitute.For<ITextDefinition>();

            _sut.TextNormal.Colour.Returns(Color.Pink);
            _sut.TextPressed.Colour.Returns(Color.Purple);

            Assert.AreEqual<Color>(Color.Pink, _sut.GetColour(ButtonState.Unpressed));
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
        public void DrawWithNullFontShouldNotDraw()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            IButton mockButton = Substitute.For<IButton>();
            mockButton.State.Returns(ButtonState.Unpressed);

            _sut.TextPressed = Substitute.For<ITextDefinition>();
            _sut.TextPressed.Font.Returns((ISpriteFont)null);

            _sut.Draw(mockButton, mockSpriteBatch);

            mockSpriteBatch.Received(0).DrawString(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawShouldDrawWithExpectedValues()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            IButton mockButton = Substitute.For<IButton>();
            mockButton.State.Returns(ButtonState.Pressed);
            mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));

            _sut.TextPressed = Substitute.For<ITextDefinition>();
            _sut.TextPressed.Font = Substitute.For<ISpriteFont>();
            _sut.TextPressed.Font.MeasureString(Arg.Any<string>()).Returns(new SizeF(50, 20));
            _sut.TextPressed.Colour.Returns(Color.Pink);
            _sut.TextPressed.Text.Returns("Test");

            _sut.Draw(mockButton, mockSpriteBatch);

            mockSpriteBatch.Received(1).DrawString(
                _sut.TextPressed.Font,
                "Test",
                new Vector2(400 - 25, 250 - 10),
                Color.Pink
            );
        }

        [TestMethod]
        public void DrawWithGlobalOffsetShouldDrawWithExpectedValues()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            IButton mockButton = Substitute.For<IButton>();
            mockButton.State.Returns(ButtonState.Pressed);
            mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));

            _sut.TextPressed = Substitute.For<ITextDefinition>();
            _sut.TextPressed.Font = Substitute.For<ISpriteFont>();
            _sut.TextPressed.Font.MeasureString(Arg.Any<string>()).Returns(new SizeF(50, 20));
            _sut.TextPressed.Colour.Returns(Color.Pink);
            _sut.TextPressed.Text.Returns("Test");
            _sut.GlobalOffset = new Vector2(7, 5);

            _sut.Draw(mockButton, mockSpriteBatch);

            mockSpriteBatch.Received(1).DrawString(
                _sut.TextPressed.Font,
                "Test",
                new Vector2(400 - 25, 250 - 10) + new Vector2(7, 5),
                Color.Pink
            );
        }

        [TestMethod]
        public void DrawWithAlignmentLeftShouldDrawWithExpectedPosition()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            IButton mockButton = Substitute.For<IButton>();
            mockButton.State.Returns(ButtonState.Pressed);
            mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));

            _sut.Alignment = TextLineAlignment.Left;
            _sut.TextPressed = Substitute.For<ITextDefinition>();
            _sut.TextPressed.Font = Substitute.For<ISpriteFont>();
            _sut.TextPressed.Font.MeasureString(Arg.Any<string>()).Returns(new SizeF(50, 20));
            _sut.TextPressed.Colour.Returns(Color.Pink);
            _sut.TextPressed.Text.Returns("Test");
            _sut.GlobalOffset = new Vector2(7, 5);

            _sut.Draw(mockButton, mockSpriteBatch);

            mockSpriteBatch.Received(1).DrawString(
                _sut.TextPressed.Font,
                "Test",
                new Vector2(200, 250 - 10) + new Vector2(7, 5),
                Color.Pink
            );
        }

        [TestMethod]
        public void DrawWithAlignmentRightShouldDrawWithExpectedPosition()
        {
            ISpriteBatch mockSpriteBatch = Substitute.For<ISpriteBatch>();
            IButton mockButton = Substitute.For<IButton>();
            mockButton.State.Returns(ButtonState.Pressed);
            mockButton.Bounds.Returns(new RectangleF(200, 100, 400, 300));

            _sut.Alignment = TextLineAlignment.Right;
            _sut.TextPressed = Substitute.For<ITextDefinition>();
            _sut.TextPressed.Font = Substitute.For<ISpriteFont>();
            _sut.TextPressed.Font.MeasureString(Arg.Any<string>()).Returns(new SizeF(50, 20));
            _sut.TextPressed.Colour.Returns(Color.Pink);
            _sut.TextPressed.Text.Returns("Test");
            _sut.GlobalOffset = new Vector2(7, 5);

            _sut.Draw(mockButton, mockSpriteBatch);

            mockSpriteBatch.Received(1).DrawString(
                _sut.TextPressed.Font,
                "Test",
                new Vector2(600 - 50, 250 - 10) + new Vector2(7, 5),
                Color.Pink
            );
        }
        #endregion
    }
}
