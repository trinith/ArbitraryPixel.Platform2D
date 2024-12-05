using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// Represents an object that describes a piece of text.
    /// </summary>
    public interface ITextObject
    {
        ///// <summary>
        ///// The colour of this text object.
        ///// </summary>
        //Color Colour { get; }

        ///// <summary>
        ///// The font for this text object.
        ///// </summary>
        //ISpriteFont Font { get; }

        ///// <summary>
        ///// The full text for this object.
        ///// </summary>
        //string Text { get; }

        /// <summary>
        /// The text definition for this text object.
        /// </summary>
        ITextDefinition TextDefinition { get; }

        /// <summary>
        /// The location for this text object.
        /// </summary>
        Vector2 Location { get; set; }

        /// <summary>
        /// The current substring of text to show for this object, as described by ShowLength.
        /// </summary>
        string CurrentText { get; }

        /// <summary>
        /// The amount of time between each character when animating this text object.
        /// </summary>
        double TimePerCharacter { get; set; }

        /// <summary>
        /// Whether or not this text object is considered complete, which is when ShowLength reaches the full text length or of TimePerCharacter is less than, or equal to, zero.
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// The length of text to show for this text object.
        /// </summary>
        int ShowLength { get; set; }
    }
}
