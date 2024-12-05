using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.ContentManagement;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.Audio
{
    /// <summary>
    /// Represents an object responsible for controlling sounds.
    /// </summary>
    public interface ISoundController
    {
        /// <summary>
        /// An event that occurs when this sound control's enabled state changes.
        /// </summary>
        event EventHandler<StateChangedEventArgs<bool>> EnabledChanged;

        /// <summary>
        /// Whether or not sounds created by this controller should play audibly.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// The volume for this controller and all resources it has created.
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Create a new resource to be controlled by this controller.
        /// </summary>
        /// <param name="content">An object responsible for managing content.</param>
        /// <param name="assetName">The name of the sound resource asset to create.</param>
        /// <returns>The newly created resource.</returns>
        ISoundResource CreateSoundResource(IContentManager content, string assetName);

        /// <summary>
        /// Stop all sounds controlled by this controller.
        /// </summary>
        void StopAll();

        /// <summary>
        /// Dispose of any resources this controller has created.
        /// </summary>
        void ClearOwnedResources();

        /// <summary>
        /// Update this controller.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        void Update(GameTime gameTime);
    }
}
