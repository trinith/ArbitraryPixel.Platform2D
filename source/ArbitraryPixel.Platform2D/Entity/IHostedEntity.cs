using ArbitraryPixel.Platform2D.Engine;

namespace ArbitraryPixel.Platform2D.Entity
{
    /// <summary>
    /// Represents an entity that is hosted by an IEngine object.
    /// </summary>
    public interface IHostedEntity
    {
        /// <summary>
        /// The IEngine object that hosts this entity.
        /// </summary>
        IEngine Host { get; }
    }
}
