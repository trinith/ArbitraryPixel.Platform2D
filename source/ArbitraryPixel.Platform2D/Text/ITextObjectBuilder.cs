using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// Represents an object that will build text objects from a formatted string.
    /// </summary>
    public interface ITextObjectBuilder
    {
        /// <summary>
        /// If set to True, states set by a format string passed to a previous Build call will remain intact for future Build calls. If set to False, these states will be reset to defaults.
        /// </summary>
        bool PreserveState { get; set; }

        /// <summary>
        /// The default font that this builder will use to build text objects when not font has been specified in the format string.
        /// </summary>
        ISpriteFont DefaultFont { get; set; }

        /// <summary>
        /// Get the font names registered with this Text Builder.
        /// </summary>
        string[] RegisteredFontNames { get; }

        /// <summary>
        /// Gets a font that has been registered with this builder by name.
        /// </summary>
        /// <param name="name">The name of the font to get.</param>
        /// <returns>The font object, if it has been registered with the supplied name. If not, an exception is thrown.</returns>
        ISpriteFont GetRegisteredFont(string name);

        /// <summary>
        /// Register a font with this builder to make that font available for use in a format string.
        /// </summary>
        /// <param name="fontName">The name of the font.</param>
        /// <param name="font">The font object.</param>
        void RegisterFont(string fontName, ISpriteFont font);

        /// <summary>
        /// Create a list of ITextObjects given the specified format string and bounds.
        /// </summary>
        /// <param name="formatString">The string specifying the text, and format, to create ITextObjects for.</param>
        /// <param name="bounds">The bounds the text will exist in. If the text is too large, it will spill over bounds.</param>
        /// <returns>A list of ITextObject objects representing the formatted string that was given.</returns>
        List<ITextObject> Build(string formatString, RectangleF bounds);
    }
}
