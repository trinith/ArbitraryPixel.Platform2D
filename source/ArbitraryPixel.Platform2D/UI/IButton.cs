using ArbitraryPixel.Platform2D.Entity;
using System;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Represents an object that acts as a button.
    /// </summary>
    public interface IButton : IGameEntity, IHostedEntity
    {
        /// <summary>
        /// The current state of this button.
        /// </summary>
        ButtonState State { get; }

        /// <summary>
        /// An arbitrary object to be used for various purposes... perhaps even nefarious ones! -.-
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// An even that occurs when this button was touched inside its bounds, and is then released either inside its bounds, or outside.
        /// </summary>
        event EventHandler<ButtonEventArgs> Released;

        /// <summary>
        /// An event that occurs when this button is touched, then released within its bounds.
        /// </summary>
        event EventHandler<ButtonEventArgs> Tapped;

        /// <summary>
        /// An event that occurs when this button is touched within its bounds.
        /// </summary>
        event EventHandler<ButtonEventArgs> Touched;
    }
}