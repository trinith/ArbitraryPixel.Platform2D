using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// An entity that displays text.
    /// </summary>
    public class TextLabel : GameEntityBase, ITextLabel
    {
        private ISpriteFont _font;
        private string _text = "";
        private RectangleF _bounds = RectangleF.Empty;

        /// <summary>
        /// The font to use.
        /// </summary>
        public ISpriteFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
                UpdateSizeForTextProperties();
            }
        }

        /// <summary>
        /// The spritebatch object used to render to the screen.
        /// </summary>
        public ISpriteBatch SpriteBatch { get; private set; } = null;

        /// <summary>
        /// This colour to render the text as.
        /// </summary>
        public Color Colour { get; set; } = Color.White;

        /// <summary>
        /// The text to display.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                UpdateSizeForTextProperties();
            }
        }

        /// <summary>
        /// The game world bounds of this text label. When set, only the location is used and the size is calculated automatically as a function of the current text and font.
        /// </summary>
        public override RectangleF Bounds
        {
            get { return _bounds; }
            set { _bounds.Location = value.Location; }
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="host">The owner of this entity.</param>
        /// <param name="location">The location of the text.</param>
        /// <param name="spriteBatch">The spritebatch used to render to the screen.</param>
        /// <param name="font">The font to use.</param>
        /// <param name="text">The message for this text.</param>
        /// <param name="colour">The colour for this text.</param>
        public TextLabel(IEngine host, Vector2 location, ISpriteBatch spriteBatch, ISpriteFont font, string text, Color colour)
            : base(host, new RectangleF(location, new SizeF(1, 1)))
        {
            this.SpriteBatch = spriteBatch ?? throw new ArgumentNullException();
            this.Font = font ?? throw new ArgumentNullException();
            this.Text = text;
            this.Colour = colour;

            _bounds.Location = location;
        }

        #region Private Methods
        private void UpdateSizeForTextProperties()
        {
            _bounds.Size = _font.MeasureString(_text);
        }
        #endregion

        /// <summary>
        /// Occurs when Draw is called.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            this.SpriteBatch.DrawString(this.Font, this.Text, this.Bounds.Location, this.Colour);
        }
    }
}
