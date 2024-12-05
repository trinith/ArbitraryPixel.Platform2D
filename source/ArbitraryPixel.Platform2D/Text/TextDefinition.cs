using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// Defines basic properties for text.
    /// </summary>
    public class TextDefinition : ITextDefinition
    {
        private string _text = "";
        private ISpriteFont _font = null;
        private Color _colour = Color.White;

        /// <summary>
        /// Whether or not this text definition is set to read only.
        /// </summary>
        public bool IsReadOnly { get; } = false;

        /// <summary>
        /// The text string for this text.
        /// <para>NOTE: If this property is set while IsReadOnly is true, an exception will be thrown.</para>
        /// </summary>
        /// <exception cref="PropertyIsReadOnlyException" />
        public string Text
        {
            get { return _text; }
            set { _text = (this.IsReadOnly) ? throw new PropertyIsReadOnlyException() : value; }
        }

        /// <summary>
        /// The font for this text.
        /// <para>NOTE: If this property is set while IsReadOnly is true, an exception will be thrown.</para>
        /// </summary>
        /// <exception cref="PropertyIsReadOnlyException" />
        public ISpriteFont Font
        {
            get { return _font; }
            set { _font = (this.IsReadOnly) ? throw new PropertyIsReadOnlyException() : value; }
        }

        /// <summary>
        /// The colour for this text.
        /// <para>NOTE: If this property is set while IsReadOnly is true, an exception will be thrown.</para>
        /// </summary>
        /// <exception cref="PropertyIsReadOnlyException" />
        public Color Colour
        {
            get { return _colour; }
            set { _colour = (this.IsReadOnly) ? throw new PropertyIsReadOnlyException() : value; }
        }

        /// <summary>
        /// Create a new instance with default values.
        /// </summary>
        public TextDefinition()
        {
        }

        /// <summary>
        /// Create a new instance, copying values from another instance.
        /// </summary>
        /// <param name="other">An ITextDefinition object to copy data from.</param>
        /// <param name="forceWriteable">If true, the newly created object will be writeable, regardless of the value of other's IsReadOnly property.</param>
        public TextDefinition(ITextDefinition other, bool forceWriteable = false)
        {
            if (other == null)
                throw new ArgumentNullException();

            this.IsReadOnly = (forceWriteable) ? false : other.IsReadOnly;
            _font = other.Font;
            _text = other.Text;
            _colour = other.Colour;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="font">The font for this text definition.</param>
        /// <param name="text">The text for this text definition.</param>
        /// <param name="colour">The colour for this text definition.</param>
        /// <param name="isReadOnly">If true, setting parameters will throw an exception.</param>
        public TextDefinition(ISpriteFont font, string text, Color colour, bool isReadOnly = false)
        {
            this.IsReadOnly = isReadOnly;

            // Use fields to set.
            _font = font;
            _text = text;
            _colour = colour;
        }
    }
}
