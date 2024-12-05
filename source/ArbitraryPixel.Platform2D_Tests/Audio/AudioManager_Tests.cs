using ArbitraryPixel.Platform2D.Audio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.Platform2D_Tests.Audio
{
    [TestClass]
    public class AudioManager_Tests
    {
        private AudioManager _sut;
        private IMusicController _mockMusicController;
        private ISoundController _mockSoundController;

        [TestInitialize]
        public void Initialize()
        {
            _mockMusicController = Substitute.For<IMusicController>();
            _mockSoundController = Substitute.For<ISoundController>();
        }

        private void Construct()
        {
            _sut = new AudioManager(_mockMusicController, _mockSoundController);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_MusicController()
        {
            _sut = new AudioManager(null, _mockSoundController);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SoundController()
        {
            _sut = new AudioManager(_mockMusicController, null);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void MusicControllerShouldReturnConstructorProperty()
        {
            Construct();

            Assert.AreSame(_mockMusicController, _sut.MusicController);
        }

        [TestMethod]
        public void SoundControllerShouldReturnConstructorProperty()
        {
            Construct();

            Assert.AreSame(_mockSoundController, _sut.SoundController);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldUpdateMusicController()
        {
            Construct();
            GameTime expected = new GameTime();

            _sut.Update(expected);

            _mockMusicController.Received(1).Update(expected);
        }

        [TestMethod]
        public void UpdateShouldUpdateSoundController()
        {
            Construct();
            GameTime expected = new GameTime();

            _sut.Update(expected);

            _mockSoundController.Received(1).Update(expected);
        }
        #endregion
    }
}
