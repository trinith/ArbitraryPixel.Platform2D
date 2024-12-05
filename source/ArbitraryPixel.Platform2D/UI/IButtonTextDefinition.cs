using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Represents an object that defintes text for a button.
    /// </summary>
    public interface IButtonTextDefinition : IButtonObjectDefinition
    {
        /// <summary>
        /// The alignment for this text definition.
        /// </summary>
        TextLineAlignment Alignment { get; set; }

        /// <summary>
        /// A definition for text when the button state is normal.
        /// </summary>
        ITextDefinition TextNormal { get; set; }

        /// <summary>
        /// A definition for text when the button state is pressed.
        /// </summary>
        ITextDefinition TextPressed { get; set; }

        /// <summary>
        /// Get the text for the supplied state.
        /// </summary>
        /// <param name="state">The state to get the text for.</param>
        /// <returns>The text for the supplied state.</returns>
        string GetText(ButtonState state);

        /// <summary>
        /// Get the font for the supplied state.
        /// </summary>
        /// <param name="state">The state to get the font for.</param>
        /// <returns>The font for the supplied state.</returns>
        ISpriteFont GetFont(ButtonState state);

        /// <summary>
        /// Get the colour for the supplied state.
        /// </summary>
        /// <param name="state">The state to get the colour for.</param>
        /// <returns>The colour for the supplied state.</returns>
        Color GetColour(ButtonState state);
    }
}
