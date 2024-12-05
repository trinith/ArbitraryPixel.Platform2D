using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Entity
{
    /// <summary>
    /// Represents an object that contains a list of entities.
    /// </summary>
    public interface IEntityContainer
    {
        /// <summary>
        /// A list of IEntity objects owned by the container.
        /// </summary>
        IReadOnlyList<IEntity> Entities { get; }

        /// <summary>
        /// Add an entity to the container.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void AddEntity(IEntity entity);

        /// <summary>
        /// Remove an entity from the container.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>True if the entity was successfull removed, false otherwise or if the entity did not exist in this container.</returns>
        bool RemoveEntity(IEntity entity);

        /// <summary>
        /// Remove all entities from this container. If this is called during an update, the update cycle will be aborted and the entities will be removed at the end.
        /// </summary>
        void ClearEntities();
    }
}
