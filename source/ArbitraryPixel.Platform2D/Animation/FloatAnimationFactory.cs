using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Animation
{
    /// <summary>
    /// An impelemtation of IAnimationFactory that creates float animations.
    /// </summary>
    public class FloatAnimationFactory : IAnimationFactory<float>
    {
        /// <summary>
        /// Create an array of animation set points given an array of values.
        /// </summary>
        /// <param name="values">An array of values where, for each pair of values in the array, the first value represents the target and the second value represents the time.</param>
        /// <returns>An array of set points created from the array of values.</returns>
        public IAnimationSetPoint<float>[] CreateAnimationSetPoints(float[] values)
        {
            if (values.Length % 2 != 0)
                throw new ArgumentException("Value array must contain an even number of values, in pairs of blah and blah.", "values");

            List<IAnimationSetPoint<float>> setPoints = new List<IAnimationSetPoint<float>>();

            for (int i = 0; i < values.Length; i += 2)
            {
                setPoints.Add(new AnimationSetPoint<float>(values[i], values[i + 1]));
            }

            return setPoints.ToArray();
        }

        /// <summary>
        /// Create a value animation.
        /// </summary>
        /// <param name="startValue">The start value for the animation.</param>
        /// <param name="setPoints">An array of values used to create set points for the animation. For each pair of values in the array, the first value represents the target and the second value represents the time.</param>
        /// <param name="isLooping">Whether or not the animation should loop. Defaults to false.</param>
        /// <returns>The newly created animation.</returns>
        public IValueAnimation<float> CreateValueAnimation(float startValue, float[] setPoints, bool isLooping = false)
        {
            return new FloatValueAnimation(startValue, CreateAnimationSetPoints(setPoints)) { IsLooping = isLooping };
        }

        /// <summary>
        /// Create a value anmation.
        /// </summary>
        /// <param name="startValue">The start value for the animation.</param>
        /// <param name="setPoints">An array of animation set points that the animation should animate over.</param>
        /// <param name="isLooping">Whether or not the animation should loop. Defaults to false.</param>
        /// <returns>The newly created animation.</returns>
        public IValueAnimation<float> CreateValueAnimation(float startValue, IAnimationSetPoint<float>[] setPoints, bool isLooping = false)
        {
            return new FloatValueAnimation(startValue, setPoints) { IsLooping = isLooping };
        }

        /// <summary>
        /// Create a new animation collection.
        /// </summary>
        /// <returns>The newly created animation collection.</returns>
        public IAnimationCollection<float> CreateAnimationCollection()
        {
            return new AnimationCollection<float>();
        }
    }
}
