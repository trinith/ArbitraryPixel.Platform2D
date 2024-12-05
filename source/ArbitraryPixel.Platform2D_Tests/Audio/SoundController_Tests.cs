using System;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Audio.Factory;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Platform2D.Audio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Audio
{
    [TestClass]
    public class SoundController_Tests
    {
        private SoundController _sut;
        private ISoundResourceFactory _mockFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockFactory = Substitute.For<ISoundResourceFactory>();
        }

        private void Construct()
        {
            _sut = new SoundController(_mockFactory);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_Factory()
        {
            _sut = new SoundController(null);
        }
        #endregion

        #region Enabled Tests
        [TestMethod]
        public void EnabledShouldDefaultTrue()
        {
            Construct();

            Assert.IsTrue(_sut.Enabled);
        }

        [TestMethod]
        public void EnabledShouldReturnSetValue()
        {
            Construct();

            _sut.Enabled = false;

            Assert.IsFalse(_sut.Enabled);
        }

        [TestMethod]
        public void EnabledWithNewValueShouldSetEnabledOnOwnedResources()
        {
            Construct();

            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);
            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");
            mockResource.ClearReceivedCalls();

            _sut.Enabled = false;

            mockResource.Received(1).Enabled = false;
        }

        [TestMethod]
        public void EnabledWithSameValueShouldNotUpdateOwnedResourceEnabled()
        {
            Construct();

            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);
            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");
            mockResource.ClearReceivedCalls();

            _sut.Enabled = true;

            mockResource.Received(0).Enabled = Arg.Any<bool>();
        }
        #endregion

        #region Volume Tests
        [TestMethod]
        public void VolumeShouldDefaultToExpectedValue()
        {
            Construct();

            Assert.AreEqual<float>(1f, _sut.Volume);
        }

        [TestMethod]
        public void VolumeShouldReturnSetValue()
        {
            Construct();

            _sut.Volume = 0.75f;

            Assert.AreEqual<float>(0.75f, _sut.Volume);
        }

        [TestMethod]
        public void VolumeSetShouldUpdateOwnedResources()
        {
            Construct();

            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);
            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");
            mockResource.ClearReceivedCalls();

            _sut.Volume = 0.75f;

            mockResource.Received(1).MasterVolume = 0.75f;
        }
        #endregion

        #region CreateSoundResource Tests
        [TestMethod]
        public void CreateSoundResourceShouldCallFactoryCreate()
        {
            Construct();
            IContentManager mockContent = Substitute.For<IContentManager>();

            _sut.CreateSoundResource(mockContent, "testAsset");

            _mockFactory.Received(1).Create(mockContent, "testAsset");
        }

        [TestMethod]
        public void CreateSoundResourceShouldSetCreatedResourceEnabledToControllerValue_TestA()
        {
            Construct();
            _sut.Enabled = true;
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);

            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");

            mockResource.Received(1).Enabled = true;
        }

        [TestMethod]
        public void CreateSoundResourceShouldSetCreatedResourceEnabledToControllerValue_TestB()
        {
            Construct();
            _sut.Enabled = false;
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);

            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");

            mockResource.Received(1).Enabled = false;
        }

        [TestMethod]
        public void CreateSoundResourceShouldSetCreatedResourceVolumeToControllerValue_TestA()
        {
            Construct();
            _sut.Volume = 0.123f;
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);

            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");

            mockResource.Received(1).MasterVolume = 0.123f;
        }

        [TestMethod]
        public void CreateSoundResourceShouldSetCreatedResourceVolumeToControllerValue_TestB()
        {
            Construct();
            _sut.Volume = 0.75f;
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);

            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");

            mockResource.Received(1).MasterVolume = 0.75f;
        }

        [TestMethod]
        public void CreateSoundResourceShouldSubscribeToCreatedResourceDisposedEvent()
        {
            Construct();
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);

            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");

            mockResource.Received(1).Disposed += Arg.Any<EventHandler<EventArgs>>();
        }
        #endregion

        #region Stop All Tests
        [TestMethod]
        public void StopAllShouldCallStopAllOnAllOwnedResources()
        {
            Construct();
            _sut.Enabled = false;
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);

            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");

            _sut.StopAll();

            mockResource.Received(1).StopAll();
        }
        #endregion

        #region ClearOwnedResources Tests
        [TestMethod]
        public void ClearOwnedResourcesShouldDisposeOwnedSounds()
        {
            Construct();
            _sut.Enabled = false;
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);
            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");

            _sut.ClearOwnedResources();

            mockResource.Received(1).Dispose();
        }
        #endregion

        #region SoundResource Disposed Event Test
        [TestMethod]
        public void DisposedResourceShouldRemoveItselfFromOwnedSounds()
        {
            Construct();
            _sut.Enabled = false;
            ISoundResource mockResource = Substitute.For<ISoundResource>();
            _mockFactory.Create(Arg.Any<IContentManager>(), Arg.Any<string>()).Returns(mockResource);
            _sut.CreateSoundResource(Substitute.For<IContentManager>(), "asdf");
            mockResource.ClearReceivedCalls();

            mockResource.Disposed += Raise.Event<EventHandler<EventArgs>>(mockResource, new EventArgs());

            _sut.Volume = 0.75f;

            mockResource.Received(0).MasterVolume = Arg.Any<float>();
        }
        #endregion

        #region EnabledChanged Event Tests
        [TestMethod]
        public void EnabledSetToNewValueShouldFireEnabledChangedEvent()
        {
            var sub = Substitute.For<EventHandler<StateChangedEventArgs<bool>>>();

            Construct();
            _sut.Enabled = true;
            _sut.EnabledChanged += sub;

            _sut.Enabled = false;

            sub.Received(1)(_sut, Arg.Is<StateChangedEventArgs<bool>>(x => x.PreviousState == true && x.CurrentState == false));
        }

        [TestMethod]
        public void EnabledSetToSameValueShouldNotFireEnabledChangedEvent()
        {
            var sub = Substitute.For<EventHandler<StateChangedEventArgs<bool>>>();

            Construct();
            _sut.Enabled = false;
            _sut.EnabledChanged += sub;

            _sut.Enabled = false;

            sub.Received(0)(_sut, Arg.Any<StateChangedEventArgs<bool>>());
        }
        #endregion
    }
}
