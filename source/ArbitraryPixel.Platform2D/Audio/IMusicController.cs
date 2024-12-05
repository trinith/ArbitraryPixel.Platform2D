using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.ContentManagement;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Audio
{
    /// <summary>
    /// Represents an object that controls music playback.
    /// </summary>
    public interface IMusicController
    {
        /// <summary>
        /// Whether or not music is enabled for this controller.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Whether or not this controller has been suspended.
        /// </summary>
        bool IsSuspended { get; }

        /// <summary>
        /// Whether or not this controller is playing music.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// The user specified volume for this controller. The final volume of playback will be Volume * Attenuation.
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// The attenuation for this controller. The final volume of playback will be Volume * Attenuation. If this value is set while a FadeVolumeAttenuation operation is occuring, the fade will stop and attenuation will be overridden with this value.
        /// </summary>
        float VolumeAttenuation { get; set; }

        /// <summary>
        /// Whether or not music playback for this controller is muted.
        /// </summary>
        bool IsMuted { get; set; }

        /// <summary>
        /// Whether or not music playback for this controller should loop.
        /// </summary>
        bool IsRepeating { get; set; }

        /// <summary>
        /// Create a song that this music controller can play.
        /// </summary>
        /// <param name="content">An object responsible for managing content.</param>
        /// <param name="assetName">The asset name of the song to create.</param>
        /// <returns>The created song.</returns>
        ISong CreateSong(IContentManager content, string assetName);

        /// <summary>
        /// Play the supplied song.
        /// </summary>
        /// <param name="song">The song to play.</param>
        void Play(ISong song);

        /// <summary>
        /// Stop the currently playing song.
        /// </summary>
        void Stop();

        /// <summary>
        /// Suspend this controller, pausing any playback that is currently occuring.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Resume this controller. The song that was last asked to play will be resumed, if one exists.
        /// </summary>
        void Resume();

        /// <summary>
        /// Fade the current music volume to a new value.
        /// </summary>
        /// <param name="newVolume">The new music volume, between 0 and 1.</param>
        /// <param name="fadeTime">The amount of time, in seconds, the fade should take.</param>
        /// <param name="holdTime">The amount of time, in seconds, to delay before performing the fade.</param>
        void FadeVolumeAttenuation(float newVolume, double fadeTime, double holdTime = 0.0);

        /// <summary>
        /// Update this controller.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        void Update(GameTime gameTime);
    }
}
