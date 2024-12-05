using ArbitraryPixel.Platform2D.Entity;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Represents an object that draws formatted text.
    /// </summary>
    public interface IFormattedTextLabel : IGameEntity
    {
        /// <summary>
        /// The formatted string that defines this label's text.
        /// </summary>
        string TextFormat { get; set; }
    }
}
