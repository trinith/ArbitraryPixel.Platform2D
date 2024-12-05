namespace ArbitraryPixel.Platform2D.Time
{
    /// <summary>
    /// Represents an object responsible for creating IStopwatch objects.
    /// </summary>
    public interface IStopwatchFactory
    {
        /// <summary>
        /// Create a new IStopwatch object.
        /// </summary>
        /// <returns>The newly created object.</returns>
        IStopwatch Create();
    }
}
