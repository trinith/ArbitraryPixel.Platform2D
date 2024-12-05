using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace ArbitraryPixel.Platform2D.UI.Controller
{
    /// <summary>
    /// An object responsible for controlling a button using a multi touch input paradigm.
    /// </summary>
    public class MultiTouchButtonController : ButtonControllerBase
    {
        #region Private Class
        // Create a simple class that can store identifying information about a touch.
        // NOTE: Only need the id right now, but leaving this structure in place because nullable fields are annoying to work with.
        private class TouchInfo
        {
            public int Id { get; private set; }

            public TouchInfo(TouchLocation touch)
            {
                this.Update(touch);
            }

            public void Update(TouchLocation touch)
            {
                this.Id = touch.Id;
            }
        }
        #endregion

        #region Private Members
        private TouchInfo _touch = null;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="targetButton">The IButton object that this controller will target.</param>
        public MultiTouchButtonController(IButton targetButton)
            : base(targetButton)
        {
        }
        #endregion

        #region Method Overrides
        /// <summary>
        /// Occurs when this controller's Dispose method is called.
        /// </summary>
        /// <param name="disposing">True for disposing from managed code, false otherwise.</param>
        protected override void OnDispose(bool disposing)
        {
            if (!this.IsDisposed && disposing && _touch != null)
            {
                if (this.TargetButton.Host.InputManager.ShouldConsumeInput(_touch.Id, this))
                    this.TargetButton.Host.InputManager.ClearConsumer(_touch.Id);
            }

            base.OnDispose(disposing);
        }

        /// <summary>
        /// Occurs when this controller's Update method is called.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            TouchCollection touches = this.TargetButton.Host.InputManager.GetSurfaceState().Touches;

            if (_touch == null)
            {
                // We currently do not have a touch locked to this controller, so go searching through the current touches to see if we can find a
                // touch that is in the bounds of this controller's button and is consumable.
                foreach (TouchLocation touch in touches)
                {
                    Vector2 surfaceLocation = this.TargetButton.Host.ScreenManager.PointToWorld(touch.Position);

                    if (this.IsPointInBounds(surfaceLocation) && touch.State == TouchLocationState.Pressed && this.TargetButton.Host.InputManager.ShouldConsumeInput(touch.Id, this))
                    {
                        _touch = new TouchInfo(touch);
                        this.TargetButton.Host.InputManager.SetConsumer(_touch.Id, this);
                        this.State = ButtonState.Pressed;
                        OnTouched(new ButtonEventArgs(surfaceLocation));
                        break;
                    }
                }
            }
            else
            {
                // This controller is already tracking a touch for its target button so check its state.
                if (touches.FindById(_touch.Id, out TouchLocation touch) == true)
                {
                    _touch.Update(touch);
                    Vector2 surfaceLocation = this.TargetButton.Host.ScreenManager.PointToWorld(touch.Position);

                    if (touch.State == TouchLocationState.Released)
                    {
                        OnReleased(new ButtonEventArgs(surfaceLocation));

                        if (this.IsPointInBounds(surfaceLocation))
                            OnTapped(new ButtonEventArgs(surfaceLocation));

                        this.TargetButton.Host.InputManager.ClearConsumer(_touch.Id);
                        this.State = ButtonState.Unpressed;
                        _touch = null;
                    }
                    else
                    {
                        this.State = (this.IsPointInBounds(surfaceLocation)) ? ButtonState.Pressed : ButtonState.Unpressed;
                    }
                }
                else
                {
                    // The touch was seemingly abruptly dropped from the touch collection. We don't know if the touch was released inside the button
                    // or not, so we just reset the button state and move on.
                    this.TargetButton.Host.InputManager.ClearConsumer(_touch.Id);
                    this.State = ButtonState.Unpressed;
                    _touch = null;
                }
            }
        }
        #endregion
    }
}
