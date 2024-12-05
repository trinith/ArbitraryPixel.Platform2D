using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.UI.Controller
{
    /// <summary>
    /// An object responsible for providing base functionality for an IButtonController implementation.
    /// </summary>
    public class ButtonControllerBase : IButtonController
    {
        #region Constructor(s)
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="targetButton">The IButton object that this controller will target.</param>
        public ButtonControllerBase(IButton targetButton)
        {
            this.TargetButton = targetButton ?? throw new ArgumentNullException();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Occurs when the conditions for a tapped event are met.
        /// </summary>
        /// <param name="e">The arguments associated with this event.</param>
        protected virtual void OnTapped(ButtonEventArgs e)
        {
            if (this.Tapped != null)
                this.Tapped(this, e);
        }

        /// <summary>
        /// Occurs when the conditions for a touched event are met.
        /// </summary>
        /// <param name="e">The arguments associated with this event.</param>
        protected virtual void OnTouched(ButtonEventArgs e)
        {
            if (this.Touched != null)
                this.Touched(this, e);
        }

        /// <summary>
        /// Occurs when the conditions for a released event are met.
        /// </summary>
        /// <param name="e">The arguments associated with this event.</param>
        protected virtual void OnReleased(ButtonEventArgs e)
        {
            if (this.Released != null)
                this.Released(this, e);
        }

        /// <summary>
        /// Test to see if a point is inside the bounds of this button.
        /// </summary>
        /// <param name="p">The point to test.</param>
        /// <returns>True if the point is within this button's bounds, false otherwise.</returns>
        protected virtual bool IsPointInBounds(Vector2 p)
        {
            return this.TargetButton.Bounds.Contains(p);
        }
        #endregion

        #region IButtonController Implementation
        /// <summary>
        /// This ButtonState for this controller.
        /// </summary>
        public ButtonState State { get; protected set; } = ButtonState.Unpressed;

        /// <summary>
        /// The button this controller is targetting.
        /// </summary>
        public IButton TargetButton { get; protected set; }

        /// <summary>
        /// An event that occurs when this controller detects a tap.
        /// </summary>
        public event EventHandler<ButtonEventArgs> Tapped;

        /// <summary>
        /// An event that occurs when this controller detects a touch.
        /// </summary>
        public event EventHandler<ButtonEventArgs> Touched;

        /// <summary>
        /// An event that occurs when this controller detects a release.
        /// </summary>
        public event EventHandler<ButtonEventArgs> Released;

        /// <summary>
        /// Update this controller's state.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        /// <summary>
        /// Occurs when this controller's Update method is called.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected virtual void OnUpdate(GameTime gameTime) { }
        #endregion

        #region IDisposable Implementation
        /// <summary>
        /// Whether or not this object has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; } = false;

        /// <summary>
        /// Dispose of this object.
        /// </summary>
        public void Dispose()
        {
            OnDispose(true);
        }

        /// <summary>
        /// Occurs when this controller's Dispose method is called.
        /// </summary>
        /// <param name="disposing">True for disposing from managed code, false otherwise.</param>
        protected virtual void OnDispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                this.IsDisposed = true;
            }
        }
        #endregion
    }
}
