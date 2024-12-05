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
    /// An implementation of IEngine, providing base functionality.
    /// </summary>
    public class EngineBase : IEngine
    {
        /// <summary>
        /// Create a new instance of this object.
        /// </summary>
        /// <param name="componentContainer">An object responsible for acting as a container for components.</param>
        public EngineBase(IComponentContainer componentContainer)
        {
            _componentContainer = componentContainer ?? throw new ArgumentNullException();

            this.Graphics = this.GetComponent<IGrfxDeviceManager>() ?? throw new RequiredComponentMissingException<IGrfxDeviceManager>();
            this.Content = this.GetComponent<IContentManager>() ?? throw new RequiredComponentMissingException<IContentManager>();
            this.ScreenManager = this.GetComponent<IScreenManager>() ?? throw new RequiredComponentMissingException<IScreenManager>();
            this.InputManager = this.GetComponent<ISurfaceInputManager>() ?? throw new RequiredComponentMissingException<ISurfaceInputManager>();
            this.GrfxFactory = this.GetComponent<IGrfxFactory>() ?? throw new RequiredComponentMissingException<IGrfxFactory>();
            this.AssetBank = this.GetComponent<IAssetBank>() ?? throw new RequiredComponentMissingException<IAssetBank>();
            this.AudioManager = this.GetComponent<IAudioManager>() ?? throw new RequiredComponentMissingException<IAudioManager>();
        }

        #region IEngine Implementation
        #region IComponentContainer Implementation
        private IComponentContainer _componentContainer;

        /// <summary>
        /// Register a component with the container.
        /// </summary>
        /// <typeparam name="TComponent">The component type to register.</typeparam>
        /// <param name="component">The component to register.</param>
        public void RegisterComponent<TComponent>(TComponent component) where TComponent : class
        {
            _componentContainer.RegisterComponent<TComponent>(component);
        }


        /// <summary>
        /// Get a component from the container. If the container does not have a component for the type specified, null is returned.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to get.</typeparam>
        /// <returns>The component of the specfied type if it exists, null otherwise.</returns>
        public TComponent GetComponent<TComponent>() where TComponent : class
        {
            return _componentContainer.GetComponent<TComponent>();
        }

        /// <summary>
        /// Get a component from the container. If the container does not have a component for the type specified, null is returned.
        /// </summary>
        /// <param name="componentType">The type of component to get.</param>
        /// <returns>The component of the specfied type if it exists, null otherwise.</returns>
        public object GetComponent(Type componentType)
        {
            return _componentContainer.GetComponent(componentType);
        }

        /// <summary>
        /// Check whether or not this container has the specified component type.
        /// </summary>
        /// <param name="componentType">The type of component to check for.</param>
        /// <returns>True if a component of the specified type exists in this container, false otherwise.</returns>
        public bool ContainsComponent(Type componentType)
        {
            return _componentContainer.ContainsComponent(componentType);
        }

        /// <summary>
        /// Check whether or not this container has the specified component type.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to check for.</typeparam>
        /// <returns>True if a component of the specified type exists in this container, false otherwise.</returns>
        public bool ContainsComponent<TComponent>() where TComponent : class
        {
            return _componentContainer.ContainsComponent<TComponent>();
        }
        #endregion

        #region Component Properties
        /// <summary>
        /// An object responsible for managing the screen that the engine will draw in.
        /// </summary>
        public IScreenManager ScreenManager { get; private set; }

        /// <summary>
        /// An object responsible for managing surface input for this engine.
        /// </summary>
        public ISurfaceInputManager InputManager { get; private set; }

        /// <summary>
        /// An object responsible for managing the engine's graphics device.
        /// </summary>
        public IGrfxDeviceManager Graphics { get; private set; }

        /// <summary>
        /// An object responsible for managing content.
        /// </summary>
        public IContentManager Content { get; private set; }

        /// <summary>
        /// An object responsible for storing assets.
        /// </summary>
        public IAssetBank AssetBank { get; private set; }


        /// <summary>
        /// An object responsible for creating graphics objects.
        /// </summary>
        public IGrfxFactory GrfxFactory { get; private set; }

        /// <summary>
        /// An object responsible for managing audio.
        /// </summary>
        public IAudioManager AudioManager { get; private set; }
        #endregion

        /// <summary>
        /// Whether or not the engine is finished and the game can close.
        /// </summary>
        public bool Finished { get; set; } = false;

        /// <summary>
        /// The scene this engine is currently running.
        /// </summary>
        public IScene CurrentScene { get; protected set; } = null;

        /// <summary>
        /// The scenes avaialble to this engine.
        /// </summary>
        public Dictionary<string, IScene> Scenes { get; } = new Dictionary<string, IScene>();

        /// <summary>
        /// An event that occurs when some external action occurs.
        /// </summary>
        public event EventHandler<ExternalActionEventArgs> ExternalActionOccurred;

        /// <summary>
        /// Load any content that this engine will utilize.
        /// </summary>
        public void LoadContent()
        {
            OnLoadContent();
        }

        /// <summary>
        /// Initialize any objects this engine needs.
        /// </summary>
        public void Initialize()
        {
            this.ScreenManager.ApplySettings(this.Graphics);

            OnInitialize();
        }

        /// <summary>
        /// Tell this engine it should exit.
        /// </summary>
        public void Exit()
        {
            this.Finished = true;

            OnExit();
        }

        /// <summary>
        /// Suspend the engine, taking any action that should be taken when suspend occurs.
        /// </summary>
        public void Suspend()
        {
            this.AudioManager.MusicController.Suspend();

            OnSuspend();
        }

        /// <summary>
        /// Resume the engine, taking any action that should be taken when resume occurs.
        /// </summary>
        public void Resume()
        {
            this.AudioManager.MusicController.Resume();

            OnResume();
        }

        /// <summary>
        /// Cause an ExternalActionOccurred event to fire with the supplied data, allowing listeners to handle the event.
        /// </summary>
        /// <param name="data">An object containing data associated with the event that listeners can utilize.</param>
        public void TriggerExternalAction(object data)
        {
            ExternalActionEventArgs e = new ExternalActionEventArgs(data);

            OnTriggerExternalAction(e);

            if (this.ExternalActionOccurred != null)
                this.ExternalActionOccurred(this, e);
        }

        /// <summary>
        /// Perform updates for this engine.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        public void Update(GameTime gameTime)
        {
            this.InputManager.Update(gameTime);
            this.AudioManager.Update(gameTime);

            if (this.CurrentScene != null)
            {
                this.CurrentScene.Update(gameTime);

                if (this.CurrentScene.SceneComplete)
                {
                    if (this.CurrentScene.NextScene != null)
                    {
                        this.InputManager.ClearConsumer();

                        while (this.CurrentScene.NextScene != null && this.CurrentScene.SceneComplete == true)
                        {
                            this.CurrentScene.End();
                            this.CurrentScene = this.CurrentScene.NextScene;
                            this.CurrentScene.Reset();
                            this.CurrentScene.Start();
                        }
                    }
                    else
                    {
                        this.CurrentScene.End();
                        this.Exit();
                    }
                }
            }

            OnUpdate(gameTime);
        }

        /// <summary>
        /// Perform draws for this engine.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        public void Draw(GameTime gameTime)
        {
            this.CurrentScene?.PreDraw(gameTime);

            this.ScreenManager.BeginDraw(this.Graphics.GraphicsDevice);
            this.CurrentScene?.Draw(gameTime);

            OnDraw(gameTime);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Called in response to LoadContent.
        /// </summary>
        protected virtual void OnLoadContent() { }

        /// <summary>
        /// Called in response to Initialize, after this engine's ScreenManager has had its settings applied.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called in response to Exit.
        /// </summary>
        protected virtual void OnExit() { }

        /// <summary>
        /// Called in response to Suspend.
        /// </summary>
        protected virtual void OnSuspend() { }

        /// <summary>
        /// Called in response to Resume.
        /// </summary>
        protected virtual void OnResume() { }

        /// <summary>
        /// Called in response to TriggerExternalAction, before the event is fired.
        /// </summary>
        /// <param name="e">An event argument object, containing the data for the external action.</param>
        protected virtual void OnTriggerExternalAction(ExternalActionEventArgs e) { }

        /// <summary>
        /// Called in response to Update, after this engine has managed its built-in components and scenes.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected virtual void OnUpdate(GameTime gameTime) { }

        /// <summary>
        /// Called in response to Draw, afer this engine has drawn its current scene.
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void OnDraw(GameTime gameTime) { }
        #endregion
    }
}
