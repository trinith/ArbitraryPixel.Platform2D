using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.UI.Controller
{
    /// <summary>
    /// Represents an object that controls an IButton object.
    /// </summary>
    public interface IButtonController : IDisposable
    {
        /// <summary>
        /// The ButtonState for this controller.
        /// </summary>
        ButtonState State { get; }

        /// <summary>
        /// The button this controller is targetting.
        /// </summary>
        IButton TargetButton { get; }

        /// <summary>
        /// An event that occurs when this controller detects a tap.
        /// </summary>
        event EventHandler<ButtonEventArgs> Tapped;

        /// <summary>
        /// An event that occurs when this controller detects a touch.
        /// </summary>
        event EventHandler<ButtonEventArgs> Touched;

        /// <summary>
        /// An event that occurs when this controller detects a release.
        /// </summary>
        event EventHandler<ButtonEventArgs> Released;

        /// <summary>
        /// Update this controller's state.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        void Update(GameTime gameTime);
    }
}
