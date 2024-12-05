using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Platform2D.Audio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.Platform2D_Tests.Audio
{
    [TestClass]
    public class MusicController_Tests
    {
        private MusicController _sut;
        private ISongPlayer _mockPlayer;

        [TestInitialize]
        public void Initialize()
        {
            _mockPlayer = Substitute.For<ISongPlayer>();
        }

        private void Construct()
        {
            _sut = new MusicController(_mockPlayer);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SongManager()
        {
            _sut = new MusicController(null);
        }
        #endregion

        #region SongFactory Tests
        [TestMethod]
        public void CreateSongShouldCallPlayerFactoryCreate()
        {
            Construct();
            IContentManager mockContent = Substitute.For<IContentManager>();

            _sut.CreateSong(mockContent, "test");

            _mockPlayer.Factory.Received(1).Create(mockContent, "test");
        }

        [TestMethod]
        public void CreateSongShouldReturnExpectedObject()
        {
            Construct();
            IContentManager mockContent = Substitute.For<IContentManager>();
            ISong mockSong = Substitute.For<ISong>();
            _mockPlayer.Factory.Create(mockContent, "test").Returns(mockSong);

            Assert.AreSame(mockSong, _sut.CreateSong(mockContent, "test"));
        }
        #endregion

        #region IsPlaying Tests
        [TestMethod]
        public void IsPlayingShouldDefaultFalse()
        {
            Construct();

            Assert.IsFalse(_sut.IsPlaying);
        }

        [TestMethod]
        public void IsPlayingAfterPlayShouldReturnTrue()
        {
            Construct();

            _sut.Play(Substitute.For<ISong>());

            Assert.IsTrue(_sut.IsPlaying);
        }

        [TestMethod]
        public void IsPlayingAfterPlayThenStopShouldReturnFalse()
        {
            Construct();
            _sut.Play(Substitute.For<ISong>());

            _sut.Stop();

            Assert.IsFalse(_sut.IsPlaying);
        }

        [TestMethod]
        public void IsPlayingAfterSuspendShouldReturnFalse()
        {
            Construct();
            _sut.Play(Substitute.For<ISong>());

            _sut.Suspend();

            Assert.IsFalse(_sut.IsPlaying);
        }

        [TestMethod]
        public void IsPlayingAfterSuspendThenResumeShouldReturnTrue()
        {
            Construct();
            _sut.Play(Substitute.For<ISong>());
            _sut.Suspend();

            _sut.Resume();

            Assert.IsTrue(_sut.IsPlaying);
        }
        #endregion

        #region IsSuspended Tests
        [TestMethod]
        public void IsSuspendedShouldDefaultFalse()
        {
            Construct();

            Assert.IsFalse(_sut.IsSuspended);
        }

        [TestMethod]
        public void IsSuspendedAfterSuspendShouldReturnTrue()
        {
            Construct();
            _sut.Suspend();

            Assert.IsTrue(_sut.IsSuspended);
        }

        [TestMethod]
        public void IsSuspendedAfterSuspendThenResumeShouldReturnFalse()
        {
            Construct();
            _sut.Suspend();

            _sut.Resume();

            Assert.IsFalse(_sut.IsSuspended);
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
        public void EnabledWhenSetFalseAndPlayingShouldStopSong()
        {
            Construct();
            _sut.Play(Substitute.For<ISong>());

            _sut.Enabled = false;

            Assert.IsFalse(_sut.IsPlaying);
        }

        [TestMethod]
        public void EnabledWhenSetFalseAndPlayingShouldCallPlayerStop()
        {
            Construct();
            _sut.Play(Substitute.For<ISong>());

            _sut.Enabled = false;

            _mockPlayer.Received(1).Stop();
        }

        [TestMethod]
        public void EnabledWhenSetTrueAfterDisableWhilePlayingShouldResumePlayingSong()
        {
            Construct();
            _sut.Play(Substitute.For<ISong>());
            _sut.Enabled = false;

            _sut.Enabled = true;

            Assert.IsTrue(_sut.IsPlaying);
        }

        [TestMethod]
        public void EnabledWhenSetTrueAfterDisableWhilePlayingShouldCallPlayerPlayWithPreviousSong()
        {
            Construct();
            ISong mockSong = Substitute.For<ISong>();
            _sut.Play(mockSong);
            _sut.Enabled = false;
            _mockPlayer.ClearReceivedCalls();

            _sut.Enabled = true;

            _mockPlayer.Received(1).Play(mockSong);
        }

        [TestMethod]
        public void EnableWhenSetTrueAfterDisableWhileNotPlayingShouldNotSetIsPlayingTrue()
        {
            Construct();
            _sut.Enabled = false;

            _sut.Enabled = true;

            Assert.IsFalse(_sut.IsPlaying);
        }
        #endregion

        #region Volume (Not Attenuated) Tests
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

            _sut.Volume = 0.5f;

            Assert.AreEqual<float>(0.5f, _sut.Volume);
        }

        [TestMethod]
        public void VolumeShouldUpdatePlayerVolume()
        {
            Construct();

            _sut.Volume = 0.5f;

            _mockPlayer.Received(1).Volume = 0.5f;
        }

        [TestMethod]
        public void VolumeSetWithValueBelowZeroShouldClampToZero()
        {
            Construct();

            _sut.Volume = -0.25f;

            Assert.AreEqual<float>(0, _sut.Volume);
        }

        [TestMethod]
        public void VolumeSEtWithValueAboveOneShouldClampToOne()
        {
            Construct();

            _sut.Volume = 1.25f;
        }
        #endregion

        #region VolumeAttenuationTests
        [TestMethod]
        public void VolumeAttenuationShouldDefaultToExpectedValue()
        {
            Construct();

            Assert.AreEqual<float>(1f, _sut.VolumeAttenuation);
        }

        [TestMethod]
        public void VolumeAttenuationShouldReturnSetValue()
        {
            Construct();

            _sut.VolumeAttenuation = 0.14f;

            Assert.AreEqual<float>(0.14f, _sut.VolumeAttenuation);
        }

        [TestMethod]
        public void VolumeAttenuationSetWithValueBelowZeroReturnsZero()
        {
            Construct();

            _sut.VolumeAttenuation = -0.25f;

            Assert.AreEqual<float>(0f, _sut.VolumeAttenuation);
        }

        [TestMethod]
        public void VolumeAttenuationWithValueAboveOneShouldReturnOne()
        {
            Construct();

            _sut.VolumeAttenuation = 1.25f;

            Assert.AreEqual<float>(1f, _sut.VolumeAttenuation);
        }

        [TestMethod]
        public void VolumeAttenuationShouldUpdatePlayerVolumeWithExpectedValue_TestA()
        {
            Construct();
            _sut.Volume = 0.75f;
            _mockPlayer.ClearReceivedCalls();

            _sut.VolumeAttenuation = 0.5f;

            _mockPlayer.Received(1).Volume = 0.5f * 0.75f;
        }

        [TestMethod]
        public void VolumeAttenuationShouldUpdatePlayerVolumeWithExpectedValue_TestB()
        {
            Construct();
            _sut.Volume = 0.25f;
            _mockPlayer.ClearReceivedCalls();

            _sut.VolumeAttenuation = 0.33f;

            _mockPlayer.Received(1).Volume = 0.33f * 0.25f;
        }

        [TestMethod]
        public void VolumeAttenuationSetShouldOverrideFade()
        {
            Construct();
            _sut.FadeVolumeAttenuation(0.5f, 1.0f);
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.25)));

            _sut.VolumeAttenuation = 0.25f;
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(10)));

            Assert.AreEqual<float>(0.25f, _sut.VolumeAttenuation);
        }

        [TestMethod]
        public void VolumeAttenuationSetShouldOverrideFadeAndNotUpdateMusicPlayerVolumeOnUpdate()
        {
            Construct();
            _sut.FadeVolumeAttenuation(0.5f, 1.0f);
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.25)));
            _sut.VolumeAttenuation = 0.25f;
            _mockPlayer.ClearReceivedCalls();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(10)));

            _mockPlayer.Received(0).Volume = Arg.Any<float>();
        }
        #endregion

        #region IsMuted Tests
        [TestMethod]
        public void IsMutedReturnsPlayerIsMuted()
        {
            Construct();
            _mockPlayer.IsMuted = true;

            Assert.IsTrue(_sut.IsMuted);
        }

        [TestMethod]
        public void IsMutedShouldSetPlayerIsMuted()
        {
            Construct();
            _sut.IsMuted = true;

            _mockPlayer.Received(1).IsMuted = true;
        }
        #endregion

        #region IsRepeating Tests
        [TestMethod]
        public void IsRepeatingReturnsPlayerIsRepeating()
        {
            Construct();
            _mockPlayer.IsRepeating = true;

            Assert.IsTrue(_sut.IsRepeating);
        }

        [TestMethod]
        public void IsRepeatingShouldSetPlayerIsRepeating()
        {
            Construct();
            _sut.IsRepeating = true;

            _mockPlayer.Received(1).IsRepeating = true;
        }
        #endregion

        #region Play Tests
        [TestMethod]
        public void PlayWhenEnabledShouldCallPlayerPlay()
        {
            Construct();
            ISong mockSong = Substitute.For<ISong>();

            _sut.Play(mockSong);

            _mockPlayer.Received(1).Play(mockSong);
        }

        [TestMethod]
        public void PlayWhenEnabledShouldSetIsPlayingTrue()
        {
            Construct();

            _sut.Play(Substitute.For<ISong>());

            Assert.IsTrue(_sut.IsPlaying);
        }

        [TestMethod]
        public void PlayWhenDisabledShouldNotCallPlayerPlay()
        {
            Construct();
            _sut.Enabled = false;

            _sut.Play(Substitute.For<ISong>());

            _mockPlayer.DidNotReceive().Play(Arg.Any<ISong>());
        }

        [TestMethod]
        public void PlayWhenDisabledShouldPlaySongWhenEnableSetToTrue()
        {
            Construct();
            _sut.Enabled = false;
            ISong mockSong = Substitute.For<ISong>();
            _sut.Play(mockSong);

            _sut.Enabled = true;

            _mockPlayer.Received(1).Play(mockSong);
        }
        #endregion

        #region Stop Tests
        [TestMethod]
        public void StopShouldCallPlayerStop()
        {
            Construct();

            _sut.Stop();

            _mockPlayer.Received(1).Stop();
        }

        [TestMethod]
        public void StopDuringPlayShouldSetIsPlayingFalse()
        {
            Construct();
            _sut.Play(Substitute.For<ISong>());

            _sut.Stop();

            Assert.IsFalse(_sut.IsPlaying);
        }

        [TestMethod]
        public void StopDuringSuspendShouldNotCallPlayerPlayOnResume()
        {
            Construct();
            ISong mockSong = Substitute.For<ISong>();
            _sut.Play(mockSong);
            _mockPlayer.PlayPosition.Returns(TimeSpan.FromSeconds(5));
            _sut.Update(new GameTime());
            _sut.Suspend();
            _sut.Stop();
            _mockPlayer.ClearReceivedCalls();

            _sut.Resume();

            _mockPlayer.Received(0).Play(Arg.Any<ISong>());
            _mockPlayer.Received(0).Play(Arg.Any<ISong>(), Arg.Any<TimeSpan>());
        }
        #endregion

        #region Suspend / Resume Tests
        [TestMethod]
        public void SuspendWhenNotSuspendedShouldCallPlayerStop()
        {
            Construct();
            _sut.Suspend();

            _mockPlayer.Received(1).Stop();
        }

        [TestMethod]
        public void SuspendWhenSuspendedShouldNotCallPlayerStop()
        {
            Construct();
            _sut.Suspend();
            _mockPlayer.ClearReceivedCalls();

            _sut.Suspend();

            _mockPlayer.Received(0).Stop();
        }

        [TestMethod]
        public void ResumeWhileSuspendedWithPausedSongShouldResumeFromPreviousPosition()
        {
            Construct();
            _mockPlayer.PlayPosition.Returns(TimeSpan.FromSeconds(35));
            ISong mockSong = Substitute.For<ISong>();
            _sut.Play(mockSong);
            _sut.Update(new GameTime());
            _sut.Suspend();

            _sut.Resume();

            _mockPlayer.Received(1).Play(mockSong, TimeSpan.FromSeconds(35));
        }

        [TestMethod]
        public void ResumeWhileSuspendedWithoutPausedSongShouldNotCallPlayerPlay()
        {
            Construct();
            _sut.Suspend();

            _sut.Resume();

            _mockPlayer.Received(0).Play(Arg.Any<ISong>());
            _mockPlayer.Received(0).Play(Arg.Any<ISong>(), Arg.Any<TimeSpan>());
        }

        [TestMethod]
        public void ResumeWhileSuspendedWhenDisabledWithPausedSongShouldNotCallPlayerPlay()
        {
            Construct();
            _sut.Play(Substitute.For<ISong>());
            _sut.Suspend();
            _sut.Enabled = false;
            _mockPlayer.ClearReceivedCalls();

            _sut.Resume();

            _mockPlayer.Received(0).Play(Arg.Any<ISong>());
            _mockPlayer.Received(0).Play(Arg.Any<ISong>(), Arg.Any<TimeSpan>());
        }

        [TestMethod]
        public void ResumeWhenNotSuspendedWhileEnabledWithoutPausedSongShouldNotCallPlayerPlay()
        {
            Construct();
            _sut.Play(Substitute.For<ISong>());
            _sut.Stop();
            _sut.Suspend();
            _mockPlayer.ClearReceivedCalls();

            _sut.Resume();

            _mockPlayer.Received(0).Play(Arg.Any<ISong>());
            _mockPlayer.Received(0).Play(Arg.Any<ISong>(), Arg.Any<TimeSpan>());
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateAfterFadeMusicAttenuation_NoHoldTime_ShouldChangeMusicVolume_TestA()
        {
            Construct();
            _sut.Volume = 0.5f;
            _sut.VolumeAttenuation = 0.5f;
            _sut.FadeVolumeAttenuation(1.0f, 1.0f);
            _mockPlayer.ClearReceivedCalls();

            float expectedVelocity = (1f - 0.5f) / 1f;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.1)));

            _mockPlayer.Received(1).Volume = 0.5f * (0.5f + expectedVelocity * 0.1f);
        }

        [TestMethod]
        public void UpdateAfterFadeMusicAttenuation_NoHoldTime_ShouldChangeMusicVolume_TestB()
        {
            Construct();
            _sut.Volume = 0.75f;
            _sut.VolumeAttenuation = 0.5f;
            _sut.FadeVolumeAttenuation(0, 1.0f);
            _mockPlayer.ClearReceivedCalls();

            float expectedVelocity = (0f - 0.5f) / 1f;

            _mockPlayer.When(x => x.Volume = Arg.Any<float>()).Do(
                x =>
                {
                    float inVol = (float)x[0];
                    Assert.That.AlmostEqual(0.75f * (0.5f + expectedVelocity * 0.1f), inVol);
                }
            );

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.1)));
        }

        [TestMethod]
        public void UpdateAfterFadeMusicAttenuation_NoHoldTime_VolumePastBoundsShouldClampToTarget()
        {
            Construct();
            _sut.Volume = 0.90f;
            _sut.VolumeAttenuation = 0.5f;
            _sut.FadeVolumeAttenuation(1f, 5f);
            _mockPlayer.ClearReceivedCalls();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(10)));

            _mockPlayer.Received(1).Volume = 0.9f;
        }

        [TestMethod]
        public void UpdateAfterFadeMusicAttenuation_WithHoldTime_VolumeAdjustedAtExpectedTime_BeforeHoldExpires()
        {
            Construct();
            _sut.Volume = 0.5f;
            _sut.VolumeAttenuation = 0.5f;
            _sut.FadeVolumeAttenuation(1f, 2f, 3f);
            _mockPlayer.ClearReceivedCalls();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1.5)));

            _mockPlayer.Received(0).Volume = Arg.Any<float>();
        }

        [TestMethod]
        public void UpdateAfterFadeMusicAttenuation_WithHoldTime_VolumeAdjustedAtExpectedTime_AfterHoldExpires()
        {
            Construct();
            _sut.Volume = 0.5f;
            _sut.VolumeAttenuation = 0.5f;
            _sut.FadeVolumeAttenuation(1f, 2f, 3f);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3.1)));

            float expectedVelocity = (1f - 0.5f) / 2f;

            _mockPlayer.Received(1).Volume = 0.5f * (0.5f + expectedVelocity * 0.1f);
        }
        #endregion
    }
}
