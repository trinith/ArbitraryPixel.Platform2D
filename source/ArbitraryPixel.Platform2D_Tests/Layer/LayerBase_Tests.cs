using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Common.Graphics;
using NSubstitute;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArbitraryPixel.Platform2D.Entity;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.Platform2D_Tests.Layer
{
    [TestClass]
    public class LayerBase_Tests
    {
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;

        private LayerBase _sut = null;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockEngine.ScreenManager.ScaleMatrix.Returns(Matrix.CreateScale(4, 2, 1));

            _sut = new LayerBase(_mockEngine, _mockSpriteBatch);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullSpriteBatchShouldThrowException()
        {
            _sut = new LayerBase(_mockEngine, null);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void MainSpriteBatchShouldHaveValueFromConstructor()
        {
            Assert.AreEqual<ISpriteBatch>(_mockSpriteBatch, _sut.MainSpriteBatch);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_SpriteSortMode()
        {
            Assert.AreEqual<SpriteSortMode>(SpriteSortMode.Deferred, _sut.SpriteSortMode);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_BlendState()
        {
            Assert.AreEqual<BlendState>(null, _sut.BlendState);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_SamplerState()
        {
            Assert.AreEqual<SamplerState>(null, _sut.SamplerState);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_DepthStencilState()
        {
            Assert.AreEqual<DepthStencilState>(null, _sut.DepthStencilState);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_RasterizerState()
        {
            Assert.AreEqual<RasterizerState>(null, _sut.RasterizerState);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_Effect()
        {
            Assert.AreEqual<IEffect>(null, _sut.Effect);
        }

        [TestMethod]
        public void PropertyShouldHaveExpectedDefault_Matrix()
        {
            Assert.AreEqual<Matrix?>(null, _sut.Matrix);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldCallSpriteBatchBegin()
        {
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateScale(4, 2, 1));
        }

        [TestMethod]
        public void DrawShouldCallSpriteBatchEnd()
        {
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).End();
        }
        #endregion

        #region Draw - SpriteBatch Parameters Tests
        [TestMethod]
        public void DrawShouldPassBeginParameterToSpriteBatch_SpriteSortMode()
        {
            _sut.SpriteSortMode = SpriteSortMode.BackToFront;
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Arg.Any<Matrix?>());
        }

        [TestMethod]
        public void DrawShouldPassBeginParameterToSpriteBatch_BlendState()
        {
            _sut.BlendState = BlendState.NonPremultiplied;
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Arg.Any<Matrix?>());
        }

        [TestMethod]
        public void DrawShouldPassBeginParameterToSpriteBatch_SamplerState()
        {
            _sut.SamplerState = SamplerState.AnisotropicWrap;
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicWrap, null, null, null, Arg.Any<Matrix?>());
        }

        [TestMethod]
        public void DrawShouldPassBeginParameterToSpriteBatch_DepthStencilState()
        {
            _sut.DepthStencilState = DepthStencilState.DepthRead;
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Begin(SpriteSortMode.Deferred, null, null, DepthStencilState.DepthRead, null, null, Arg.Any<Matrix?>());
        }

        [TestMethod]
        public void DrawShouldPassBeginParameterToSpriteBatch_RasterizerState()
        {
            _sut.RasterizerState = RasterizerState.CullClockwise;
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Begin(SpriteSortMode.Deferred, null, null, null, RasterizerState.CullClockwise, null, Arg.Any<Matrix?>());
        }

        [TestMethod]
        public void DrawShouldPassBeginParameterToSpriteBatch_Effect()
        {
            IEffect mockEffect = Substitute.For<IEffect>();
            _sut.Effect = mockEffect;
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Begin(SpriteSortMode.Deferred, null, null, null, null, mockEffect, Arg.Any<Matrix?>());
        }

        [TestMethod]
        public void DrawShouldPassBeginParameterToSpriteBatch_Matrix()
        {
            _sut.Matrix = Matrix.CreateTranslation(-5, 3, 8);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Begin(Arg.Any<SpriteSortMode>(), Arg.Any<BlendState>(), Arg.Any<SamplerState>(), Arg.Any<DepthStencilState>(), Arg.Any<RasterizerState>(), Arg.Any<IEffect>(), Matrix.CreateTranslation(-5, 3, 8));
        }
        #endregion

        #region Draw - Parent Layer Tests
        [TestMethod]
        public void DrawWithParentSpriteBatchShouldNotCallBegin()
        {
            ILayer parentLayer = Substitute.For<ILayer>();
            parentLayer.MainSpriteBatch.Returns(_mockSpriteBatch);
            _sut.OwningContainer = parentLayer;

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Begin(Arg.Any<SpriteSortMode>(), Arg.Any<BlendState>(), Arg.Any<SamplerState>(), Arg.Any<DepthStencilState>(), Arg.Any<RasterizerState>(), Arg.Any<IEffect>(), Arg.Any<Matrix>());
        }

        [TestMethod]
        public void DrawWithParentSeBatchShouldNotCallEnd()
        {
            ILayer parentLayer = Substitute.For<ILayer>();
            parentLayer.MainSpriteBatch.Returns(_mockSpriteBatch);
            _sut.OwningContainer = parentLayer;

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).End();
        }

        [TestMethod]
        public void DrawWithDifferentParentBatchShouldCallBegin()
        {
            ILayer parentLayer = Substitute.For<ILayer>();
            parentLayer.MainSpriteBatch.Returns(Substitute.For<ISpriteBatch>());
            _sut.OwningContainer = parentLayer;

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Begin(Arg.Any<SpriteSortMode>(), Arg.Any<BlendState>(), Arg.Any<SamplerState>(), Arg.Any<DepthStencilState>(), Arg.Any<RasterizerState>(), Arg.Any<IEffect>(), Arg.Any<Matrix>());
        }

        [TestMethod]
        public void DrawWithDifferentParentBatchShouldCallEnd()
        {
            ILayer parentLayer = Substitute.For<ILayer>();
            parentLayer.MainSpriteBatch.Returns(Substitute.For<ISpriteBatch>());
            _sut.OwningContainer = parentLayer;

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).End();
        }
        #endregion
    }
}
