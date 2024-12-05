using ArbitraryPixel.Common;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// Represents an object that will process a text format string.
    /// </summary>
    public interface ITextFormatProcessor
    {
        /// <summary>
        /// A character representing the start of a format sequence.
        /// </summary>
        char FormatOpen { get; }

        /// <summary>
        /// A character representing the end of a format sequence.
        /// </summary>
        char FormatClose { get; }

        /// <summary>
        /// A character used to separate items in a format.
        /// </summary>
        char FormatSeparator { get; }

        /// <summary>
        /// A character used to escape the next character in a string.
        /// </summary>
        char FormatEscape { get; }

        /// <summary>
        /// An event that occurs when this object processes a Colour format value.
        /// </summary>
        event EventHandler<ValueEventArgs<Color>> ColourFormatSet;

        /// <summary>
        /// An event that occurs when this object processes a TimePerCharacter format value.
        /// </summary>
        event EventHandler<ValueEventArgs<double>> TimePerCharacterSet;

        /// <summary>
        /// An event that occurs when this object processes a FontName format value.
        /// </summary>
        event EventHandler<ValueEventArgs<string>> FontNameSet;

        /// <summary>
        /// An event that occurs when this object processes a LineAlignment format value.
        /// </summary>
        event EventHandler<ValueEventArgs<TextLineAlignment>> LineAlignmentSet;

        /// <summary>
        /// Process a format specifier and take the appropriate action.
        /// </summary>
        /// <param name="formatSpecifier">The format specifier to process.</param>
        void Process(string formatSpecifier);
    }
}
