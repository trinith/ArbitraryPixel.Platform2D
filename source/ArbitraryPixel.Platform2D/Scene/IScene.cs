using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Scene
{
    /// <summary>
    /// A scene for a Platform2D game.
    /// </summary>
    public interface IScene : IEntity, IUpdateable, IDrawableEntity, IHostedEntity, IEntityContainer
    {
        /// <summary>
        /// The next scene to be loaded once this scene is done.
        /// </summary>
        IScene NextScene { get; set; }

        /// <summary>
        /// Whether or not the scene is complete.
        /// </summary>
        bool SceneComplete { get; set; }

        /// <summary>
        /// Finish this scene and set the next scene to the specified parameter.
        /// </summary>
        /// <param name="nextScene">The next scene to change to.</param>
        void ChangeScene(IScene nextScene);

        /// <summary>
        /// Initialize this scene.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Reset the scene, triggering any actions that need to be done to reset the scene to an initial state.
        /// </summary>
        void Reset();

        /// <summary>
        /// Start the scene, triggering any actions that need to be done before the scene begins to update and draw.
        /// </summary>
        void Start();

        /// <summary>
        /// End the scene, triggering any actions that need to be done before leaving this scene and changing to a new one.
        /// </summary>
        void End();
    }
}
