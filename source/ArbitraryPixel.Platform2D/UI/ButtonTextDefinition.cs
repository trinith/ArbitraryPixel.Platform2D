using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// An object responsible for defining text for a button.
    /// </summary>
    public class ButtonTextDefinition : IButtonTextDefinition
    {
        private ITextDefinition _textNormal = new TextDefinition();
        private ITextDefinition _textPressed = new TextDefinition();

        /// <summary>
        /// The alignment for this text definition.
        /// </summary>
        public TextLineAlignment Alignment { get; set; } = TextLineAlignment.Centre;

        /// <summary>
        /// A definition for text when the button state is normal.
        /// </summary>
        public ITextDefinition TextNormal
        {
            get { return _textNormal; }
            set { _textNormal = value ?? throw new ArgumentNullException(); }
        }

        /// <summary>
        /// A definition for text when the button state is pressed.
        /// </summary>
        public ITextDefinition TextPressed
        {
            get { return _textPressed; }
            set { _textPressed = value ?? throw new ArgumentNullException(); }
        }

        /// <summary>
        /// An offset to apply to the object, relative to the parent button's centre.
        /// </summary>
        public Vector2 GlobalOffset { get; set; } = Vector2.Zero;

        /// <summary>
        /// Get the text for the supplied state.
        /// </summary>
        /// <param name="state">The state to get the text for.</param>
        /// <returns>The text for the supplied state.</returns>
        public string GetText(ButtonState state)
        {
            return (state == ButtonState.Pressed) ? this.TextPressed.Text : this.TextNormal.Text;
        }

        /// <summary>
        /// Get the font for the supplied state.
        /// </summary>
        /// <param name="state">The state to get the font for.</param>
        /// <returns>The font for the supplied state.</returns>
        public ISpriteFont GetFont(ButtonState state)
        {
            return (state == ButtonState.Pressed) ? this.TextPressed.Font : this.TextNormal.Font;
        }

        /// <summary>
        /// Get the colour for the supplied state.
        /// </summary>
        /// <param name="state">The state to get the colour for.</param>
        /// <returns>The colour for the supplied state.</returns>
        public Color GetColour(ButtonState state)
        {
            return (state == ButtonState.Pressed) ? this.TextPressed.Colour : this.TextNormal.Colour;
        }

        /// <summary>
        /// Draw the object that this definition represents.
        /// </summary>
        /// <param name="host">The button this definition is being drawn for.</param>
        /// <param name="spriteBatch">A sprite batch object that can be used to draw this definition.</param>
        public void Draw(IButton host, ISpriteBatch spriteBatch)
        {
            if (host == null || spriteBatch == null)
                throw new ArgumentNullException();

            ISpriteFont font = this.GetFont(host.State);
            if (font != null)
            {
                string text = this.GetText(host.State);
                SizeF textSize = font.MeasureString(text);

                Vector2 textPos = Vector2.Zero;
                switch (this.Alignment)
                {
                    case TextLineAlignment.Left:
                        textPos = new Vector2(host.Bounds.Left, host.Bounds.Centre.Y - textSize.Centre.Y);
                        break;
                    case TextLineAlignment.Right:
                        textPos = new Vector2(host.Bounds.Right - textSize.Width, host.Bounds.Centre.Y - textSize.Centre.Y);
                        break;
                    case TextLineAlignment.Centre:
                    default:
                        textPos = host.Bounds.Centre - textSize.Centre;
                        break;
                }

                textPos += this.GlobalOffset;

                spriteBatch.DrawString(font, text, textPos, this.GetColour(host.State));
            }
        }
    }
}
