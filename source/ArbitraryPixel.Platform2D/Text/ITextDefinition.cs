using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// Represents an object that defines text.
    /// </summary>
    public interface ITextDefinition : IReadOnly
    {
        /// <summary>
        /// The colour for this text.
        /// <para>NOTE: If this property is set while IsReadOnly is true, an exception will be thrown.</para>
        /// </summary>
        /// <exception cref="PropertyIsReadOnlyException" />
        Color Colour { get; set; }

        /// <summary>
        /// The font for this text.
        /// <para>NOTE: If this property is set while IsReadOnly is true, an exception will be thrown.</para>
        /// </summary>
        /// <exception cref="PropertyIsReadOnlyException" />
        ISpriteFont Font { get; set; }

        /// <summary>
        /// The text string for this text.
        /// <para>NOTE: If this property is set while IsReadOnly is true, an exception will be thrown.</para>
        /// </summary>
        /// <exception cref="PropertyIsReadOnlyException" />
        string Text { get; set; }
    }
}