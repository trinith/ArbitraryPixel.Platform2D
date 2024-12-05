using System;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Common.Screen;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Audio;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Engine
{
    [TestClass]
    public class EngineBase_Tests
    {
        #region EngineBaseTestable Class
        // Add functionality to EngineBase so that we can facilitate testing.
        public class EngineBaseTestable : EngineBase
        {
            public EngineBaseTestable(IComponentContainer componentContainer)
                : base(componentContainer)
            {
            }

            // CurrentScene is protected, allow a way to set the current scene so we can test stuff.
            public void SetCurrentScene(IScene scene)
            {
                this.CurrentScene = scene;
            }
        }
        #endregion

        private EngineBaseTestable _sut;
        private IComponentContainer _mockComponentContainer;

        private IGrfxDeviceManager _mockGrfxDeviceManager;
        private IContentManager _mockContentManager;
        private IScreenManager _mockScreenManager;
        private ISurfaceInputManager _mockSurfaceInputManager;
        private IGrfxFactory _mockGrfxFactory;
        private IAssetBank _mockAssetBank;
        private IAudioManager _mockAudioManager;

        [TestInitialize]
        public void Initialize()
        {
            _mockComponentContainer = Substitute.For<IComponentContainer>();

            _mockGrfxDeviceManager = Substitute.For<IGrfxDeviceManager>();
            _mockComponentContainer.GetComponent<IGrfxDeviceManager>().Returns(_mockGrfxDeviceManager);

            _mockContentManager = Substitute.For<IContentManager>();
            _mockComponentContainer.GetComponent<IContentManager>().Returns(_mockContentManager);

            _mockScreenManager = Substitute.For<IScreenManager>();
            _mockComponentContainer.GetComponent<IScreenManager>().Returns(_mockScreenManager);

            _mockSurfaceInputManager = Substitute.For<ISurfaceInputManager>();
            _mockComponentContainer.GetComponent<ISurfaceInputManager>().Returns(_mockSurfaceInputManager);

            _mockGrfxFactory = Substitute.For<IGrfxFactory>();
            _mockComponentContainer.GetComponent<IGrfxFactory>().Returns(_mockGrfxFactory);

            _mockAssetBank = Substitute.For<IAssetBank>();
            _mockComponentContainer.GetComponent<IAssetBank>().Returns(_mockAssetBank);

            _mockAudioManager = Substitute.For<IAudioManager>();
            _mockComponentContainer.GetComponent<IAudioManager>().Returns(_mockAudioManager);
        }

        private void Construct()
        {
            _sut = new EngineBaseTestable(_mockComponentContainer);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowExpectedException_ComponentContainer()
        {
            _sut = new EngineBaseTestable(null);
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException<IGrfxDeviceManager>))]
        public void ConstructWithMissingComponentShouldThrowExpectedException_IGrfxDeviceManager()
        {
            _mockComponentContainer.GetComponent<IGrfxDeviceManager>().Returns((IGrfxDeviceManager)null);
            Construct();
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException<IContentManager>))]
        public void ConstructWithMissingComponentShouldThrowExpectedException_IContentManager()
        {
            _mockComponentContainer.GetComponent<IContentManager>().Returns((IContentManager)null);
            Construct();
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException<IScreenManager>))]
        public void ConstructWithMissingComponentShouldThrowExpectedException_IScreenManager()
        {
            _mockComponentContainer.GetComponent<IScreenManager>().Returns((IScreenManager)null);
            Construct();
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException<ISurfaceInputManager>))]
        public void ConstructWithMissingComponentShouldThrowExpectedException_ISurfaceInputManager()
        {
            _mockComponentContainer.GetComponent<ISurfaceInputManager>().Returns((ISurfaceInputManager)null);
            Construct();
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException<IGrfxFactory>))]
        public void ConstructWithMissingComponentShouldThrowExpectedException_IGrfxFactory()
        {
            _mockComponentContainer.GetComponent<IGrfxFactory>().Returns((IGrfxFactory)null);
            Construct();
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException<IAssetBank>))]
        public void ConstructWithMissingComponentShouldThrowExpectedException_IAssetBank()
        {
            _mockComponentContainer.GetComponent<IAssetBank>().Returns((IAssetBank)null);
            Construct();
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException<IAudioManager>))]
        public void ConstructWithMissingComponentShouldThrowExpectedException_IAudioManager()
        {
            _mockComponentContainer.GetComponent<IAudioManager>().Returns((IAudioManager)null);
            Construct();
        }
        #endregion

        #region Component Container Tests
        [TestMethod]
        public void RegisterComponentShouldCallContainerRegisterComponent()
        {
            Construct();

            _sut.RegisterComponent<string>("test");

            _mockComponentContainer.Received(1).RegisterComponent<string>("test");
        }

        [TestMethod]
        public void GetComponentShouldCallContainerGetComponent()
        {
            Construct();

            _sut.GetComponent<string>();

            _mockComponentContainer.Received(1).GetComponent<string>();
        }

        [TestMethod]
        public void GetComponentWithGenericShouldReturnExpectedValue()
        {
            Construct();

            _mockComponentContainer.GetComponent<string>().Returns("test");

            Assert.AreEqual<string>("test", _sut.GetComponent<string>());
        }

        [TestMethod]
        public void GetComponentWithTypeShouldReturnExpectedValue()
        {
            Construct();

            _mockComponentContainer.GetComponent(typeof(string)).Returns("test");

            Assert.AreEqual<string>("test", (string)_sut.GetComponent(typeof(string)));
        }

        [TestMethod]
        public void ContainsComponentWithTypeShouldCallContainerContainsComponentWithType()
        {
            Construct();

            _sut.ContainsComponent(typeof(string));

            _mockComponentContainer.Received(1).ContainsComponent(typeof(string));
        }

        [TestMethod]
        public void ContainsComponentWithTypeShouldReturnExpectedValue()
        {
            Construct();

            _mockComponentContainer.ContainsComponent(typeof(string)).Returns(true);

            Assert.IsTrue(_sut.ContainsComponent(typeof(string)));
        }

        [TestMethod]
        public void ContainsComponentWithGenericShouldCallContainerContainsComponentWithGeneric()
        {
            Construct();

            _sut.ContainsComponent<string>();

            _mockComponentContainer.Received(1).ContainsComponent<string>();
        }

        [TestMethod]
        public void ContainsComponentWithGenericShouldReturnExpectedValue()
        {
            Construct();

            _mockComponentContainer.ContainsComponent<string>().Returns(true);

            Assert.IsTrue(_sut.ContainsComponent<string>());
        }
        #endregion

        #region Component Property Tests
        [TestMethod]
        public void ComponentPropertyShouldReturnExpectedValue_ScreenManager()
        {
            Construct();

            Assert.AreSame(_mockScreenManager, _sut.ScreenManager);
        }

        [TestMethod]
        public void ComponentPropertyShouldReturnExpectedValue_InputManager()
        {
            Construct();

            Assert.AreSame(_mockSurfaceInputManager, _sut.InputManager);
        }

        [TestMethod]
        public void ComponentPropertyShouldReturnExpectedValue_Graphics()
        {
            Construct();

            Assert.AreSame(_mockGrfxDeviceManager, _sut.Graphics);
        }

        [TestMethod]
        public void ComponentPropertyShouldReturnExpectedValue_Content()
        {
            Construct();

            Assert.AreSame(_mockContentManager, _sut.Content);
        }

        [TestMethod]
        public void ComponentPropertyShouldReturnExpectedValue_AssetBank()
        {
            Construct();

            Assert.AreSame(_mockAssetBank, _sut.AssetBank);
        }

        [TestMethod]
        public void ComponentPropertyShouldReturnExpectedValue_GrfxFactory()
        {
            Construct();

            Assert.AreSame(_mockGrfxFactory, _sut.GrfxFactory);
        }

        [TestMethod]
        public void ComponentPropertyShouldReturnExpectedValue_AudioManager()
        {
            Construct();

            Assert.AreSame(_mockAudioManager, _sut.AudioManager);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void FinishedShouldDefaultToFalse()
        {
            Construct();

            Assert.IsFalse(_sut.Finished);
        }

        [TestMethod]
        public void FinishedShouldReturnSetValue()
        {
            Construct();

            _sut.Finished = true;

            Assert.IsTrue(_sut.Finished);
        }

        [TestMethod]
        public void CurrentSceneShouldDefaultToNull()
        {
            Construct();

            Assert.IsNull(_sut.CurrentScene);
        }

        [TestMethod]
        public void CurrentSceneShouldReturnTheActiveScene()
        {
            Construct();
            IScene mockScene = Substitute.For<IScene>();

            _sut.SetCurrentScene(mockScene);

            Assert.AreSame(mockScene, _sut.CurrentScene);
        }

        [TestMethod]
        public void ScenesShouldNotBeNull()
        {
            Construct();

            Assert.IsNotNull(_sut.Scenes);
        }
        #endregion

        #region LoadContent Tests
        // LoadContent currently does nothing extra.
        #endregion

        #region Initialize Tests
        [TestMethod]
        public void InitializeShouldCallScreenManagerApplySettings()
        {
            Construct();

            _sut.Initialize();

            _mockScreenManager.Received(1).ApplySettings(_mockGrfxDeviceManager);
        }
        #endregion

        #region Exit Tests
        [TestMethod]
        public void ExitShouldSetFinishedToTrue()
        {
            Construct();

            _sut.Exit();

            Assert.IsTrue(_sut.Finished);
        }
        #endregion

        #region Suspend Tests
        [TestMethod]
        public void SuspendShouldCallAudioManagerMusicControllerSuspend()
        {
            Construct();

            _sut.Suspend();

            _mockAudioManager.MusicController.Received(1).Suspend();
        }
        #endregion

        #region Resume Tests
        [TestMethod]
        public void ResumeShouldCallAudioManagerMusicControllerResume()
        {
            Construct();

            _sut.Resume();

            _mockAudioManager.MusicController.Received(1).Resume();
        }
        #endregion

        #region TriggerExternalAction Tests
        [TestMethod]
        public void TriggerExternalActionShouldFireExternalActionOccurredEvent()
        {
            Construct();

            var subscriber = Substitute.For<EventHandler<ExternalActionEventArgs>>();
            _sut.ExternalActionOccurred += subscriber;

            _sut.TriggerExternalAction("testData");

            subscriber.Received(1)(_sut, Arg.Is<ExternalActionEventArgs>(x => x.Data.ToString() == "testData"));
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldCallInputManagerUpdate()
        {
            Construct();
            GameTime gameTime = new GameTime();

            _sut.Update(gameTime);

            _mockSurfaceInputManager.Received(1).Update(gameTime);
        }

        [TestMethod]
        public void UpdateShouldCallAudioManagerUpdate()
        {
            Construct();
            GameTime gameTime = new GameTime();

            _sut.Update(gameTime);

            _mockAudioManager.Received(1).Update(gameTime);
        }

        [TestMethod]
        public void UpdateWithNullCurrentSceneShouldNotThrowException()
        {
            Construct();

            _sut.Update(new GameTime());
        }

        [TestMethod]
        public void UpdateShouldUpdateCurrentScene()
        {
            Construct();
            GameTime gameTime = new GameTime();
            _sut.SetCurrentScene(Substitute.For<IScene>());

            _sut.Update(gameTime);

            _sut.CurrentScene.Received(1).Update(gameTime);
        }

        [TestMethod]
        public void UpdateWithCurrentSceneCompleteAndNextSceneSetShouldCallInputManagerClearConsumer()
        {
            Construct();
            IScene mockScene = Substitute.For<IScene>();
            mockScene.SceneComplete.Returns(true);
            mockScene.NextScene.Returns(Substitute.For<IScene>());
            _sut.SetCurrentScene(mockScene);

            _sut.Update(new GameTime());

            _mockSurfaceInputManager.Received(1).ClearConsumer();
        }

        [TestMethod]
        public void UpdateWithCurrentSceneCompleteAndNextSceneSetShouldSetCurrentSceneToExpectedScene()
        {
            Construct();
            IScene mockScene = Substitute.For<IScene>();
            mockScene.SceneComplete.Returns(true);

            IScene mockNextScene = Substitute.For<IScene>();
            mockScene.NextScene.Returns(mockNextScene);

            _sut.SetCurrentScene(mockScene);

            _sut.Update(new GameTime());

            Assert.AreSame(mockNextScene, _sut.CurrentScene);
        }

        [TestMethod]
        public void UpdateWithCurrentSceneCompleteAndNextSceneSetShouldMakeMethodCallsOnSceneInExpectedOrder()
        {
            Construct();
            IScene mockScene = Substitute.For<IScene>();
            mockScene.SceneComplete.Returns(true);

            IScene mockNextScene = Substitute.For<IScene>();
            mockScene.NextScene.Returns(mockNextScene);

            _sut.SetCurrentScene(mockScene);

            _sut.Update(new GameTime());

            Received.InOrder(
                () =>
                {
                    mockScene.End();
                    mockNextScene.Reset();
                    mockNextScene.Start();
                }
            );
        }

        [TestMethod]
        public void UpdateWithCurrentSceneCompleteAndNextSceneSetShouldSetCurrentSceneToExpectedScene_MultipleScenes()
        {
            Construct();
            IScene mockScene = Substitute.For<IScene>();
            mockScene.SceneComplete.Returns(true);

            IScene mockNextScene = Substitute.For<IScene>();
            mockNextScene.SceneComplete.Returns(true);
            mockScene.NextScene.Returns(mockNextScene);

            IScene mockNextNextScene = Substitute.For<IScene>();
            mockNextScene.NextScene.Returns(mockNextNextScene);

            _sut.SetCurrentScene(mockScene);

            _sut.Update(new GameTime());

            Assert.AreSame(mockNextNextScene, _sut.CurrentScene);
        }

        [TestMethod]
        public void UpdateWithCurrentSceneCompleteAndNextSceneSetShouldMakeMethodCallsOnSceneInExpectedOrder_MultipleScenes()
        {
            Construct();
            IScene mockScene = Substitute.For<IScene>();
            mockScene.SceneComplete.Returns(true);

            IScene mockNextScene = Substitute.For<IScene>();
            mockNextScene.SceneComplete.Returns(true);
            mockScene.NextScene.Returns(mockNextScene);

            IScene mockNextNextScene = Substitute.For<IScene>();
            mockNextScene.NextScene.Returns(mockNextNextScene);

            _sut.SetCurrentScene(mockScene);

            _sut.Update(new GameTime());

            Received.InOrder(
                () =>
                {
                    mockScene.End();
                    mockNextScene.Reset();
                    mockNextScene.Start();

                    mockNextScene.End();
                    mockNextNextScene.Reset();
                    mockNextNextScene.Start();
                }
            );
        }

        [TestMethod]
        public void UpdateWithCurrentSceneCompleteAndNextSceneNotSetShouldCallCurrentSceneEnd()
        {
            Construct();
            IScene mockScene = Substitute.For<IScene>();
            mockScene.SceneComplete.Returns(true);
            mockScene.NextScene.Returns((IScene)null);
            _sut.SetCurrentScene(mockScene);

            _sut.Update(new GameTime());

            mockScene.Received(1).End();
        }

        [TestMethod]
        public void UpdateWithCurrentSceneCompleteAndNextSceneNotSetShouldSetFinishedToTrue()
        {
            Construct();
            IScene mockScene = Substitute.For<IScene>();
            mockScene.SceneComplete.Returns(true);
            mockScene.NextScene.Returns((IScene)null);
            _sut.SetCurrentScene(mockScene);

            _sut.Update(new GameTime());

            Assert.IsTrue(_sut.Finished);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawWithNullCurrentSceneShouldNotThrowException()
        {
            Construct();

            _sut.Draw(new GameTime());
        }

        [TestMethod]
        public void DrawShouldCallCurrentScenePreDraw()
        {
            Construct();
            GameTime gameTime = new GameTime();
            IScene mockScene = Substitute.For<IScene>();
            _sut.SetCurrentScene(mockScene);

            _sut.Draw(gameTime);

            mockScene.Received(1).PreDraw(gameTime);
        }

        [TestMethod]
        public void DrawShouldCallScreenManagerBeginDraw()
        {
            Construct();

            _sut.Draw(new GameTime());

            _mockScreenManager.Received(1).BeginDraw(_mockGrfxDeviceManager.GraphicsDevice);
        }

        [TestMethod]
        public void DrawShouldCallCurrentSceneDraw()
        {
            Construct();
            IScene mockScene = Substitute.For<IScene>();
            _sut.SetCurrentScene(mockScene);
            GameTime gameTime = new GameTime();

            _sut.Draw(gameTime);

            mockScene.Received(1).Draw(gameTime);
        }

        [TestMethod]
        public void DrawShouldMakeCallsInExpectedOrder()
        {
            Construct();
            IScene mockScene = Substitute.For<IScene>();
            _sut.SetCurrentScene(mockScene);
            GameTime gameTime = new GameTime();

            _sut.Draw(gameTime);

            Received.InOrder(
                () =>
                {
                    mockScene.PreDraw(gameTime);
                    _mockScreenManager.BeginDraw(_mockGrfxDeviceManager.GraphicsDevice);
                    mockScene.Draw(gameTime);
                }
            );
        }
        #endregion
    }
}
