using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Engine;
using System;

namespace ArbitraryPixel.Platform2D.Entity
{
    /// <summary>
    /// Extends EntityBase to provide IGameEntity functionality.
    /// </summary>
    public class GameEntityBase : EntityBase, IGameEntity
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="host">The IEngine object that this entity belongs to.</param>
        /// <param name="bounds">The bounds in the gameworld of this entity.</param>
        public GameEntityBase(IEngine host, RectangleF bounds)
            : base(host)
        {
            if (bounds == RectangleF.Empty)
                throw new ArgumentException("Bounds cannot be empty.");

            this.Bounds = bounds;
        }

        #region IGameEntity Implementation
        /// <summary>
        /// The bounds within the world coordinate space that this entity occupies.
        /// </summary>
        public virtual RectangleF Bounds { get; set; }
        #endregion
    }
}
