using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests
{
    [TestClass]
    public class EntityBase_ProtectedPassThrough_Tests
    {
        #region Helper Class
        private class EntityBaseProtectedMethodTester : EntityBase
        {
            public class DataEventArgs : EventArgs
            {
                public object Data { get; private set; } = null;
                public DataEventArgs(object data)
                {
                    this.Data = data;
                }
            }

            public EventHandler<DataEventArgs> OnUpdateCalled;
            public EventHandler<DataEventArgs> OnPreDrawCalled;
            public EventHandler<DataEventArgs> OnDrawCalled;
            public EventHandler<DataEventArgs> OnDrawBeginCalled;
            public EventHandler<DataEventArgs> OnDrawEndCalled;

            public EntityBaseProtectedMethodTester(IEngine host)
                : base(host)
            {
            }

            protected override void OnUpdate(GameTime gameTime)
            {
                base.OnUpdate(gameTime);

                if (this.OnUpdateCalled != null)
                    this.OnUpdateCalled(this, new DataEventArgs(gameTime));
            }

            protected override void OnPreDraw(GameTime gameTime)
            {
                if (this.OnPreDrawCalled != null)
                    this.OnPreDrawCalled(this, new DataEventArgs(gameTime));
            }

            protected override void OnDraw(GameTime gameTime)
            {
                if (this.OnDrawCalled != null)
                    this.OnDrawCalled(this, new DataEventArgs(gameTime));
            }

            protected override void OnDrawBegin(GameTime gameTime)
            {
                if (this.OnDrawBeginCalled != null)
                    this.OnDrawBeginCalled(this, new DataEventArgs(gameTime));
            }

            protected override void OnDrawEnd(GameTime gameTime)
            {
                if (this.OnDrawEndCalled != null)
                    this.OnDrawEndCalled(this, new DataEventArgs(gameTime));
            }
        }
        #endregion

        #region Helper Methods
        private bool CheckGameTimeEquivalent(GameTime actual, GameTime expected)
        {
            return (actual.TotalGameTime == expected.TotalGameTime && actual.ElapsedGameTime == expected.ElapsedGameTime);
        }
        #endregion

        private IEngine _mockEngine = null;
        private EntityBaseProtectedMethodTester _sut = null;
        private EventHandler<EntityBaseProtectedMethodTester.DataEventArgs> _subscriber = null;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            _sut = new EntityBaseProtectedMethodTester(_mockEngine);
            _subscriber = Substitute.For<EventHandler<EntityBaseProtectedMethodTester.DataEventArgs>>();
        }

        #region Virtual Pass Through Tests
        [TestMethod]
        public void UpdateShouldCallOnUpdate()
        {
            _sut.OnUpdateCalled += _subscriber;

            _sut.Update(new GameTime(TimeSpan.FromSeconds(22), TimeSpan.FromSeconds(11)));

            _subscriber.Received(1)(_sut, Arg.Any<EntityBaseProtectedMethodTester.DataEventArgs>());
        }

        [TestMethod]
        public void UpdateShouldPassGameTimeToOnUpdate()
        {
            bool eventFired = false;
            GameTime gameTime = new GameTime(TimeSpan.FromSeconds(22), TimeSpan.FromSeconds(11));

            _sut.OnUpdateCalled += _subscriber;
            _subscriber.When(x => x(Arg.Any<object>(), Arg.Any<EntityBaseProtectedMethodTester.DataEventArgs>())).Do(
                x =>
                {
                    EntityBaseProtectedMethodTester.DataEventArgs arg = x[1] as EntityBaseProtectedMethodTester.DataEventArgs;
                    Assert.AreEqual(gameTime, arg.Data);
                    eventFired = true;
                }
            );

            _sut.Update(gameTime);

            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void DrawShouldCallOnDraw()
        {
            _sut.OnDrawCalled += _subscriber;

            _sut.Draw(new GameTime(TimeSpan.FromSeconds(22), TimeSpan.FromSeconds(11)));

            _subscriber.Received(1)(_sut, Arg.Any<EntityBaseProtectedMethodTester.DataEventArgs>());
        }

        [TestMethod]
        public void DrawShouldPassGameTimeToOnDraw()
        {
            bool eventFired = false;
            GameTime gameTime = new GameTime(TimeSpan.FromSeconds(22), TimeSpan.FromSeconds(11));

            _sut.OnDrawCalled += _subscriber;
            _subscriber.When(x => x(Arg.Any<object>(), Arg.Any<EntityBaseProtectedMethodTester.DataEventArgs>())).Do(
                x =>
                {
                    EntityBaseProtectedMethodTester.DataEventArgs arg = x[1] as EntityBaseProtectedMethodTester.DataEventArgs;
                    Assert.AreEqual(gameTime, arg.Data);
                    eventFired = true;
                }
            );

            _sut.Draw(gameTime);

            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void PreDrawShouldCallOnPreDraw()
        {
            _sut.OnPreDrawCalled += _subscriber;

            _sut.PreDraw(new GameTime(TimeSpan.FromSeconds(22), TimeSpan.FromSeconds(11)));

            GameTime expected = new GameTime(TimeSpan.FromSeconds(22), TimeSpan.FromSeconds(11));
            _subscriber.Received(1)(_sut, Arg.Is<EntityBaseProtectedMethodTester.DataEventArgs>(x => CheckGameTimeEquivalent((GameTime)x.Data, expected)));
        }

        [TestMethod]
        public void DrawBeginShouldCallOnDrawBegin()
        {
            _sut.OnDrawBeginCalled += _subscriber;

            _sut.Draw(new GameTime(TimeSpan.FromSeconds(22), TimeSpan.FromSeconds(11)));

            GameTime expected = new GameTime(TimeSpan.FromSeconds(22), TimeSpan.FromSeconds(11));
            _subscriber.Received(1)(_sut, Arg.Is<EntityBaseProtectedMethodTester.DataEventArgs>(x => CheckGameTimeEquivalent((GameTime)x.Data, expected)));
        }

        [TestMethod]
        public void DrawEndShouldCallOnDrawBegin()
        {
            _sut.OnDrawEndCalled += _subscriber;

            _sut.Draw(new GameTime(TimeSpan.FromSeconds(22), TimeSpan.FromSeconds(11)));

            GameTime expected = new GameTime(TimeSpan.FromSeconds(22), TimeSpan.FromSeconds(11));
            _subscriber.Received(1)(_sut, Arg.Is<EntityBaseProtectedMethodTester.DataEventArgs>(x => CheckGameTimeEquivalent((GameTime)x.Data, expected)));
        }

        [TestMethod]
        public void DrawShouldCallMethodsInExpectedOrder()
        {
            _sut.OnDrawCalled += Substitute.For<EventHandler<EntityBaseProtectedMethodTester.DataEventArgs>>();
            _sut.OnDrawBeginCalled += Substitute.For<EventHandler<EntityBaseProtectedMethodTester.DataEventArgs>>();
            _sut.OnDrawEndCalled += Substitute.For<EventHandler<EntityBaseProtectedMethodTester.DataEventArgs>>();

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _sut.OnDrawBeginCalled(Arg.Any<object>(), Arg.Any<EntityBaseProtectedMethodTester.DataEventArgs>());
                    _sut.OnDrawCalled(Arg.Any<object>(), Arg.Any<EntityBaseProtectedMethodTester.DataEventArgs>());
                    _sut.OnDrawEndCalled(Arg.Any<object>(), Arg.Any<EntityBaseProtectedMethodTester.DataEventArgs>());
                }
            );
        }
        #endregion
    }
}
