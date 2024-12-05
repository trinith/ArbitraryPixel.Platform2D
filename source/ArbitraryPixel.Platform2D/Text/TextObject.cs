using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// An implementation of ITextObject.
    /// </summary>
    public class TextObject : ITextObject
    {
        private int _showLength = 0;

        #region ITextObject Implementation
        /// <summary>
        /// The text definition for this text object.
        /// </summary>
        public ITextDefinition TextDefinition { get; }

        /// <summary>
        /// The location for this text object.
        /// </summary>
        public Vector2 Location { get; set; }

        /// <summary>
        /// The current substring of text to show for this object, as described by ShowLength.
        /// </summary>
        public string CurrentText
        {
            get { return this.TextDefinition.Text.Substring(0, _showLength); }
        }

        /// <summary>
        /// The alignment for this text object.
        /// </summary>
        public TextLineAlignment Alignment { get; }

        /// <summary>
        /// The amount of time between each character when animating this text object.
        /// </summary>
        public double TimePerCharacter { get; set; }

        /// <summary>
        /// Whether or not this text object is considered complete, which is when ShowLength reaches the full text length or of TimePerCharacter is less than, or equal to, zero.
        /// </summary>
        public bool IsComplete
        {
            get { return (_showLength == this.TextDefinition.Text.Length || this.TimePerCharacter <= 0); }
        }

        /// <summary>
        /// The length of text to show for this text object.
        /// </summary>
        public int ShowLength
        {
            get { return _showLength; }
            set
            {
                _showLength = MathHelper.Clamp(value, 0, this.TextDefinition.Text.Length);
            }
        }
        #endregion

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="font">The font for this text object.</param>
        /// <param name="text">The full text for this text object.</param>
        /// <param name="location">The location for this text object.</param>
        /// <param name="colour">The colour for this text object.</param>
        /// <param name="timePerCharacter">The time to wait between characters when animating this text object.</param>
        public TextObject(ISpriteFont font, string text, Vector2 location, Color colour, double timePerCharacter = 0)
        {
            this.TextDefinition = new TextDefinition(
                font ?? throw new ArgumentNullException(),
                text,
                colour,
                true // Ensure our text definition is read only
            );

            this.Location = location;
            this.TimePerCharacter = timePerCharacter;

            if (timePerCharacter == 0)
                _showLength = this.TextDefinition.Text.Length;
        }
    }
}
