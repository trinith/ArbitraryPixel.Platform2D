using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.Audio
{
    /// <summary>
    /// An object responsible for managing audio.
    /// </summary>
    public class AudioManager : IAudioManager
    {
        /// <summary>
        /// Create a new audio manager object.
        /// </summary>
        /// <param name="musicController">An object responsible for controlling background music.</param>
        /// <param name="soundController">An object responsible for controlling sound effects.</param>
        public AudioManager(IMusicController musicController, ISoundController soundController)
        {
            this.MusicController = musicController ?? throw new ArgumentNullException();
            this.SoundController = soundController ?? throw new ArgumentNullException();
        }

        #region IAudioManager Implementation
        /// <summary>
        /// An object responsible for controlling the music for this Audio Manager.
        /// </summary>
        public IMusicController MusicController { get; private set; }

        /// <summary>
        /// An object responsible for controlling the sounds for this Audio Manager.
        /// </summary>
        public ISoundController SoundController { get; private set; }

        /// <summary>
        /// Update this audio manager.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        public virtual void Update(GameTime gameTime)
        {
            this.MusicController.Update(gameTime);
            this.SoundController.Update(gameTime);
        }
        #endregion
    }
}
