namespace ArbitraryPixel.Platform2D.Time
{
    /// <summary>
    /// Represents an object that manages IStopwatch objects.
    /// </summary>
    public interface IStopwatchManager
    {
        /// <summary>
        /// Create a new IStopwatch object to be managed by this manager.
        /// </summary>
        /// <returns>The newly created object.</returns>
        IStopwatch Create();

        /// <summary>
        /// Start all owned IStopwatch objects.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop all owned IStopwatch objects.
        /// </summary>
        void Stop();

        /// <summary>
        /// Reset all owned IStopwatch objects.
        /// </summary>
        void Reset();

        /// <summary>
        /// Dispose and remove of all objects owned by this manager.
        /// </summary>
        void Clear();
    }
}
