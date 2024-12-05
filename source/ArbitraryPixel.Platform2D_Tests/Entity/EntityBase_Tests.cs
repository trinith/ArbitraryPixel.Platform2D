using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.Platform2D_Tests
{
    [TestClass]
    public class EntityBase_Tests
    {
        private EntityBase _sut = null;
        private IEngine _mockEngine = null;

        private IUniqueIdGenerator _mockIdGen = null;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            _mockIdGen = Substitute.For<IUniqueIdGenerator>();
            _mockIdGen.GenerateUniqueId().Returns("1234abcd");
            _mockEngine.GetComponent<IUniqueIdGenerator>().Returns(_mockIdGen);

            _sut = new EntityBase(_mockEngine);
        }

        #region Constructor Tests Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullEngineShouldThrowException()
        {
            _sut = new EntityBase(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullUniqueIdGeneratorOnHostShouldThrowException()
        {
            _mockEngine.GetComponent<IUniqueIdGenerator>().Returns((IUniqueIdGenerator)null);
            _sut = new EntityBase(_mockEngine);
        }
        #endregion

        #region Property Get/Set Tests
        [TestMethod]
        public void HostShouldReturnConstructorParameter()
        {
            Assert.AreEqual<IEngine>(_mockEngine, _sut.Host);
        }

        [TestMethod]
        public void EnabledShouldDefaultTrue()
        {
            Assert.IsTrue(_sut.Enabled);
        }

        [TestMethod]
        public void EnabledShouldReturnSetValue()
        {
            _sut.Enabled = false;
            Assert.IsFalse(_sut.Enabled);
        }

        [TestMethod]
        public void VisibleShouldDefaultTrue()
        {
            Assert.IsTrue(_sut.Visible);
        }

        [TestMethod]
        public void VisibleShouldReturnSetValue()
        {
            _sut.Visible = false;
            Assert.IsFalse(_sut.Visible);
        }

        [TestMethod]
        public void UpdateOrderShouldDefaultToZero()
        {
            Assert.AreEqual<int>(0, _sut.UpdateOrder);
        }

        [TestMethod]
        public void UpdateOrderShouldReturnSetValue()
        {
            _sut.UpdateOrder = 25;
            Assert.AreEqual<int>(25, _sut.UpdateOrder);
        }

        [TestMethod]
        public void DrawOrderShouldDefaultToZero()
        {
            Assert.AreEqual<int>(0, _sut.DrawOrder);
        }

        [TestMethod]
        public void DrawOrderShouldReturnSetValue()
        {
            _sut.DrawOrder = 25;
            Assert.AreEqual<int>(25, _sut.DrawOrder);
        }

        [TestMethod]
        public void AliveShouldDefaultTrue()
        {
            Assert.IsTrue(_sut.Alive);
        }

        [TestMethod]
        public void AliveShouldReturnSetValue()
        {
            _sut.Alive = false;
            Assert.IsFalse(_sut.Alive);
        }

        [TestMethod]
        public void HostLayerShouldDefaultNull()
        {
            Assert.IsNull(_sut.OwningContainer);
        }

        [TestMethod]
        public void HostLayerShouldReturnSetValue()
        {
            IEntityContainer mockContainer = Substitute.For<IEntityContainer>();
            _sut.OwningContainer = mockContainer;

            Assert.AreEqual<IEntityContainer>(mockContainer, _sut.OwningContainer);
        }

        [TestMethod]
        public void IsDisposedShouldDefaultFalse()
        {
            Assert.IsFalse(_sut.IsDisposed);
        }

        [TestMethod]
        public void UniqueIdShouldReturnExpectedValue()
        {
            Assert.AreEqual<string>("1234abcd", _sut.UniqueId);
        }
        #endregion

        #region Property Changed Event Tests
        [TestMethod]
        public void EnabledWithNewValueShouldFireEnabledChanged()
        {
            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.EnabledChanged += sub;

            _sut.Enabled = false;

            sub.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void EnabledWithSameValueShouldNotFiredEnabledChanged()
        {
            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.EnabledChanged += sub;

            _sut.Enabled = true;

            sub.Received(0)(Arg.Any<object>(), Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void VisibleWithNewValueShoudlFireVisibleChanged()
        {
            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.VisibleChanged += sub;

            _sut.Visible = false;

            sub.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void VisibleWithSameValueShouldNotFireVisibleChanged()
        {
            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.VisibleChanged += sub;

            _sut.Visible = true;

            sub.Received(0)(Arg.Any<object>(), Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void UpdateOrderWithNewValueShouldFireUpdateOrderChanged()
        {
            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.UpdateOrderChanged += sub;

            _sut.UpdateOrder = 25;

            sub.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void UpdateOrderWithSameValueShouldNotFireUpdateOrderChanged()
        {
            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.UpdateOrderChanged += sub;

            _sut.UpdateOrder = 0;

            sub.Received(0)(Arg.Any<object>(), Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void DrawOrderWithNewValueShouldFireDrawOrderChanged()
        {
            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.DrawOrderChanged += sub;

            _sut.DrawOrder = 25;

            sub.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void DrawOrderWithSameValueShouldNotFireDrawOrderChanged()
        {
            var sub = Substitute.For<EventHandler<EventArgs>>();
            _sut.DrawOrderChanged += sub;

            _sut.DrawOrder = 0;

            sub.Received(0)(Arg.Any<object>(), Arg.Any<EventArgs>());
        }
        #endregion

        #region Dispose Tests
        [TestMethod]
        public void FirstDisposeShouldCallHostContainerRemoveEntity()
        {
            IEntityContainer mockContainer = Substitute.For<IEntityContainer>();
            _sut.OwningContainer = mockContainer;

            _sut.Dispose();

            mockContainer.Received(1).RemoveEntity(_sut);
        }

        [TestMethod]
        public void SecondDisposeShouldNotCallHostContainerRemoveEntity()
        {
            IEntityContainer mockContainer = Substitute.For<IEntityContainer>();
            _sut.OwningContainer = mockContainer;
            _sut.Dispose();
            mockContainer.ClearReceivedCalls();

            _sut.Dispose();

            mockContainer.Received(0).RemoveEntity(_sut);
        }

        [TestMethod]
        public void FirstDisposeShouldFireDisposedEvent()
        {
            EventHandler<EventArgs> mockSubscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.Disposed += mockSubscriber;

            _sut.Dispose();

            mockSubscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void SecondDisposeShouldNotFireDisposedEvent()
        {
            EventHandler<EventArgs> mockSubscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.Disposed += mockSubscriber;
            _sut.Dispose();
            mockSubscriber.ClearReceivedCalls();

            _sut.Dispose();

            mockSubscriber.Received(0)(_sut, Arg.Any<EventArgs>());
        }

        [TestMethod]
        public void DisposeShouldSetIsDisposedToTrue()
        {
            _sut.Dispose();

            Assert.IsTrue(_sut.IsDisposed);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawWithTargetShouldSetRenderTarget()
        {
            IRenderTarget2D mockTarget = Substitute.For<IRenderTarget2D>();
            _sut.Draw(new GameTime(), mockTarget);

            _mockEngine.Graphics.GraphicsDevice.Received(1).SetRenderTarget(mockTarget);
        }

        [TestMethod]
        public void DrawWithTargetShouldSetRenderTargetNull()
        {
            _sut.Draw(new GameTime(), Substitute.For<IRenderTarget2D>());

            _mockEngine.Graphics.GraphicsDevice.Received(1).SetRenderTarget(null);
        }

        [TestMethod]
        public void DrawWithRenderTargetAndClearColourNullShouldNotCallGraphicsDeviceClear()
        {
            _sut.Draw(new GameTime(), Substitute.For<IRenderTarget2D>());

            _mockEngine.Graphics.GraphicsDevice.Received(0).Clear(Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawWithRenderTargetAndClearColourShouldCallGraphicsDeviceClear()
        {
            _sut.Draw(new GameTime(), Substitute.For<IRenderTarget2D>(), Color.Pink);

            _mockEngine.Graphics.GraphicsDevice.Received(1).Clear(Color.Pink);
        }
        #endregion
    }
}
