using System.Collections;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Animation
{
    /// <summary>
    /// An object responsible for storing a collection of IValueAnimation objects.
    /// </summary>
    /// <typeparam name="T">The value type for the animations stored in this object.</typeparam>
    public class AnimationCollection<T> : IAnimationCollection<T>
    {
        private Dictionary<string, IValueAnimation<T>> _animations = new Dictionary<string, IValueAnimation<T>>();

        #region IAnimationCollection Implementation
        /// <summary>
        /// Get the animation for the specified name.
        /// </summary>
        /// <param name="animationName">The name of the animation to get.</param>
        /// <returns>The animation with the specified name, if it exists. If it does not, an exception will be thrown.</returns>
        public IValueAnimation<T> this[string animationName] => _animations[animationName];

        /// <summary>
        /// Add an animation to this collection.
        /// </summary>
        /// <param name="animationName">The name of the animation to add.</param>
        /// <param name="animation">The animation to add to this collection.</param>
        public void Add(string animationName, IValueAnimation<T> animation)
        {
            _animations.Add(animationName, animation);
        }

        /// <summary>
        /// Remove an animation from this collection given a specified name.
        /// </summary>
        /// <param name="animationName">The name of the animation to remove.</param>
        public bool Remove(string animationName)
        {
            return _animations.Remove(animationName);
        }

        /// <summary>
        /// Whether or not this collection contains an animation of the specified name.
        /// </summary>
        /// <param name="animationName">The name of the animation to check.</param>
        /// <returns>True if the specified animation name exists in this collection, False otherwise.</returns>
        public bool Contains(string animationName)
        {
            return _animations.ContainsKey(animationName);
        }
        #endregion

        #region IEnumerable Implementation
        /// <summary>
        /// Get the enumerator for this object.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IValueAnimation<T>> GetEnumerator()
        {
            return _animations.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _animations.Values.GetEnumerator();
        }
        #endregion
    }
}
