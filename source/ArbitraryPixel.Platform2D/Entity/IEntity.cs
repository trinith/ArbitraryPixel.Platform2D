using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.Entity
{
    /// <summary>
    /// Represents an entity in a Platform2D game.
    /// </summary>
    public interface IEntity : IUpdateableEntity, IDrawableEntity, IContainedEntity, IDisposable
    {
        /// <summary>
        /// A unique identifier generated when the entity is created.
        /// </summary>
        string UniqueId { get; }

        /// <summary>
        /// Whether or not this entity is alive, or if it can be flagged for deletion on the next update.
        /// </summary>
        bool Alive { get; set; }

        /// <summary>
        /// Whether or not this entity has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// An event that fires when this entity is disposed.
        /// </summary>
        event EventHandler<EventArgs> Disposed;

        /// <summary>
        /// An event that fires before drawing begins.
        /// </summary>
        event EventHandler<ValueEventArgs<GameTime>> DrawBegin;

        /// <summary>
        /// An event that fires before drawing ends.
        /// </summary>
        event EventHandler<ValueEventArgs<GameTime>> DrawEnd;

        /// <summary>
        /// Draw this scene to the specified render target. This should not be called while any render objects, such as an ISpriteBatch object, are open. A good place to call this would be from PreDraw :)
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        /// <param name="target">The target to draw to.</param>
        /// <param name="clearColour">If set, clears the graphics device with the specified colour.</param>
        void Draw(GameTime gameTime, IRenderTarget2D target, Color? clearColour = null);
    }
}
