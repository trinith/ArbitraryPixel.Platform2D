using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// Represents an object responsible for creating ITextObjects.
    /// </summary>
    public interface ITextObjectFactory
    {
        /// <summary>
        /// Create a new text object with the specified parameters.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="text">The text.</param>
        /// <param name="location">The location.</param>
        /// <param name="colour">The colour.</param>
        /// <param name="timePerCharacter">The time per character.</param>
        /// <returns>The newly created object.</returns>
        ITextObject Create(ISpriteFont font, string text, Vector2 location, Color colour, double timePerCharacter);
    }
}
