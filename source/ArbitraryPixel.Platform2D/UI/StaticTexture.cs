using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// An object responsible for rendering a texture.
    /// </summary>
    public class StaticTexture : GameEntityBase, IStaticTexture
    {
        private ISpriteBatch _spriteBatch;

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="host">The IEngine object that owns this button.</param>
        /// <param name="bounds">The bounds of the button.</param>
        /// <param name="spriteBatch">The spritebatch used to render to the screen.</param>
        /// <param name="texture">The texture to draw.</param>
        public StaticTexture(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITexture2D texture)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            this.Texture = texture;
        }

        #region IStaticTexture Implementation
        /// <summary>
        /// The texture to render.
        /// </summary>
        public ITexture2D Texture { get; set; } = null;

        /// <summary>
        /// A rectangle representing the portion of the Texture to draw. If set to null, the full image will be drawn.
        /// </summary>
        public Rectangle? SourceRectangle { get; set; } = null;

        /// <summary>
        /// The mask to apply to Texture when rendering.
        /// </summary>
        public Color Mask { get; set; } = Color.White;

        /// <summary>
        /// The rotation.
        /// </summary>
        public float Rotation { get; set; } = 0f;

        /// <summary>
        /// The origin of drawing, with respect to the top left.
        /// </summary>
        public Vector2 Origin { get; set; } = Vector2.Zero;

        /// <summary>
        /// Effects to apply to drawing.
        /// </summary>
        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
        #endregion

        /// <summary>
        /// Occurs when Draw is called.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            if (this.Texture != null)
                _spriteBatch.Draw(this.Texture, this.Bounds, this.SourceRectangle, this.Mask, this.Rotation, this.Origin, this.SpriteEffects, 0);
        }
    }
}
