using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// An entity that displays formatted text.
    /// </summary>
    public class FormattedTextLabel : GameEntityBase, IFormattedTextLabel
    {
        #region Private Members
        private ISpriteBatch _spriteBatch;
        private ITextObjectBuilder _builder;
        private ITextObjectRenderer _renderer;
        private string _textFormat;

        private ITexture2D _texture;
        #endregion

        #region IFormattedText Implementation
        /// <summary>
        /// The formatted string that defines this label's text.
        /// </summary>
        public string TextFormat
        {
            get { return _textFormat; }
            set
            {
                if (value != _textFormat)
                {
                    _textFormat = value;
                    _renderer.Clear();
                    UpdateTexture();
                }
            }
        }
        #endregion

        /// <summary>
        /// Create a new instance of this object.
        /// </summary>
        /// <param name="host">The IEngine object that this entity belongs to.</param>
        /// <param name="bounds">The bounds in the gameworld of this entity.</param>
        /// <param name="spriteBatch">The ISpriteBatch object that will be used to render the label.</param>
        /// <param name="builder">An object responsible for building ITextObject objects.</param>
        /// <param name="renderer">An object responsible for rendering ITextObject objects to an ITexture object.</param>
        /// <param name="textFormat">The format string representing the text to show in this label.</param>
        public FormattedTextLabel(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITextObjectBuilder builder, ITextObjectRenderer renderer, string textFormat)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _builder = builder ?? throw new ArgumentNullException();
            _renderer = renderer ?? throw new ArgumentNullException();
            _textFormat = (textFormat != null) ? textFormat : "";

            UpdateTexture();
        }

        #region Private Methods
        private void UpdateTexture()
        {
            foreach (ITextObject textObject in _builder.Build(_textFormat, new RectangleF(Vector2.Zero, this.Bounds.Size)))
                _renderer.Enqueue(textObject);

            _renderer.Flush();
            _texture = _renderer.Render();
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Occurs when Draw is called.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            _spriteBatch.Draw(_texture, this.Bounds.Location, Color.White);
        }
        #endregion
    }
}
