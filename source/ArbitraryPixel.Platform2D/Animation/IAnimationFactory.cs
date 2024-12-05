namespace ArbitraryPixel.Platform2D.Animation
{
    /// <summary>
    /// Represents an object that creates objects for use in animation.
    /// </summary>
    public interface IAnimationFactory<T>
    {
        /// <summary>
        /// Create an array of animation set points given an array of values.
        /// </summary>
        /// <param name="values">An array of values used to create set points for the animation. For each pair of values in the array, the first value represents the target and the second value represents the time.</param>
        /// <returns>An array of set points created from the array of values.</returns>
        IAnimationSetPoint<T>[] CreateAnimationSetPoints(T[] values);

        /// <summary>
        /// Create a value animation.
        /// </summary>
        /// <param name="startValue">The start value for the animation.</param>
        /// <param name="setPoints">An array of values used to create set points for the animation. For each pair of values in the array, the first value represents the target and the second value represents the time.</param>
        /// <param name="isLooping">Whether or not the animation should loop. Defaults to false.</param>
        /// <returns>The newly created animation.</returns>
        IValueAnimation<T> CreateValueAnimation(T startValue, T[] setPoints, bool isLooping = false);

        /// <summary>
        /// Create a value anmation.
        /// </summary>
        /// <param name="startValue">The start value for the animation.</param>
        /// <param name="setPoints">An array of animation set points that the animation should animate over.</param>
        /// <param name="isLooping">Whether or not the animation should loop. Defaults to false.</param>
        /// <returns>The newly created animation.</returns>
        IValueAnimation<T> CreateValueAnimation(T startValue, IAnimationSetPoint<T>[] setPoints, bool isLooping = false);

        /// <summary>
        /// Create a new animation collection.
        /// </summary>
        /// <returns>The newly created animation collection.</returns>
        IAnimationCollection<T> CreateAnimationCollection();
    }
}
