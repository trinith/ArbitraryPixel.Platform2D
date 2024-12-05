using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Represents an object that defines an object for a button.
    /// </summary>
    public interface IButtonObjectDefinition
    {
        /// <summary>
        /// An offset to apply to the object, relative to the parent button's centre.
        /// </summary>
        Vector2 GlobalOffset { get; set; }

        /// <summary>
        /// Draw the object that this definition represents.
        /// </summary>
        /// <param name="host">The button this definition is being drawn for.</param>
        /// <param name="spriteBatch">A sprite batch object that can be used to draw this definition.</param>
        void Draw(IButton host, ISpriteBatch spriteBatch);
    }
}
