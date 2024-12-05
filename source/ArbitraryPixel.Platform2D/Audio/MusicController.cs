using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Audio.Factory;
using ArbitraryPixel.Common.ContentManagement;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.Audio
{
    /// <summary>
    /// An object responsible for controlling music playback.
    /// </summary>
    public class MusicController : IMusicController
    {
        #region Private Methods
        private ISong _currentSong = null;
        private TimeSpan _currentPosition = TimeSpan.Zero;
        private TimeSpan _savedPosition = TimeSpan.Zero;
        private ISongPlayer _player = null;
        private bool _enabled = true;
        private float _volume = 1f;
        private float _volumeAttenuation = 1f;
        private float _volumeAttentuationVelocity = 0f;
        private float _volumeAttentuationTarget = 0f;
        private double _volumeChangeHoldTime = 0.0;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="player">An object responsible for playing songs.</param>
        public MusicController(ISongPlayer player)
        {
            _player = player ?? throw new ArgumentNullException();
        }
        #endregion

        #region IMusicController Implementation
        /// <summary>
        /// Whether or not this controller is playing music.
        /// </summary>
        public bool IsPlaying { get; private set; } = false;

        /// <summary>
        /// Whether or not this controller has been suspended.
        /// </summary>
        public bool IsSuspended { get; private set; } = false;

        /// <summary>
        /// Whether or not music is enabled for this controller.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;

                    if (_enabled == false && this.IsPlaying)
                    {
                        var currentSong = _currentSong;
                        this.Stop();
                        _currentSong = currentSong;
                    }
                    else if (_enabled == true && _currentSong != null)
                    {
                        this.Play(_currentSong);
                    }
                }
            }
        }

        /// <summary>
        /// The user specified volume for this controller. The final volume of playback will be Volume * Attenuation.
        /// </summary>
        public float Volume
        {
            get { return _volume; }
            set
            {
                _volume = MathHelper.Clamp(value, 0f, 1f);
                UpdatePlayerVolume();
            }
        }

        /// <summary>
        /// The attenuation for this controller. The final volume of playback will be Volume * Attenuation. If this value is set while a FadeVolumeAttenuation operation is occuring, the fade will stop and attenuation will be overridden with this value.
        /// </summary>
        public float VolumeAttenuation
        {
            get { return _volumeAttenuation; }
            set
            {
                _volumeAttenuation = MathHelper.Clamp(value, 0f, 1f);
                _volumeAttentuationVelocity = 0f;
                _volumeAttentuationTarget = 0f;
                _volumeChangeHoldTime = 0.0;
                UpdatePlayerVolume();
            }
        }

        /// <summary>
        /// Whether or not music playback for this controller is muted.
        /// </summary>
        public bool IsMuted
        {
            get { return _player.IsMuted; }
            set { _player.IsMuted = value; }
        }

        /// <summary>
        /// Whether or not music playback for this controller should loop.
        /// </summary>
        public bool IsRepeating
        {
            get { return _player.IsRepeating; }
            set { _player.IsRepeating = value; }
        }

        /// <summary>
        /// Create a song that this music controller can play.
        /// </summary>
        /// <param name="content">An object responsible for managing content.</param>
        /// <param name="assetName">The asset name of the song to create.</param>
        /// <returns>The created song.</returns>
        public ISong CreateSong(IContentManager content, string assetName)
        {
            return _player.Factory.Create(content, assetName);
        }

        /// <summary>
        /// Play the supplied song.
        /// </summary>
        /// <param name="song">The song to play.</param>
        public void Play(ISong song)
        {
            if (this.Enabled == false)
            {
                _currentSong = song;
                return;
            }

            _player.Play(song);

            this.IsPlaying = true;
            _savedPosition = TimeSpan.Zero;
            _currentSong = song;
        }

        /// <summary>
        /// Stop the currently playing song.
        /// </summary>
        public void Stop()
        {
            _player.Stop();
            _savedPosition = TimeSpan.Zero;
            this.IsPlaying = false;
            _currentSong = null;
        }

        /// <summary>
        /// Suspend this controller, pausing any playback that is currently occuring.
        /// </summary>
        public void Suspend()
        {
            if (this.IsSuspended == false)
            {
                _player.Stop();
                _savedPosition = _currentPosition;
                this.IsPlaying = false;
                this.IsSuspended = true;
            }
        }

        /// <summary>
        /// Resume this controller. The song that was last asked to play will be resumed, if one exists.
        /// </summary>
        public void Resume()
        {
            if (this.IsSuspended == true)
            {
                if (this.Enabled == true && _currentSong != null)
                {
                    _player.Play(_currentSong, _savedPosition);
                    _savedPosition = TimeSpan.Zero;
                    this.IsPlaying = true;
                }

                this.IsSuspended = false;
            }
        }

        /// <summary>
        /// Fade the current music volume to a new value.
        /// </summary>
        /// <param name="newVolume">The new music volume, between 0 and 1.</param>
        /// <param name="fadeTime">The amount of time, in seconds, the fade should take.</param>
        /// <param name="holdTime">The amount of time, in seconds, to delay before performing the fade.</param>
        public void FadeVolumeAttenuation(float newVolume, double fadeTime, double holdTime = 0.0)
        {
            _volumeAttentuationTarget = MathHelper.Clamp(newVolume, 0f, 1f);
            _volumeChangeHoldTime = holdTime;

            if (fadeTime == 0f)
            {
                _volumeAttentuationVelocity = float.MaxValue;
            }
            else
            {
                _volumeAttentuationVelocity = (float)((newVolume - _volumeAttenuation) / fadeTime);
            }
        }

        /// <summary>
        /// Update this controller.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        public void Update(GameTime gameTime)
        {
            if (this.IsPlaying)
                _currentPosition = _player.PlayPosition;
            else
                _currentPosition = TimeSpan.Zero;

            UpdateAttenuation(gameTime);
        }
        #endregion

        #region Private Methods
        private void UpdatePlayerVolume()
        {
            _player.Volume = _volume * _volumeAttenuation;
        }

        private void UpdateAttenuation(GameTime gameTime)
        {
            if (_volumeAttentuationVelocity != 0f)
            {
                double t = gameTime.ElapsedGameTime.TotalSeconds;

                if (_volumeChangeHoldTime > 0)
                {
                    if (_volumeChangeHoldTime - gameTime.ElapsedGameTime.TotalSeconds < 0)
                        t -= _volumeChangeHoldTime;

                    _volumeChangeHoldTime = _volumeChangeHoldTime - gameTime.ElapsedGameTime.TotalSeconds;
                    if (_volumeChangeHoldTime < 0)
                    {
                        _volumeChangeHoldTime = 0;
                    }
                }

                float v = (_volumeChangeHoldTime == 0.0) ? _volumeAttentuationVelocity : 0f;
                if (v != 0f)
                    _volumeAttenuation += (float)(v * t);

                if (
                       (_volumeAttentuationVelocity < 0 && _volumeAttenuation <= _volumeAttentuationTarget)
                    || (_volumeAttentuationVelocity > 0 && _volumeAttenuation >= _volumeAttentuationTarget)
                    )
                {
                    _volumeAttentuationVelocity = 0f;
                    _volumeAttenuation = _volumeAttentuationTarget;
                }

                if (_volumeChangeHoldTime == 0)
                    UpdatePlayerVolume();
            }
        }
        #endregion
    }
}
