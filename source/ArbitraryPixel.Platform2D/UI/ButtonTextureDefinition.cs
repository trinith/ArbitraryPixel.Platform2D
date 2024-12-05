using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// An object responsible for defining a texture for a generic button.
    /// </summary>
    public class ButtonTextureDefinition : IButtonTextureDefinition
    {
        /// <summary>
        /// An image to use when not pressed.
        /// </summary>
        public ITexture2D ImageNormal { get; set; } = null;

        /// <summary>
        /// An image to use when pressed.
        /// </summary>
        public ITexture2D ImagePressed { get; set; } = null;

        /// <summary>
        /// An image mask to use when not pressed.
        /// </summary>
        public Color MaskNormal { get; set; } = Color.White;

        /// <summary>
        /// An image mask to use when pressed.
        /// </summary>
        public Color MaskPressed { get; set; } = Color.White;

        /// <summary>
        /// The sprite effects to use when rendering the image.
        /// </summary>
        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;

        /// <summary>
        /// An offset to apply to the object, relative to the parent button's centre.
        /// </summary>
        public Vector2 GlobalOffset { get; set; } = Vector2.Zero;

        /// <summary>
        /// Get the mask for the supplied state.
        /// </summary>
        /// <param name="state">The state to get the mask for.</param>
        /// <returns>The mask for the supplied state.</returns>
        public ITexture2D GetTexture(ButtonState state)
        {
            return (state == ButtonState.Pressed) ? this.ImagePressed : this.ImageNormal;
        }

        /// <summary>
        /// Get the image for the supplied state.
        /// </summary>
        /// <param name="state">The state to get the image for.</param>
        /// <returns>The image for the supplied state.</returns>
        public Color GetMask(ButtonState state)
        {
            return (state == ButtonState.Pressed) ? this.MaskPressed : this.MaskNormal;
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

            ITexture2D icon = this.GetTexture(host.State);
            if (icon != null)
            {
                SizeF iconSize = new SizeF(icon.Width, icon.Height);
                Vector2 iconLocation = host.Bounds.Centre - iconSize.Centre + this.GlobalOffset;
                RectangleF iconRect = new RectangleF(iconLocation, iconSize);
                spriteBatch.Draw(icon, iconRect, null, this.GetMask(host.State), 0f, Vector2.Zero, this.SpriteEffects, 0f);
            }
        }
    }
}
