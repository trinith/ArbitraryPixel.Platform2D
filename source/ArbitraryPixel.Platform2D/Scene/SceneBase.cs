using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Scene
{
    /// <summary>
    /// An implementation of IScene that provides base functionality to be extended in a game using the Platform2D engine.
    /// </summary>
    public class SceneBase : EntityContainerBase, IScene
    {
        private bool _sceneReset = false;
        private IAssetLoader _assetLoader;

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="host">The IEngine object this scene belongs to.</param>
        public SceneBase(IEngine host)
            : base(host)
        {
            _assetLoader = this.Host.AssetBank.CreateLoader();
            _assetLoader.RegisterLoadMethod(x => OnLoadAssetBank(this.Host.Content, x));
        }

        #region IScene Implementation
        /// <summary>
        /// The next scene that should load when this scene is complete.
        /// </summary>
        public IScene NextScene { get; set; } = null;

        /// <summary>
        /// Whether or not the scene is complete.
        /// </summary>
        public bool SceneComplete { get; set; } = false;

        /// <summary>
        /// Finish this scene and set the next scene to the specified parameter.
        /// </summary>
        /// <param name="nextScene">The next scene to change to.</param>
        public void ChangeScene(IScene nextScene)
        {
            this.NextScene = nextScene;
            this.SceneComplete = true;
        }

        /// <summary>
        /// Initialize this scene.
        /// </summary>
        public void Initialize()
        {
            _assetLoader.LoadBank();
            OnInitialize();
        }

        /// <summary>
        /// Reset the scene, triggering any actions that need to be done to reset the scene to an initial state.
        /// </summary>
        public void Reset()
        {
            if (_sceneReset == false)
            {
                OnReset();
                _sceneReset = true;
            }
        }

        /// <summary>
        /// Start the scene, triggering any actions that need to be done before the scene begins to update and draw.
        /// </summary>
        public void Start()
        {
            OnStarting();
        }

        /// <summary>
        /// End the scene, triggering any actions that need to be done before leaving this scene and changing to a new one.
        /// </summary>
        public void End()
        {
            OnEnding();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Occurs when Initialize is called.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Occurs when Reset is called.
        /// </summary>
        protected virtual void OnReset() { }

        /// <summary>
        /// Occurs when Start is called.
        /// </summary>
        protected virtual void OnStarting() { }

        /// <summary>
        /// Occurs when End is called.
        /// </summary>
        protected virtual void OnEnding() { }

        /// <summary>
        /// Occurs at the start of Initialize, when assets for a given bank have not yet been loaded.
        /// </summary>
        /// <param name="content">The content manager object to load assets into the asset bank.</param>
        /// <param name="bank">The asset bank to load assets into.</param>
        protected virtual void OnLoadAssetBank(IContentManager content, IAssetBank bank) { }

        /// <summary>
        /// Occurs when Update is called.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            _sceneReset = false;
        }
        #endregion
    }
}
