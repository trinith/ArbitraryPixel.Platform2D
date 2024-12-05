using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Audio
{
    /// <summary>
    /// Represents an object that can manage audio.
    /// </summary>
    public interface IAudioManager
    {
        /// <summary>
        /// An object responsible for controlling the music for this Audio Manager.
        /// </summary>
        IMusicController MusicController { get; }

        /// <summary>
        /// An object responsible for controlling the sounds for this Audio Manager.
        /// </summary>
        ISoundController SoundController { get; }

        /// <summary>
        /// Update this audio manager.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        void Update(GameTime gameTime);
    }
}
