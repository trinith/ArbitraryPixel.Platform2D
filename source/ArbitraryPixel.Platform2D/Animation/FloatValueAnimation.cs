using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.Platform2D.Animation
{
    /// <summary>
    /// An object that will animate a float value over a sequence of set points.
    /// </summary>
    public class FloatValueAnimation : IValueAnimation<float>
    {
        private float? _factorVelocity = null;
        private List<IAnimationSetPoint<float>> _setPoints = new List<IAnimationSetPoint<float>>();

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="startValue">The value animation will start at.</param>
        /// <param name="setPoints">A sequence of one or more set points that the animation will follow. This sequence must exist and cannot contain null entries.</param>
        public FloatValueAnimation(float startValue, IEnumerable<IAnimationSetPoint<float>> setPoints)
        {
            if (setPoints == null || setPoints.ToList().Count == 0)
                throw new ArgumentNullException("Must provide a non-null, non-empty sequence.");

            if (setPoints.ToList().Contains(null))
                throw new ArgumentException("The sequence of set points cannot contain a null entry.");

            this.Value = startValue;

            _setPoints.Add(new AnimationSetPoint<float>(startValue, 0));
            _setPoints.AddRange(setPoints);
        }

        #region IValueAnimation<float> Implementation
        /// <summary>
        /// The current value within the animation.
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        /// The set points the value is animating over.
        /// </summary>
        public IAnimationSetPoint<float>[] SetPoints => _setPoints.ToArray();

        /// <summary>
        /// Whether or not the animation should, upon reaching the end of the set points, start again from the beginning.
        /// </summary>
        public bool IsLooping { get; set; }

        /// <summary>
        /// Whether or not the animation has finished.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// The current factor, between 0 and 1, representing the current progress towards the next set point.
        /// </summary>
        public float Factor { get; private set; }

        /// <summary>
        /// The set point the animation is currently working towards.
        /// </summary>
        public int CurrentSetPoint { get; private set; }

        /// <summary>
        /// Reset this animation to its initial state.
        /// </summary>
        public void Reset()
        {
            this.Value = _setPoints[0].Target;

            this.CurrentSetPoint = 0;
            this.IsComplete = false;

            this.Factor = 0;
            _factorVelocity = null;
        }

        /// <summary>
        /// Update the animation for the current state of game time.
        /// </summary>
        /// <param name="gameTime">An object representing the current state of game time.</param>
        public void Update(GameTime gameTime)
        {
            // TODO: Refactor this so we consume exact chunks of time. Currently we don't fully utilize an update if it pushes us to the next breakpoint.
            // The implementation to correct this is actually simpler than what I did because I won't actually need velocity. Just use the times and subtract from
            // elapsed time for each setpoint... that kind of thing. Get rid of velocity entirely :)

            if (this.CurrentSetPoint < _setPoints.Count - 1)
            {
                if (this.Factor == 1f)
                {
                    _factorVelocity = null;
                    this.CurrentSetPoint++;

                    if (this.CurrentSetPoint >= _setPoints.Count - 1)
                    {
                        if (this.IsLooping)
                        {
                            this.CurrentSetPoint = 0;
                            this.Factor = 0;
                        }
                        else
                        {
                            this.IsComplete = true;
                        }

                        _factorVelocity = null;
                    }
                }

                if (this.IsComplete == false)
                {
                    if (_factorVelocity == null)
                    {
                        this.Factor = 0f;
                        _factorVelocity = 1f / _setPoints[this.CurrentSetPoint + 1].Time;
                    }

                    this.Factor += _factorVelocity.Value * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    this.Factor = MathHelper.Clamp(this.Factor, 0f, 1f);  // TODO: Refactor this to put unused time to the next setpoint.

                    this.Value = MathHelper.SmoothStep(_setPoints[this.CurrentSetPoint].Target, _setPoints[this.CurrentSetPoint + 1].Target, this.Factor);
                }
            }
        }
        #endregion
    }
}
