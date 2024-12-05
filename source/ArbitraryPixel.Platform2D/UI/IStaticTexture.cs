using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Represents a static texture entity.
    /// </summary>
    public interface IStaticTexture : IGameEntity
    {
        /// <summary>
        /// The texture to render.
        /// </summary>
        ITexture2D Texture { get; set; }

        /// <summary>
        /// A rectangle representing the portion of the Texture to draw. If set to null, the full image will be drawn.
        /// </summary>
        Rectangle? SourceRectangle { get; set; }

        /// <summary>
        /// The mask to apply to Texture when rendering.
        /// </summary>
        Color Mask { get; set; }

        /// <summary>
        /// The rotation.
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// The origin of drawing, with respect to the top left.
        /// </summary>
        Vector2 Origin { get; set; }

        /// <summary>
        /// Effects to apply to drawing.
        /// </summary>
        SpriteEffects SpriteEffects { get; set; }
    }
}
