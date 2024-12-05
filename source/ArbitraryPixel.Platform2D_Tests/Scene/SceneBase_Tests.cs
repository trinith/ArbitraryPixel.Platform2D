using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace ArbitraryPixel.Platform2D_Tests
{
    [TestClass]
    public class SceneBase_Tests
    {
        private IEngine _mockEngine = null;
        private IAssetLoader _mockAssetLoader = null;
        private SceneBase _sut = null;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();

            _mockEngine.AssetBank.CreateLoader().Returns(_mockAssetLoader = Substitute.For<IAssetLoader>());

            _sut = new SceneBase(_mockEngine);
        }

        #region Constructor Tests Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullEngineShouldThrowException()
        {
            _sut = new SceneBase(null);
        }

        [TestMethod]
        public void ConstructShouldCreateAssetLoader()
        {
            _mockEngine.AssetBank.Received(1).CreateLoader();
        }

        [TestMethod]
        public void ConstructShouldRegisterLoadMethod()
        {
            _mockAssetLoader.Received(1).RegisterLoadMethod(Arg.Any<AssetLoadMethod>());
        }
        #endregion

        #region Property Get/Set Tests
        [TestMethod]
        public void NextSceneShouldDefaultNull()
        {
            Assert.AreEqual<IScene>(null, _sut.NextScene);
        }

        [TestMethod]
        public void NextScenePropertyShouldReturnSetValue()
        {
            IScene mockScene = Substitute.For<IScene>();
            _sut.NextScene = mockScene;
        }

        [TestMethod]
        public void SceneCompleteShouldDefaultFalse()
        {
            Assert.IsFalse(_sut.SceneComplete);
        }

        [TestMethod]
        public void SceneCompleteShouldReturnSetValue()
        {
            _sut.SceneComplete = true; ;

            Assert.IsTrue(_sut.SceneComplete);
        }
        #endregion

        #region Initialize Tests
        [TestMethod]
        public void InitializeShouldCallAssetLoaderLoadBank()
        {
            _sut.Initialize();

            _mockAssetLoader.Received(1).LoadBank();
        }
        #endregion

        #region ChangeScene Tests
        [TestMethod]
        public void ChangeSceneShouldSetNextScene()
        {
            IScene nextScene = Substitute.For<IScene>();
            _sut.ChangeScene(nextScene);

            Assert.AreSame(nextScene, _sut.NextScene);
        }

        [TestMethod]
        public void ChangeSceneShouldSetSceneCompleteToTrue()
        {
            _sut.ChangeScene(Substitute.For<IScene>());

            Assert.IsTrue(_sut.SceneComplete);
        }
        #endregion
    }
}
