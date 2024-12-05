using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// Represents an object responsible for animating and rendering ITextObjects
    /// </summary>
    public interface ITextObjectRenderer
    {
        /// <summary>
        /// True if this text renderer has rendered all of its text objects, False otherwise.
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// Enqueue a text object to be rendered at the appropraite time.
        /// </summary>
        /// <param name="textObject">The text object to add to this queue.</param>
        void Enqueue(ITextObject textObject);

        /// <summary>
        /// Flush all outstanding text objects to a fully rendered state.
        /// </summary>
        void Flush();

        /// <summary>
        /// Update this renderer.
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Render the current state of the text objects.
        /// </summary>
        /// <returns>A texture containing the rendered state.</returns>
        ITexture2D Render();

        /// <summary>
        /// Clear this renderer of all outstanding text objects and cached image data.
        /// </summary>
        void Clear();
    }
}
