using ArbitraryPixel.Common.Drawing;

namespace ArbitraryPixel.Platform2D.Entity
{
    /// <summary>
    /// Represents an entity in a Platform2D game that exists within coordinate space.
    /// </summary>
    public interface IGameEntity : IEntity
    {
        /// <summary>
        /// The bounds within the world coordinate space that this entity occupies.
        /// </summary>
        RectangleF Bounds { get; set; }
    }
}
