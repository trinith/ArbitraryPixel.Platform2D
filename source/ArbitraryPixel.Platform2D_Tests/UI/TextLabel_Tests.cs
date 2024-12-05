using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;
using NSubstitute;
using ArbitraryPixel.Platform2D.Layer;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class TextLabel_Tests
    {
        private TextLabel _sut;

        private IEngine _mockEngine;
        private Vector2 _location = new Vector2(100, 200);
        private SizeF _size = new SizeF(300, 400);
        private ISpriteBatch _mockSpriteBatch;
        private ISpriteFont _mockSpriteFont;
        private string _text = "Test";
        private Color _colour = Color.Pink;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockSpriteFont = Substitute.For<ISpriteFont>();
            _mockSpriteFont.MeasureString(Arg.Any<string>()).Returns(_size);

            _sut = new TextLabel(_mockEngine, _location, _mockSpriteBatch, _mockSpriteFont, _text, _colour);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullSpriteBatchShouldThrowException()
        {
            _sut = new TextLabel(_mockEngine, _location, null, _mockSpriteFont, _text, _colour);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullFontShouldThrowException()
        {
            _sut = new TextLabel(_mockEngine, _location, _mockSpriteBatch, null, _text, _colour);
        }

        [TestMethod]
        public void ConstructShouldCallFontMeasureString()
        {
            _mockSpriteFont.Received(1).MeasureString(_text);
        }

        [TestMethod]
        public void ConstructShouldSetBoundsToMeasuredValue()
        {
            Assert.AreEqual<RectangleF>(new RectangleF(_location, _size), _sut.Bounds);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void FontShouldDefaultToConstructorParameter()
        {
            Assert.AreEqual<ISpriteFont>(_mockSpriteFont, _sut.Font);
        }

        [TestMethod]
        public void FontSetShouldUpdateBounds()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            mockFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 50));

            _sut.Font = mockFont;

            Assert.AreEqual<RectangleF>(new RectangleF(100, 200, 100, 50), _sut.Bounds);
        }

        [TestMethod]
        public void TextShouldDefaultToConstructorParameter()
        {
            Assert.AreEqual<string>(_text, _sut.Text);
        }

        [TestMethod]
        public void TextSetShouldUpdateBounds()
        {
            _mockSpriteFont.MeasureString("othertext").Returns(new SizeF(100, 50));

            _sut.Text = "othertext";

            Assert.AreEqual<RectangleF>(new RectangleF(100, 200, 100, 50), _sut.Bounds);
        }

        [TestMethod]
        public void ColourShouldDefaultToConstructorParameter()
        {
            Assert.AreEqual<Color>(_colour, _sut.Colour);
        }

        [TestMethod]
        public void ColourShouldReturnSetValue()
        {
            _sut.Colour = Color.Brown;
            Assert.AreEqual<Color>(Color.Brown, _sut.Colour);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldCallHostLayerSpriteBatchDraw()
        {
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).DrawString(_mockSpriteFont, _text, _location, _colour);
        }
        #endregion
    }
}
