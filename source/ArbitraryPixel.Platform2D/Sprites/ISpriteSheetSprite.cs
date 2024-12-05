using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Sprites
{
    /// <summary>
    /// Represents a sprite that is a part of a larger sprite sheet.
    /// </summary>
    public interface ISpriteSheetSprite : ISprite
    {
        /// <summary>
        /// The dimensions of the sprites within the sprite sheet.
        /// </summary>
        Point SpriteSize { get; }

        /// <summary>
        /// The index of the sprite within the sprite sheet texture, ordered left to right, then top to bottom.
        /// </summary>
        int Index { get; set; }
    }
}
