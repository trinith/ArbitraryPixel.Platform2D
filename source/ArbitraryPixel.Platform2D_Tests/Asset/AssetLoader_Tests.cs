using ArbitraryPixel.Platform2D.Assets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace ArbitraryPixel.Platform2D_Tests.Asset
{
    [TestClass]
    public class AssetLoader_Tests
    {
        private IAssetBank _mockBank;
        private AssetLoadMethod _mockLoadMethod;
        private AssetLoader _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockBank = Substitute.For<IAssetBank>();
            _mockLoadMethod = Substitute.For<AssetLoadMethod>();
            _sut = new AssetLoader(_mockBank);
            _sut.RegisterLoadMethod(_mockLoadMethod);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullAssetBankShouldThrowException()
        {
            _sut = new AssetLoader(null);
        }
        #endregion

        #region LoadBank Tests
        [TestMethod]
        public void LoadBankWithNewBankShouldCallLoadMethod()
        {
            _sut.LoadBank();

            _mockLoadMethod.Received(1)(_mockBank);
        }

        [TestMethod]
        public void LoadBankWithLoadedBankShouldNotCallLoadMethod()
        {
            IAssetBank mockBank = Substitute.For<IAssetBank>();
            _sut.LoadBank();
            _mockLoadMethod.ClearReceivedCalls();

            _sut.LoadBank();

            _mockLoadMethod.Received(0)(mockBank);
        }

        [TestMethod]
        public void LoadBankShouldRunAllLoadMethods()
        {
            AssetLoadMethod mockLoaderOther = Substitute.For<AssetLoadMethod>();
            _sut.RegisterLoadMethod(mockLoaderOther);

            _sut.LoadBank();

            Received.InOrder(
                () =>
                {
                    _mockLoadMethod(Arg.Any<IAssetBank>());
                    mockLoaderOther(Arg.Any<IAssetBank>());
                }
            );
        }
        #endregion

        #region Bank Assets Cleared Response Tests
        [TestMethod]
        public void LoadBankWithLoadedBankThatIsClearedShouldCallLoadMethod()
        {
            _sut.LoadBank();
            _mockLoadMethod.ClearReceivedCalls();
            _mockBank.AssetsCleared += Raise.Event<EventHandler<EventArgs>>();

            _sut.LoadBank();

            _mockLoadMethod.Received(1)(_mockBank);
        }
        #endregion
    }
}
