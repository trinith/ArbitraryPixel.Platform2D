using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Entity
{
    /// <summary>
    /// Represents an entity that can draw itself.
    /// </summary>
    public interface IDrawableEntity : IDrawable
    {
        /// <summary>
        /// Preform any rendering that should be done before the core rendering pass. This typically includes things like rendering to different targets.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        void PreDraw(GameTime gameTime);

        /// <summary>
        /// Whether or not the entity should be drawn.
        /// </summary>
        new bool Visible { get; set; }
    }
}
