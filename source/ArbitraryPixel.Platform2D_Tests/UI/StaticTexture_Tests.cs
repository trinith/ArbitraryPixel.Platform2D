using System;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class StaticTexture_Tests
    {
        private StaticTexture _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ITexture2D _mockTexture;

        private RectangleF _bounds = new RectangleF(200, 100, 400, 300);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockTexture = Substitute.For<ITexture2D>();
        }

        private void Construct()
        {
            Construct(_mockTexture);
        }

        private void Construct(ITexture2D texture)
        {
            _sut = new StaticTexture(_mockEngine, _bounds, _mockSpriteBatch, texture);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new StaticTexture(_mockEngine, _bounds, null, _mockTexture);
        }
        #endregion

        #region Property Tests
        #region Default Values
        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_SourceRectangle()
        {
            Construct();

            Assert.AreEqual<Rectangle?>(null, _sut.SourceRectangle);
        }

        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_Mask()
        {
            Construct();

            Assert.AreEqual<Color>(Color.White, _sut.Mask);
        }

        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_Rotation()
        {
            Construct();

            Assert.AreEqual<float>(0, _sut.Rotation);
        }

        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_Origin()
        {
            Construct();

            Assert.AreEqual<Vector2>(Vector2.Zero, _sut.Origin);
        }

        [TestMethod]
        public void PropertyShouldDefaultToExpectedValue_SpriteEffects()
        {
            Construct();

            Assert.AreEqual<SpriteEffects>(SpriteEffects.None, _sut.SpriteEffects);
        }
        #endregion

        #region Set Then Get
        [TestMethod]
        public void PropertyShouldReturnSetValue_Texture()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            Construct();

            _sut.Texture = mockTexture;

            Assert.AreSame(mockTexture, _sut.Texture);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_SourceRectangle()
        {
            Construct();

            _sut.SourceRectangle = new Rectangle(20, 10, 40, 30);

            Assert.AreEqual<Rectangle>(new Rectangle(20, 10, 40, 30), _sut.SourceRectangle.Value);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_Mask()
        {
            Construct();

            _sut.Mask = Color.Pink;

            Assert.AreEqual<Color>(Color.Pink, _sut.Mask);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_Rotation()
        {
            Construct();

            _sut.Rotation = MathHelper.PiOver4;

            Assert.AreEqual<float>(MathHelper.PiOver4, _sut.Rotation);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_Origin()
        {
            Construct();

            _sut.Origin = new Vector2(5, 5);

            Assert.AreEqual<Vector2>(new Vector2(5, 5), _sut.Origin);
        }

        [TestMethod]
        public void PropertyShouldReturnSetValue_SpriteEffects()
        {
            Construct();

            _sut.SpriteEffects = SpriteEffects.FlipHorizontally;

            Assert.AreEqual<SpriteEffects>(SpriteEffects.FlipHorizontally, _sut.SpriteEffects);
        }
        #endregion
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldCallSpriteBatchDrawWithExpectedParameters_TestA()
        {
            Construct();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockTexture, _bounds, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        [TestMethod]
        public void DrawShouldCallSpriteBatchDrawWithExpectedParameters_TestB()
        {
            Construct();

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _sut.Texture = mockTexture;
            _sut.SourceRectangle = new Rectangle(20, 10, 40, 30);
            _sut.Mask = Color.Pink;
            _sut.Rotation = MathHelper.PiOver4;
            _sut.Origin = new Vector2(5, 5);
            _sut.SpriteEffects = SpriteEffects.FlipVertically;

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, _bounds, new Rectangle(20, 10, 40, 30), Color.Pink, MathHelper.PiOver4, new Vector2(5, 5), SpriteEffects.FlipVertically, 0);
        }

        [TestMethod]
        public void DrawWithNullTextureShouldNotcallSpriteBatchDraw()
        {
            Construct(null);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(Arg.Any<ITexture2D>(), Arg.Any<RectangleF>(), Arg.Any<Rectangle?>(), Arg.Any<Color>(), Arg.Any<float>(), Arg.Any<Vector2>(), Arg.Any<SpriteEffects>(), Arg.Any<float>());
        }
        #endregion
    }
}
