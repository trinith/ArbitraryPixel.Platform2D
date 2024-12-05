using System;
using System.Collections.Generic;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class FormattedTextLabel_Tests
    {
        private FormattedTextLabel _sut;

        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private ITextObjectBuilder _mockTextObjectBuilder;
        private ITextObjectRenderer _mockTextObjectRenderer;

        private RectangleF _bounds = new RectangleF(200, 100, 400, 300);
        private List<ITextObject> _mockTextObjects;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockTextObjectBuilder = Substitute.For<ITextObjectBuilder>();
            _mockTextObjectRenderer = Substitute.For<ITextObjectRenderer>();

            _mockTextObjects = new List<ITextObject>(
                new ITextObject[]
                {
                    Substitute.For<ITextObject>(),
                    Substitute.For<ITextObject>(),
                }
            );
            _mockTextObjectBuilder.Build(Arg.Any<string>(), new RectangleF(Vector2.Zero, _bounds.Size)).Returns(_mockTextObjects);
        }

        private void Construct()
        {
            _sut = new FormattedTextLabel(_mockEngine, _bounds, _mockSpriteBatch, _mockTextObjectBuilder, _mockTextObjectRenderer, "test format string");
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new FormattedTextLabel(_mockEngine, _bounds, null, _mockTextObjectBuilder, _mockTextObjectRenderer, "abcd");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Builder()
        {
            _sut = new FormattedTextLabel(_mockEngine, _bounds, _mockSpriteBatch, null, _mockTextObjectRenderer, "abcd");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Renderer()
        {
            _sut = new FormattedTextLabel(_mockEngine, _bounds, _mockSpriteBatch, _mockTextObjectBuilder, null, "abcd");
        }

        [TestMethod]
        public void ConstructShouldCallBuildOnTextObjectBuilder()
        {
            Construct();

            _mockTextObjectBuilder.Received(1).Build("test format string", new RectangleF(Vector2.Zero, _bounds.Size));
        }

        [TestMethod]
        public void ConstructShouldCallRendererEnqueueWithResultOfTextObjectBuilderBuild()
        {
            Construct();

            Received.InOrder(
                () =>
                {
                    _mockTextObjectRenderer.Enqueue(_mockTextObjects[0]);
                    _mockTextObjectRenderer.Enqueue(_mockTextObjects[1]);
                }
            );
        }

        [TestMethod]
        public void ConstructShouldCallRendererFlush()
        {
            Construct();

            _mockTextObjectRenderer.Received(1).Flush();
        }

        [TestMethod]
        public void ConstructShouldCallRendererRender()
        {
            Construct();

            _mockTextObjectRenderer.Received(1).Render();
        }
        #endregion

        #region TextFormat Tests
        [TestMethod]
        public void TextFormatShouldCallBuildOnTextObjectBuilder()
        {
            Construct();
            _mockTextObjectBuilder.ClearReceivedCalls();

            _sut.TextFormat = "blahblahblah";

            _mockTextObjectBuilder.Received(1).Build("blahblahblah", new RectangleF(Vector2.Zero, _bounds.Size));
        }

        [TestMethod]
        public void TextFormatShouldCallRendererEnqueueWithResultOfTextObjectBuilderBuild()
        {
            Construct();
            _mockTextObjectRenderer.ClearReceivedCalls();

            _sut.TextFormat = "asdf";

            Received.InOrder(
                () =>
                {
                    _mockTextObjectRenderer.Enqueue(_mockTextObjects[0]);
                    _mockTextObjectRenderer.Enqueue(_mockTextObjects[1]);
                }
            );
        }

        [TestMethod]
        public void TextFormatShouldCallRendererFlush()
        {
            Construct();
            _mockTextObjectRenderer.ClearReceivedCalls();

            _sut.TextFormat = "asdf";

            _mockTextObjectRenderer.Received(1).Flush();
        }

        [TestMethod]
        public void TextFormatShouldCallRendererRender()
        {
            Construct();
            _mockTextObjectRenderer.ClearReceivedCalls();

            _sut.TextFormat = "asdf";

            _mockTextObjectRenderer.Received(1).Render();
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldCallSpriteBatchDrawWithRendererRenderResult()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockTextObjectRenderer.Render().Returns(mockTexture);

            Construct();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, _bounds.Location, Color.White);
        }
        #endregion
    }
}
