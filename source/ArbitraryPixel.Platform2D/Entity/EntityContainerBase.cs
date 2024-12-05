using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Entity
{
    /// <summary>
    /// An implementation of IEntityContainer that builds on EntityBase to provide a base implementation.
    /// </summary>
    public class EntityContainerBase : EntityBase, IEntityContainer
    {
        private bool _inCoreUpdate = false;
        private bool _clearAfterUpdate = false;
        private Dictionary<string, IEntity> _deadEntities = new Dictionary<string, IEntity>();
        private List<IEntity> _entities = new List<IEntity>();

        // TODO: At some point this needs to be refactored. A dependency should replace the entities list instead.
        private Dictionary<string, IEntity> _trackedEntities = new Dictionary<string, IEntity>();

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="host">The IEngine object this scene belongs to.</param>
        public EntityContainerBase(IEngine host)
            : base(host)
        {
        }

        #region IContainerEntity Implementation
        /// <summary>
        /// A list of IEntity objects owned by the container.
        /// </summary>
        public IReadOnlyList<IEntity> Entities => _entities;

        /// <summary>
        /// Add an entity to the container.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void AddEntity(IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            if (_trackedEntities.ContainsKey(entity.UniqueId))
            {
                this.Host.GetComponent<ILogger>()?.WriteLine(string.Format("Entity with UniqueId, {0}, already exists in this container. This operation is currently permitted but may cause issues with entity cleanup so this should be avoided. Future versions will address this issue and an exception will be thrown.", entity.UniqueId));
            }
            else
            {
                _trackedEntities.Add(entity.UniqueId, entity);
            }

            _entities.Add(entity);
            entity.OwningContainer = this;
        }

        /// <summary>
        /// Remove an entity from the container.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>True if the entity was successfull removed, false otherwise or if the entity did not exist in this container.</returns>
        public bool RemoveEntity(IEntity entity)
        {
            bool success = false;

            if (_inCoreUpdate)
            {
                // Defer removal until the update loop is complete.
                if (!_deadEntities.ContainsKey(entity.UniqueId))
                    _deadEntities.Add(entity.UniqueId, entity);

                success = true;
            }
            else
            {
                success = RemoveAndDisposeEntity(entity);
            }

            return success;
        }

        /// <summary>
        /// Remove all entities from this container. If this is called during an update, the update cycle will be aborted and the entities will be removed at the end.
        /// </summary>
        public void ClearEntities()
        {
            if (_inCoreUpdate)
                _clearAfterUpdate = true;
            else
                DisposeAndClearEntities();
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Occurs when Update is called and updates all of the entities that belong to this scene, if they are enabled. Also, entities that are no longer alive are removed from the entities list.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.Enabled)
            {
                #region Core Update
                _inCoreUpdate = true;

                base.OnUpdate(gameTime);

                for (int i = _entities.Count - 1; i >= 0; i--)
                {
                    if (_entities[i].Enabled)
                        _entities[i].Update(gameTime);

                    if (_clearAfterUpdate)
                        break;

                    if (_entities[i].Alive == false)
                        this.RemoveEntity(_entities[i]);
                }

                // Entities that were marked as dead in the first loop can now be removed.
                if (_deadEntities.Count > 0)
                {
                    foreach (IEntity deadEntity in _deadEntities.Values)
                        RemoveAndDisposeEntity(deadEntity);

                    _deadEntities.Clear();
                }

                _inCoreUpdate = false;
                #endregion

                // If we were asked to clear during an update, we can clear out entities now.
                if (_clearAfterUpdate)
                {
                    DisposeAndClearEntities();
                    _clearAfterUpdate = false;
                }
            }
        }

        /// <summary>
        /// Occurs when PreDraw is called.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected override void OnPreDraw(GameTime gameTime)
        {
            base.OnPreDraw(gameTime);

            if (this.Visible)
            {
                foreach (IEntity entity in _entities)
                {
                    if (entity.Visible)
                        entity.PreDraw(gameTime);
                }
            }
        }

        /// <summary>
        /// Occurs when Draw is called and draws all entities belonging to this scene, if they are visible.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            if (this.Visible)
            {
                foreach (IEntity entity in _entities)
                {
                    if (entity.Visible)
                        entity.Draw(gameTime);
                }
            }
        }
        #endregion

        #region Private Methods
        private void DisposeAndClearEntities()
        {
            // Dispose of all our entities.
            foreach (IEntity entity in _entities)
            {
                if (_deadEntities.ContainsKey(entity.UniqueId))
                    continue;

                entity.OwningContainer = null;
                entity.Dispose();

                if (_trackedEntities.ContainsKey(entity.UniqueId))
                    _trackedEntities.Remove(entity.UniqueId);
            }

            _entities.Clear();

            // I think the above is fine... not sure why I did it the way I did. Test before we commit though.
            //for (int i = 0; i < _entities.Count; i++)
            //{
            //    IEntity entity = _entities[i];

            //    // Skip any entities that are doomed already.
            //    if (_deadEntities.ContainsKey(entity.UniqueId))
            //        continue;

            //    entity.OwningContainer = null;
            //    entity.Dispose();

            //    _trackedEntities.Remove(entity.UniqueId);

            //    _entities.RemoveAt(i);
            //    i--;
            //}
        }

        private bool RemoveAndDisposeEntity(IEntity entity)
        {
            // Can just go ahead and remove the entity.
            entity.OwningContainer = null;
            entity.Dispose();

            if (_trackedEntities.ContainsKey(entity.UniqueId))
                _trackedEntities.Remove(entity.UniqueId);

            return _entities.Remove(entity);
        }
        #endregion
    }
}
