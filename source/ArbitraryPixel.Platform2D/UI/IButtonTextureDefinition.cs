using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Represents an object that defines a texture for a generic button.
    /// </summary>
    public interface IButtonTextureDefinition : IButtonObjectDefinition
    {
        /// <summary>
        /// An image to use when not pressed.
        /// </summary>
        ITexture2D ImageNormal { get; set; }

        /// <summary>
        /// An image to use when pressed.
        /// </summary>
        ITexture2D ImagePressed { get; set; }

        /// <summary>
        /// An image mask to use when not pressed.
        /// </summary>
        Color MaskNormal { get; set; }

        /// <summary>
        /// An image mask to use when pressed.
        /// </summary>
        Color MaskPressed { get; set; }

        /// <summary>
        /// The sprite effects to use when rendering the image.
        /// </summary>
        SpriteEffects SpriteEffects { get; set; }

        /// <summary>
        /// Get the mask for the supplied state.
        /// </summary>
        /// <param name="state">The state to get the mask for.</param>
        /// <returns>The mask for the supplied state.</returns>
        Color GetMask(ButtonState state);

        /// <summary>
        /// Get the image for the supplied state.
        /// </summary>
        /// <param name="state">The state to get the image for.</param>
        /// <returns>The image for the supplied state.</returns>
        ITexture2D GetTexture(ButtonState state);
    }
}