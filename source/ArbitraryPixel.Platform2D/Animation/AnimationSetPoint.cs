namespace ArbitraryPixel.Platform2D.Animation
{
    /// <summary>
    /// A set point for a FloatValueAnimation, defining a target value to achieve and how much time it should take to get there.
    /// </summary>
    public class AnimationSetPoint<T> : IAnimationSetPoint<T>
    {
        /// <summary>
        /// The target value.
        /// </summary>
        public T Target { get; private set; }

        /// <summary>
        /// The amount of time, in seconds, it should take to reach the target.
        /// </summary>
        public float Time { get; private set; }

        /// <summary>
        /// Create a new instance with the specified parameters.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="time">The amount of time, in seconds, to reach the target value.</param>
        public AnimationSetPoint(T target, float time)
        {
            this.Target = target;
            this.Time = time;
        }
    }
}
