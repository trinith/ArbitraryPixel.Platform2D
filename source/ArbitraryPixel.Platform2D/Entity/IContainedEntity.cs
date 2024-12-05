namespace ArbitraryPixel.Platform2D.Entity
{
    /// <summary>
    /// Represents an entity that exists inside a container
    /// </summary>
    public interface IContainedEntity
    {
        /// <summary>
        /// The container that this entity belongs to.
        /// </summary>
        IEntityContainer OwningContainer { get; set;  }
    }
}
