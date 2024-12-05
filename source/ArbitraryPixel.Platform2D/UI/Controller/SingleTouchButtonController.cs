using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.UI.Controller
{
    /// <summary>
    /// An object responsible for controlling a button using a single touch input paradigm.
    /// </summary>
    public class SingleTouchButtonController : ButtonControllerBase
    {
        #region Private Members / Properties
        private SurfaceState _previousState;
        private Vector2? _initialTouch = null;

        private IEngine Host { get { return this.TargetButton.Host; } }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="targetButton">The IButton object that this controller will target.</param>
        public SingleTouchButtonController(IButton targetButton)
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
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    if (this.Host.InputManager.ShouldConsumeInput(this))
                        this.Host.InputManager.ClearConsumer();
                }
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

            SurfaceState currentState = this.Host.InputManager.GetSurfaceState();

            Vector2 surfaceLocation = this.Host.ScreenManager.PointToWorld(currentState.SurfaceLocation);

            if (this.Host.InputManager.IsActive && this.Host.InputManager.ShouldConsumeInput(this))
            {
                if (_previousState.IsTouched == false && currentState.IsTouched == true && IsPointInBounds(surfaceLocation))
                {
                    this.Host.InputManager.SetConsumer(this);
                    this.State = ButtonState.Pressed;
                    _initialTouch = surfaceLocation;
                    OnTouched(new ButtonEventArgs(surfaceLocation));
                }
                else if (_initialTouch != null && currentState.IsTouched && IsPointInBounds(surfaceLocation))
                {
                    this.State = ButtonState.Pressed;
                }
                else if (_previousState.IsTouched == true && currentState.IsTouched == false)
                {
                    Vector2 previousLocation = this.Host.ScreenManager.PointToWorld(_previousState.SurfaceLocation);

                    if (_initialTouch != null)
                    {
                        OnReleased(new ButtonEventArgs(previousLocation));

                        if (IsPointInBounds(previousLocation))
                            OnTapped(new ButtonEventArgs(previousLocation));

                        this.Host.InputManager.ClearConsumer();
                    }

                    this.State = ButtonState.Unpressed;
                    _initialTouch = null;
                }
                else if (_initialTouch != null && currentState.IsTouched && !IsPointInBounds(surfaceLocation))
                {
                    this.State = ButtonState.Unpressed;
                }
            }

            _previousState = currentState;
        }
        #endregion
    }
}
