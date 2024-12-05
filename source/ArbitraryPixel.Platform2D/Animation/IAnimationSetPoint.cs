namespace ArbitraryPixel.Platform2D.Animation
{
    /// <summary>
    /// Represents an animation set point.
    /// </summary>
    /// <typeparam name="T">A data type that will store the target value.</typeparam>
    public interface IAnimationSetPoint<T>
    {
        /// <summary>
        /// The target value.
        /// </summary>
        T Target { get; }

        /// <summary>
        /// The amount of time, in seconds, it should take to reach the target.
        /// </summary>
        float Time { get; }
    }
}
