using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// An object responsible for creating ITextObjectRenderer objects.
    /// </summary>
    public interface ITextObjectRendererFactory
    {
        /// <summary>
        /// Create an ITextObjectRenderer object.
        /// </summary>
        /// <param name="renderTargetFactory">An object responsible for creating IRenderTarget2D objects.</param>
        /// <param name="device">An object responsible for rendering to the screen.</param>
        /// <param name="spriteBatch">An object responsible for drawing textures.</param>
        /// <param name="bounds">Defines the bounds text objects should be rendered within. Should be located at (0, 0).</param>
        ITextObjectRenderer Create(IRenderTargetFactory renderTargetFactory, IGrfxDevice device, ISpriteBatch spriteBatch, Rectangle bounds);
    }
}
