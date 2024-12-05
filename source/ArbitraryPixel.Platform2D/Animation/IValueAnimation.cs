using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D.Animation
{
    /// <summary>
    /// Represents an object that will animate a value over a sequence of set points.
    /// </summary>
    /// <typeparam name="T">The type representing the value to animate.</typeparam>
    public interface IValueAnimation<T>
    {
        #region Properties
        /// <summary>
        /// The current value within the animation.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// The set points the value is animating over.
        /// </summary>
        IAnimationSetPoint<T>[] SetPoints { get; }

        /// <summary>
        /// Whether or not the animation should, upon reaching the end of the set points, start again from the beginning.
        /// </summary>
        bool IsLooping { get; set; }

        /// <summary>
        /// Whether or not the animation has finished.
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// The current factor, between 0 and 1, representing the current progress towards the next set point.
        /// </summary>
        float Factor { get; }

        /// <summary>
        /// The set point the animation is currently working towards.
        /// </summary>
        int CurrentSetPoint { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Reset this animation to its initial state.
        /// </summary>
        void Reset();

        /// <summary>
        /// Update the animation for the current state of game time.
        /// </summary>
        /// <param name="gameTime">An object representing the current state of game time.</param>
        void Update(GameTime gameTime);
        #endregion
    }
}
