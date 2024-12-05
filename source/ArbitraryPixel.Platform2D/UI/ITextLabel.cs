using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Represents an object that draws text on the screen.
    /// </summary>
    public interface ITextLabel : IGameEntity
    {
        /// <summary>
        /// This colour to render the text as.
        /// </summary>
        Color Colour { get; set; }

        /// <summary>
        /// The spritebatch object used to render to the screen.
        /// </summary>
        ISpriteBatch SpriteBatch { get; }

        /// <summary>
        /// The font to use.
        /// </summary>
        ISpriteFont Font { get; set; }

        /// <summary>
        /// The text to display.
        /// </summary>
        string Text { get; set; }
    }
}