using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Animation
{
    /// <summary>
    /// Represents a collection of value animations, indexed by name.
    /// </summary>
    /// <typeparam name="T">The type of value animation stored in this collection.</typeparam>
    public interface IAnimationCollection<T> : IEnumerable<IValueAnimation<T>>
    {
        /// <summary>
        /// Get the animation for the specified name.
        /// </summary>
        /// <param name="animationName">The name of the animation to get.</param>
        /// <returns>The animation with the specified name, if it exists. If it does not, an exception will be thrown.</returns>
        IValueAnimation<T> this[string animationName] { get; }

        /// <summary>
        /// Add an animation to this collection.
        /// </summary>
        /// <param name="animationName">The name of the animation to add.</param>
        /// <param name="animation">The animation to add to this collection.</param>
        void Add(string animationName, IValueAnimation<T> animation);

        /// <summary>
        /// Remove an animation from this collection given a specified name.
        /// </summary>
        /// <param name="animationName">The name of the animation to remove.</param>
        /// <returns>True if the animation was successfully removed, False otherwise.</returns>
        bool Remove(string animationName);

        /// <summary>
        /// Whether or not this collection contains an animation of the specified name.
        /// </summary>
        /// <param name="animationName">The name of the animation to check.</param>
        /// <returns>True if the specified animation name exists in this collection, False otherwise.</returns>
        bool Contains(string animationName);
    }
}
