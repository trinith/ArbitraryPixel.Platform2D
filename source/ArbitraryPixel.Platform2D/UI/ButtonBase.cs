using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.UI.Controller;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.UI
{
    /// <summary>
    /// Base functionality for an object that has button behaviour.
    /// </summary>
    public class ButtonBase : GameEntityBase, IButton
    {
        #region Private Members
        private IButtonController _controller;
        #endregion

        #region IButton Implementation
        /// <summary>
        /// The current state of this button.
        /// </summary>
        //public ButtonState State { get; protected set; } = ButtonState.Unpressed;
        public ButtonState State
        {
            get { return _controller.State; }
        }

        /// <summary>
        /// An arbitrary object to be used for various purposes... perhaps even nefarious ones! -.-
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// An event that occurs when this button is touched, then released within its bounds.
        /// </summary>
        public event EventHandler<ButtonEventArgs> Tapped;

        /// <summary>
        /// An event that occurs when this button is touched within its bounds.
        /// </summary>
        public event EventHandler<ButtonEventArgs> Touched;

        /// <summary>
        /// An even that occurs when this button was touched inside its bounds, and is then released either inside its bounds, or outside.
        /// </summary>
        public event EventHandler<ButtonEventArgs> Released;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new instance, retrieving an IButtonControllerFactory object from the supplied host's components. If such a component is not registered with the host, an exception will be thrown.
        /// </summary>
        /// <param name="host">The IEngine object that owns this button.</param>
        /// <param name="bounds">The bounds of the button.</param>
        public ButtonBase(IEngine host, RectangleF bounds)
            : base(host, bounds)
        {
            Initialize(host.GetComponent<IButtonControllerFactory>() ?? throw new RequiredComponentMissingException());
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="host">The IEngine object that owns this button.</param>
        /// <param name="bounds">The bounds of the button.</param>
        /// <param name="controllerFactory">An object responsible for creating a controller for this button.</param>
        public ButtonBase(IEngine host, RectangleF bounds, IButtonControllerFactory controllerFactory)
            : base(host, bounds)
        {
            Initialize(controllerFactory);
        }
        #endregion

        #region Private Methods
        private void Initialize(IButtonControllerFactory controllerFactory)
        {
            if (controllerFactory == null)
                throw new ArgumentNullException();

            _controller = controllerFactory.Create(this);
            _controller.Tapped += (sender, e) => OnTapped(new ButtonEventArgs(e.Location));
            _controller.Touched += (sender, e) => OnTouched(new ButtonEventArgs(e.Location));
            _controller.Released += (sender, e) => OnReleased(new ButtonEventArgs(e.Location));
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
            return this.Bounds.Contains(p);
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// A method that is called when this object disposes.
        /// </summary>
        /// <param name="disposing">True for disposing from managed code, false otherwise.</param>
        protected override void OnDispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    _controller.Dispose();
                }
            }

            base.OnDispose(disposing);
        }

        /// <summary>
        /// Occurs when Update is called.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _controller.Update(gameTime);
        }
        #endregion
    }
}
