using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace ArbitraryPixel.Platform2D_Tests
{
    [TestClass]
    public class AssetBank_Tests
    {
        private AssetBank _sut = null;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new AssetBank();
        }

        #region Get
        [TestMethod]
        public void GetOnAssetShouldReturnStoredAsset()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _sut.Put<ITexture2D>("texture", mockTexture);

            Assert.AreEqual<ITexture2D>(mockTexture, _sut.Get<ITexture2D>("texture"));
        }

        [TestMethod]
        public void GetOnAssetWithSameNameAsDifferentTypeShouldReturnCorrectAssetForType_TypeA()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            ISpriteFont mockSpriteFont = Substitute.For<ISpriteFont>();

            _sut.Put<ITexture2D>("asset", mockTexture);
            _sut.Put<ISpriteFont>("asset", mockSpriteFont);

            Assert.AreEqual<ITexture2D>(mockTexture, _sut.Get<ITexture2D>("asset"));
        }

        [TestMethod]
        public void GetOnAssetWithSameNameAsDifferentTypeShouldReturnCorrectAssetForType_TypeB()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            ISpriteFont mockSpriteFont = Substitute.For<ISpriteFont>();

            _sut.Put<ITexture2D>("asset", mockTexture);
            _sut.Put<ISpriteFont>("asset", mockSpriteFont);

            Assert.AreEqual<ISpriteFont>(mockSpriteFont, _sut.Get<ISpriteFont>("asset"));
        }
        #endregion

        #region Put (Generic) Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PutGenericWithExistingAssetAndTypeShouldThrowException()
        {
            _sut.Put<ITexture2D>("asset", Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("asset", Substitute.For<ITexture2D>());
        }

        [TestMethod]
        public void PutWithOverwriteOnExistingAssetShouldReplaceAsset()
        {
            ITexture2D mockAsset = Substitute.For<ITexture2D>();
            _sut.Put<ITexture2D>("asset", Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("asset", mockAsset, true);

            Assert.AreEqual<ITexture2D>(mockAsset, _sut.Get<ITexture2D>("asset"));
        }

        [TestMethod]
        public void PutGenericWithExistingTypeShouldStoreAsset_AssetA()
        {
            ITexture2D assetA = Substitute.For<ITexture2D>();
            ITexture2D assetB = Substitute.For<ITexture2D>();

            _sut.Put<ITexture2D>("assetA", assetA);
            _sut.Put<ITexture2D>("assetB", assetB);

            Assert.AreEqual<ITexture2D>(assetA, _sut.Get<ITexture2D>("assetA"));
        }

        [TestMethod]
        public void PutGenericWithExistingTypeShouldStoreAsset_AssetB()
        {
            ITexture2D assetA = Substitute.For<ITexture2D>();
            ITexture2D assetB = Substitute.For<ITexture2D>();

            _sut.Put<ITexture2D>("assetA", assetA);
            _sut.Put<ITexture2D>("assetB", assetB);

            Assert.AreEqual<ITexture2D>(assetA, _sut.Get<ITexture2D>("assetA"));
        }
        #endregion

        #region Put (Non Generic) Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PutWithExistingAssetAndTypeShouldThrowException()
        {
            _sut.Put<ITexture2D>("asset", Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("asset", Substitute.For<ITexture2D>());
        }

        [TestMethod]
        public void PutWithExistingTypeShouldStoreAsset_AssetA()
        {
            ITexture2D assetA = Substitute.For<ITexture2D>();
            ITexture2D assetB = Substitute.For<ITexture2D>();

            _sut.Put(typeof(ITexture2D), "assetA", assetA);
            _sut.Put(typeof(ITexture2D), "assetB", assetB);

            Assert.AreEqual<ITexture2D>(assetA, _sut.Get<ITexture2D>("assetA"));
        }

        [TestMethod]
        public void PutWithExistingTypeShouldStoreAsset_AssetB()
        {
            ITexture2D assetA = Substitute.For<ITexture2D>();
            ITexture2D assetB = Substitute.For<ITexture2D>();

            _sut.Put(typeof(ITexture2D), "assetA", assetA);
            _sut.Put(typeof(ITexture2D), "assetB", assetB);

            Assert.AreEqual<ITexture2D>(assetA, _sut.Get<ITexture2D>("assetA"));
        }
        #endregion

        #region GetAllAssets Tests
        [TestMethod]
        public void GetAllAssetsWithEmptyTypeShouldReturnNull()
        {
            Assert.IsNull(_sut.GetAllAssets<ITexture2D>());
        }

        [TestMethod]
        public void GetAllAssetsWithExistingTypeShouldReturnExpectedAssets_TypeA()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            ISpriteFont mockSpriteFont = Substitute.For<ISpriteFont>();

            _sut.Put<ITexture2D>("texture", mockTexture);
            _sut.Put<ISpriteFont>("font", mockSpriteFont);

            Assert.AreEqual<ITexture2D>(mockTexture, _sut.GetAllAssets<ITexture2D>()[0]);
        }

        [TestMethod]
        public void GetAllAssetsWithExistingTypeShouldReturnExpectedAssets_TypeB()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            ISpriteFont mockSpriteFont = Substitute.For<ISpriteFont>();

            _sut.Put<ITexture2D>("texture", mockTexture);
            _sut.Put<ISpriteFont>("font", mockSpriteFont);

            Assert.AreEqual<ISpriteFont>(mockSpriteFont, _sut.GetAllAssets<ISpriteFont>()[0]);
        }
        #endregion

        #region GetAllMatchingAssets Tests
        [TestMethod]
        public void GetAllMatchingAssetsWithPartialNameShouldReturnExpectedAsset_Index0()
        {
            ITexture2D textureA, textureB, otherTexture;
            _sut.Put<ITexture2D>("textureA", textureA = Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("textureB", textureB = Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("texture", Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("otherTexture", otherTexture = Substitute.For<ITexture2D>());

            Assert.AreEqual<ITexture2D>(textureA, _sut.GetAllMatchingAssets<ITexture2D>(@"texture\w+")[0]);
        }

        [TestMethod]
        public void GetAllMatchingAssetsWithPartialNameShouldReturnExpectedAsset_Index1()
        {
            ITexture2D textureA, textureB, otherTexture;
            _sut.Put<ITexture2D>("textureA", textureA = Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("textureB", textureB = Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("texture", Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("otherTexture", otherTexture = Substitute.For<ITexture2D>());

            Assert.AreEqual<ITexture2D>(textureB, _sut.GetAllMatchingAssets<ITexture2D>(@"texture\w+")[1]);
        }

        [TestMethod]
        public void GetAllMatchingAssetsWithPartialNameShouldReturnExpectedAsset_ExpectedLength()
        {
            ITexture2D textureA, textureB, otherTexture;
            _sut.Put<ITexture2D>("textureA", textureA = Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("textureB", textureB = Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("texture", Substitute.For<ITexture2D>());
            _sut.Put<ITexture2D>("otherTexture", otherTexture = Substitute.For<ITexture2D>());

            Assert.AreEqual<int>(2, _sut.GetAllMatchingAssets<ITexture2D>(@"texture\w+").Length);
        }
        #endregion

        #region Exists Tests
        [TestMethod]
        public void ExistsWithExistingTypeAndNameShouldReturnTrue()
        {
            _sut.Put<ITexture2D>("asdf", Substitute.For<ITexture2D>());

            Assert.IsTrue(_sut.Exists<ITexture2D>("asdf"));
        }

        [TestMethod]
        public void ExistsWithNameAndTypeNotStoredShouldReturnFalse()
        {
            Assert.IsFalse(_sut.Exists<ITexture2D>("asdf"));
        }

        [TestMethod]
        public void ExistsWithAssetNameForDifferentTypeShouldReturnFalse()
        {
            _sut.Put<ISpriteFont>("asdf", Substitute.For<ISpriteFont>());

            Assert.IsFalse(_sut.Exists<ITexture2D>("asdf"));
        }

        [TestMethod]
        public void ExistsWithDifferentTypeForAssetNameShouldReturnFalse()
        {
            _sut.Put<ITexture2D>("asdf", Substitute.For<ITexture2D>());

            Assert.IsFalse(_sut.Exists<ISpriteFont>("asdf"));
        }
        #endregion

        #region Clear Tests
        [TestMethod]
        public void ClearShouldRemoveAssets()
        {
            ITexture2D asset = Substitute.For<ITexture2D>();
            _sut.Put<ITexture2D>("asset", asset);

            _sut.Clear();

            Assert.IsFalse(_sut.Exists<ITexture2D>("asset"));
        }

        [TestMethod]
        public void ClearShouldFireAssetsClearedEvent()
        {
            EventHandler<EventArgs> mockSubscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.AssetsCleared += mockSubscriber;

            _sut.Clear();

            mockSubscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion

        #region CreateLoader Tests
        [TestMethod]
        public void CreateLoaderShouldReturnNewAssetLoaderObject()
        {
            Assert.IsNotNull(_sut.CreateLoader());
        }

        [TestMethod]
        public void CreateLoaderShouldLoadAssetsForSelf()
        {
            AssetLoadMethod mockLoadMethod = Substitute.For<AssetLoadMethod>();
            IAssetLoader loader = _sut.CreateLoader();
            loader.RegisterLoadMethod(mockLoadMethod);

            loader.LoadBank();

            mockLoadMethod.Received(1)(_sut);
        }
        #endregion
    }
}
