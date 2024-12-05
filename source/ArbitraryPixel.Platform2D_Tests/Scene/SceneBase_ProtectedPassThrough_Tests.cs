using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;
using NSubstitute;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Common.ContentManagement;

namespace ArbitraryPixel.Platform2D_Tests
{
    [TestClass]
    public class SceneBase_ProtectedPassThrough_Tests
    {
        #region Helper Class
        private class SceneBaseProtectedMethodTester : SceneBase
        {
            public class DataEventArgs : EventArgs
            {
                public object[] Data { get; private set; } = null;
                public DataEventArgs(object[] data)
                {
                    this.Data = data;
                }
            }

            public EventHandler<DataEventArgs> OnInitializedCalled;
            public EventHandler<DataEventArgs> OnResetCalled;
            public EventHandler<DataEventArgs> OnLoadAssetBankCalled;
            public EventHandler<DataEventArgs> OnStartingCalled;
            public EventHandler<DataEventArgs> OnEndingCalled;

            public SceneBaseProtectedMethodTester(IEngine host)
                : base(host)
            {
            }

            protected override void OnInitialize()
            {
                base.OnInitialize();

                if (this.OnInitializedCalled != null)
                    this.OnInitializedCalled(this, new DataEventArgs(null));
            }

            protected override void OnReset()
            {
                base.OnReset();

                if (this.OnResetCalled != null)
                    this.OnResetCalled(this, new DataEventArgs(null));

            }

            protected override void OnStarting()
            {
                base.OnStarting();

                if (this.OnStartingCalled != null)
                    this.OnStartingCalled(this, new DataEventArgs(null));
            }

            protected override void OnEnding()
            {
                base.OnEnding();

                if (this.OnEndingCalled != null)
                    this.OnEndingCalled(this, new DataEventArgs(null));
            }

            protected override void OnLoadAssetBank(IContentManager content, IAssetBank bank)
            {
                base.OnLoadAssetBank(content, bank);

                if (this.OnLoadAssetBankCalled != null)
                    this.OnLoadAssetBankCalled(this, new DataEventArgs(new object[] { content, bank }));
            }
        }
        #endregion

        private IEngine _mockEngine = null;
        private SceneBaseProtectedMethodTester _sut = null;
        private EventHandler<SceneBaseProtectedMethodTester.DataEventArgs> _subscriber = null;

        private AssetLoadMethod _assetLoadMethod;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            IAssetLoader mockAssetLoader = Substitute.For<IAssetLoader>();
            _mockEngine.AssetBank.CreateLoader().Returns(mockAssetLoader);
            mockAssetLoader.When(x => x.RegisterLoadMethod(Arg.Any<AssetLoadMethod>())).Do(x => _assetLoadMethod = x[0] as AssetLoadMethod);
            mockAssetLoader.When(x => x.LoadBank()).Do(x => _assetLoadMethod(_mockEngine.AssetBank));

            _sut = new SceneBaseProtectedMethodTester(_mockEngine);
            _subscriber = Substitute.For<EventHandler<SceneBaseProtectedMethodTester.DataEventArgs>>();
        }

        #region Virtual Pass Through Tests
        [TestMethod]
        public void InitializeShouldCallOnInitialize()
        {
            _sut.OnInitializedCalled += _subscriber;

            _sut.Initialize();

            _subscriber.Received(1)(_sut, Arg.Any<SceneBaseProtectedMethodTester.DataEventArgs>());
        }

        [TestMethod]
        public void InitializeShouldCallOnLoadAssetBank()
        {
            _sut.OnLoadAssetBankCalled += _subscriber;

            _sut.Initialize();

            _subscriber.Received(1)(
                _sut,
                Arg.Is<SceneBaseProtectedMethodTester.DataEventArgs>(
                    x => (x.Data[0] == _mockEngine.Content && x.Data[1] == _mockEngine.AssetBank)
                )
            );
        }

        [TestMethod]
        public void ResetShouldCallOnReset()
        {
            _sut.OnResetCalled += _subscriber;

            _sut.Reset();

            _subscriber.Received(1)(_sut, Arg.Any<SceneBaseProtectedMethodTester.DataEventArgs>());
        }

        [TestMethod]
        public void ResetWhenAlreadyResetShouldNotCallOnReset()
        {
            _sut.OnResetCalled += _subscriber;
            _sut.Reset();
            _subscriber.ClearReceivedCalls();

            _sut.Reset();

            _subscriber.Received(0)(Arg.Any<object>(), Arg.Any<SceneBaseProtectedMethodTester.DataEventArgs>());
        }

        [TestMethod]
        public void ResetSubsequentCallAfterUpdateShouldCallOnReset()
        {
            _sut.OnResetCalled += _subscriber;
            _sut.Reset();
            _subscriber.ClearReceivedCalls();
            _sut.Update(new GameTime());

            _sut.Reset();

            _subscriber.Received(1)(_sut, Arg.Any<SceneBaseProtectedMethodTester.DataEventArgs>());
        }

        [TestMethod]
        public void EndShouldCallOnEnding()
        {
            _sut.OnEndingCalled += _subscriber;

            _sut.End();

            _subscriber.Received(1)(_sut, Arg.Any<SceneBaseProtectedMethodTester.DataEventArgs>());
        }

        [TestMethod]
        public void StartShouldCallOnStarting()
        {
            _sut.OnStartingCalled += _subscriber;

            _sut.Start();

            _subscriber.Received(1)(_sut, Arg.Any<SceneBaseProtectedMethodTester.DataEventArgs>());
        }
        #endregion
    }
}
