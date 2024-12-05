using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Entity
{
    /// <summary>
    /// Represents an entity that can update itself.
    /// </summary>
    public interface IUpdateableEntity : IUpdateable
    {
        /// <summary>
        /// Whether or not this entity should be updated.
        /// </summary>
        new bool Enabled { get; set; }
    }
}
