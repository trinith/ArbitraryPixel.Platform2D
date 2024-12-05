using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Common.Screen;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Audio;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Engine
{
    /// <summary>
    /// Represents a main engine for a Platform2D game.
    /// </summary>
    public interface IEngine : IComponentContainer
    {
        /// <summary>
        /// An event that occurs when some external action occurs.
        /// </summary>
        event EventHandler<ExternalActionEventArgs> ExternalActionOccurred;

        /// <summary>
        /// Whether or not the engine is finished and the game can close.
        /// </summary>
        bool Finished { get; set; }

        /// <summary>
        /// The scene this engine is currently running.
        /// </summary>
        IScene CurrentScene { get; }

        #region Component Properties
        /// <summary>
        /// An object responsible for managing the screen that the engine will draw in.
        /// </summary>
        IScreenManager ScreenManager { get; }

        /// <summary>
        /// An object responsible for managing surface input for this engine.
        /// </summary>
        ISurfaceInputManager InputManager { get; }

        /// <summary>
        /// An object responsible for managing the engine's graphics device.
        /// </summary>
        IGrfxDeviceManager Graphics { get; }

        /// <summary>
        /// An object responsible for managing content.
        /// </summary>
        IContentManager Content { get; }

        /// <summary>
        /// An object responsible for storing assets.
        /// </summary>
        IAssetBank AssetBank { get; }

        /// <summary>
        /// An object responsible for creating graphics objects.
        /// </summary>
        IGrfxFactory GrfxFactory { get; }

        /// <summary>
        /// An object responsible for managing audio.
        /// </summary>
        IAudioManager AudioManager { get; }
        #endregion

        /// <summary>
        /// The scenes avaialble to this engine.
        /// </summary>
        Dictionary<string, IScene> Scenes { get; }

        /// <summary>
        /// Initialize any objects this engine needs.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Load any content that this engine will utilize.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Perform updates for this engine.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Perform draws for this engine.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        void Draw(GameTime gameTime);

        /// <summary>
        /// Tell this engine it should exit.
        /// </summary>
        void Exit();

        /// <summary>
        /// Suspend the engine, taking any action that should be taken when suspend occurs.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Resume the engine, taking any action that should be taken when resume occurs.
        /// </summary>
        void Resume();

        /// <summary>
        /// Cause an ExternalActionOccurred event to fire with the supplied data, allowing listeners to handle the event.
        /// </summary>
        /// <param name="data">An object containing data associated with the event that listeners can utilize.</param>
        void TriggerExternalAction(object data);
    }
}
