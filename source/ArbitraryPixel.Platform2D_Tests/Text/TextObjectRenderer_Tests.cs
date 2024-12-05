using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using NSubstitute;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace ArbitraryPixel.Platform2D_Tests.Text
{
    [TestClass]
    public class TextObjectRenderer_Tests
    {
        private IRenderTargetFactory _mockRenderTargetFactory;
        private IGrfxDevice _mockDevice;
        private ISpriteBatch _mockSpriteBatch;
        private Rectangle _testBounds = new Rectangle(0, 0, 100, 100);

        private IRenderTarget2D _mockStaticTarget;
        private IRenderTarget2D _mockFinalTarget;

        private TextObjectRenderer _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockRenderTargetFactory = Substitute.For<IRenderTargetFactory>();
            _mockDevice = Substitute.For<IGrfxDevice>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockRenderTargetFactory.Create(Arg.Any<IGrfxDevice>(), Arg.Any<int>(), Arg.Any<int>(), RenderTargetUsage.PreserveContents).Returns(_mockStaticTarget = Substitute.For<IRenderTarget2D>());
            _mockRenderTargetFactory.Create(Arg.Any<IGrfxDevice>(), Arg.Any<int>(), Arg.Any<int>(), RenderTargetUsage.DiscardContents).Returns(_mockFinalTarget = Substitute.For<IRenderTarget2D>());

            _mockStaticTarget.Width.Returns(_testBounds.Width);
            _mockStaticTarget.Height.Returns(_testBounds.Height);

            _mockFinalTarget.Width.Returns(_testBounds.Width);
            _mockFinalTarget.Height.Returns(_testBounds.Height);

            _sut = new TextObjectRenderer(_mockRenderTargetFactory, _mockDevice, _mockSpriteBatch, _testBounds);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullRenderTargetFactoryShouldThrowException()
        {
            _sut = new TextObjectRenderer(null, _mockDevice, _mockSpriteBatch, _testBounds);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullDeviceShouldThrowException()
        {
            _sut = new TextObjectRenderer(_mockRenderTargetFactory, null, _mockSpriteBatch, _testBounds);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullSpriteBatchShouldThrowException()
        {
            _sut = new TextObjectRenderer(_mockRenderTargetFactory, _mockDevice, null, _testBounds);
        }

        [TestMethod]
        public void ConstructShouldCreateRenderTargetWithPreserveContents()
        {
            _mockRenderTargetFactory.Received(1).Create(_mockDevice, 100, 100, RenderTargetUsage.PreserveContents);
        }

        [TestMethod]
        public void ConstructShouldCreateTargetWithDiscardContents()
        {
            _mockRenderTargetFactory.Received(1).Create(_mockDevice, 100, 100, RenderTargetUsage.DiscardContents);
        }

        [TestMethod]
        public void ConstructShouldCallSetDataOnStaticTargetWithExpectedArray()
        {
            Color[] expectedData = new Color[_testBounds.Width * _testBounds.Height];
            for (int i = 0; i < expectedData.Length; i++)
                expectedData[i] = Color.Transparent;

            _mockStaticTarget.Received(1).SetData<Color>(
                Arg.Is<Color[]>(
                    x => x.SequenceEqual(expectedData)
                )
            );
        }

        [TestMethod]
        public void ConstructShouldCallSetDataOnFinalTargetWithExpectedArray()
        {
            Color[] expectedData = new Color[_testBounds.Width * _testBounds.Height];
            for (int i = 0; i < expectedData.Length; i++)
                expectedData[i] = Color.Transparent;

            _mockFinalTarget.Received(1).SetData<Color>(
                Arg.Is<Color[]>(
                    x => x.SequenceEqual(expectedData)
                )
            );
        }
        #endregion

        #region IsComplete Tests
        [TestMethod]
        public void IsCompleteWithNoQueuedItemsShouldReturnTrue()
        {
            Assert.IsTrue(_sut.IsComplete);
        }

        [TestMethod]
        public void IsCompleteWithIncompleteQueuedItemsShouldReturnFalse()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            _sut.Enqueue(mockObject);

            Assert.IsFalse(_sut.IsComplete);
        }

        [TestMethod]
        public void IsCompleteAfterItemProcessedShouldReturnTrue()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            _sut.Enqueue(mockObject);
            _sut.Flush();

            Assert.IsTrue(_sut.IsComplete);
        }
        #endregion

        #region Enqueue Tests
        // Not sure we need specific tests here since we just add to a list. This gets tested via other methods that allow us to confirm data gets put into the list.
        #endregion

        #region Flush Tests
        [TestMethod]
        public void FlushWithQueuedObjectsShouldSetupRenderingAsExpected()
        {
            _sut.Enqueue(Substitute.For<ITextObject>());

            _sut.Flush();

            Received.InOrder(
                () =>
                {
                    _mockDevice.SetRenderTarget(_mockStaticTarget);
                    _mockSpriteBatch.Begin();
                    _mockSpriteBatch.End();
                    _mockDevice.SetRenderTarget(null);
                }
            );
        }

        [TestMethod]
        public void FlushWithNothingQueuedShouldNotCallSpriteBatchDrawString()
        {
            _sut.Flush();

            _mockSpriteBatch.Received(0).DrawString(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void FlushWithNothingQueuedShouldNotSetupRendering()
        {
            _sut.Flush();

            _mockDevice.DidNotReceive().SetRenderTarget(_mockStaticTarget);
            _mockSpriteBatch.DidNotReceive().Begin();
            _mockSpriteBatch.DidNotReceive().End();
            _mockDevice.DidNotReceive().SetRenderTarget(null);
        }

        [TestMethod]
        public void FlushWithIncompleteQueuedObjectShouldCallSpriteBatchDrawStringOnText()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            mockObject.TextDefinition.Text.Returns("FullText");
            mockObject.CurrentText.Returns("Text");
            mockObject.IsComplete.Returns(false);
            _sut.Enqueue(mockObject);

            _sut.Flush();

            _mockSpriteBatch.Received(1).DrawString(Arg.Any<ISpriteFont>(), "Text", Arg.Any<Vector2>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void FlushWithIncompleteQueuedObjectShouldSetShowLengthToFullLength()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            mockObject.TextDefinition.Text.Returns("FullText");
            mockObject.IsComplete.Returns(false);
            _sut.Enqueue(mockObject);

            _sut.Flush();

            mockObject.Received(1).ShowLength = 8;
        }

        [TestMethod]
        public void FlushWithCompleteObjectShouldSetShowLengthToFullLength()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            mockObject.TextDefinition.Text.Returns("FullText");
            mockObject.IsComplete.Returns(true);
            _sut.Enqueue(mockObject);

            _sut.Flush();

            mockObject.Received(1).ShowLength = 8;
        }

        [TestMethod]
        public void FlushWithMultipleObjectsShouldDrawAllObjects_ObjectA()
        {
            ITextObject objA = Substitute.For<ITextObject>();
            objA.CurrentText.Returns("objA");
            ITextObject objB = Substitute.For<ITextObject>();
            objB.CurrentText.Returns("objB");

            _sut.Enqueue(objA);
            _sut.Enqueue(objB);

            _sut.Flush();

            Received.InOrder(
                () =>
                {
                    _mockSpriteBatch.DrawString(Arg.Any<ISpriteFont>(), "objA", Arg.Any<Vector2>(), Arg.Any<Color>());
                    _mockSpriteBatch.DrawString(Arg.Any<ISpriteFont>(), "objB", Arg.Any<Vector2>(), Arg.Any<Color>());
                }
            );
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateAfterCharacterTimeElapsedShouldUpdateQueuedItemShowLength()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            mockObject.TimePerCharacter.Returns(1);
            _sut.Enqueue(mockObject);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3)));

            mockObject.Received(1).ShowLength += 3;
        }

        [TestMethod]
        public void UpdateBeforeCharacterTimeElapsedShouldNotUpdateQueuedItemShowLength()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            mockObject.TimePerCharacter.Returns(1);
            _sut.Enqueue(mockObject);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5)));

            mockObject.Received(0).ShowLength += Arg.Any<int>();
        }

        [TestMethod]
        public void UpdateWithIncompleteItemShouldNotRender()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            mockObject.IsComplete.Returns(false);
            _sut.Enqueue(mockObject);

            _sut.Update(new GameTime());

            _mockSpriteBatch.Received(0).DrawString(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void UpdateWithCompleteItemShouldSetupRender()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            mockObject.IsComplete.Returns(true);
            _sut.Enqueue(mockObject);

            _sut.Update(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockDevice.SetRenderTarget(_mockStaticTarget);
                    _mockSpriteBatch.Begin();
                    _mockSpriteBatch.End();
                    _mockDevice.SetRenderTarget(null);
                }
            );
        }

        [TestMethod]
        public void UpdateWIthCompleteItemShouldCallSpriteBatchDrawString()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            ITextObject mockObject = Substitute.For<ITextObject>();
            mockObject.IsComplete.Returns(true);
            mockObject.TextDefinition.Font.Returns(mockFont);
            mockObject.CurrentText.Returns("ASDF");
            mockObject.Location.Returns(new Vector2(100, 200));
            mockObject.TextDefinition.Colour.Returns(Color.Pink);
            _sut.Enqueue(mockObject);

            _sut.Update(new GameTime());

            _mockSpriteBatch.Received(1).DrawString(mockFont, "ASDF", new Vector2(100, 200), Color.Pink);
        }
        #endregion

        #region Render Tests
        [TestMethod]
        public void RenderShouldSetRenderTargetToFinalTarget()
        {
            _sut.Render();

            _mockDevice.Received(1).SetRenderTarget(_mockFinalTarget);
        }

        [TestMethod]
        public void RenderShouldSetRenderTargetToNull()
        {
            _sut.Render();

            _mockDevice.Received(1).SetRenderTarget(null);
        }

        [TestMethod]
        public void RenderShouldReturnFinalTarget()
        {
            Assert.AreSame(_mockFinalTarget, _sut.Render());
        }

        [TestMethod]
        public void RenderShouldCallSpriteBatchBegin()
        {
            _sut.Render();

            _mockSpriteBatch.Received(1).Begin();
        }

        [TestMethod]
        public void RenderShouldCallSpriteBatchEnd()
        {
            _sut.Render();

            _mockSpriteBatch.Received(1).End();
        }

        [TestMethod]
        public void RenderShouldDrawItemAtStartOfQueue()
        {
            ITextObject mockText = Substitute.For<ITextObject>();
            ISpriteFont mockFont;
            mockText.TextDefinition.Font.Returns(mockFont = Substitute.For<ISpriteFont>());
            mockText.CurrentText.Returns("Test");
            mockText.Location.Returns(Vector2.Zero);
            mockText.TextDefinition.Colour.Returns(Color.Pink);
            _sut.Enqueue(mockText);

            _sut.Render();

            _mockSpriteBatch.Received(1).DrawString(mockFont, "Test", Vector2.Zero, Color.Pink);
        }
        #endregion

        #region Clear Tests
        [TestMethod]
        public void ClearShouldSetDataOnStaticTarget()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            _sut.Enqueue(mockObject);
            _mockStaticTarget.ClearReceivedCalls();

            _sut.Clear();

            // Should get an array of transparent pixels, but just make sure the first one is transparent.
            _mockStaticTarget.Received(1).SetData<Color>(Arg.Is<Color[]>(x => x[0] == Color.Transparent));
        }

        [TestMethod]
        public void ClearShouldSetDataOnFinalTarget()
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            _sut.Enqueue(mockObject);
            _mockFinalTarget.ClearReceivedCalls();

            _sut.Clear();

            // Should get an array of transparent pixels, but just make sure the first one is transparent.
            _mockFinalTarget.Received(1).SetData<Color>(Arg.Is<Color[]>(x => x[0] == Color.Transparent));
        }

        [TestMethod]
        public void ClearShouldSetIsCompleteToTrue()    // Indicates that the text queue is empty.
        {
            ITextObject mockObject = Substitute.For<ITextObject>();
            _sut.Enqueue(mockObject);
            _mockFinalTarget.ClearReceivedCalls();

            _sut.Clear();

            Assert.IsTrue(_sut.IsComplete);
        }
        #endregion
    }
}
