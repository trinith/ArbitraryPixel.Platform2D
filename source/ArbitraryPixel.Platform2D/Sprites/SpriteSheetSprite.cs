using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryPixel.Platform2D.Sprites
{
    // TODO: Move me
    public class IndexNotInSpriteSheetBoundsException : Exception
    {
        public IndexNotInSpriteSheetBoundsException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Implements ISpriteSheetSprite to provide retrieval of an individual sprite within a sprite sheet texture.
    /// </summary>
    public class SpriteSheetSprite : Sprite, ISpriteSheetSprite
    {
        private int _index = 0;
        private int _spritesPerRow = 0;

        public SpriteSheetSprite(ITexture2D texture, int index, Point spriteSize) : base(texture, null)
        {
            this.Index = index;
            this.SpriteSize = spriteSize;

            _spritesPerRow = (int)Math.Floor(texture.Width / (double)spriteSize.X);
        }

        public Point SpriteSize { get; private set; }

        public int Index
        {
            get { return _index; }
            set
            {
                int newIndex = value;

                Point p = new Point(
                    newIndex % _spritesPerRow * this.SpriteSize.X,
                    (int)Math.Floor(newIndex / (double)_spritesPerRow) * this.SpriteSize.Y
                );

                Rectangle spriteRect = new Rectangle(p.X * this.SpriteSize.X, p.Y * this.SpriteSize.Y, this.SpriteSize.X, this.SpriteSize.Y);
                Rectangle textureBounds = new Rectangle(0, 0, this.Texture.Width, this.Texture.Height);

                if (!textureBounds.Contains(spriteRect))
                    throw new IndexNotInSpriteSheetBoundsException($"An index of {newIndex} for {this.SpriteSize} sprites within a {textureBounds.Size} sprite sheet is not valid.");

                _index = newIndex;
                this.SourceRectangle = spriteRect;
            }
        }
    }
}
