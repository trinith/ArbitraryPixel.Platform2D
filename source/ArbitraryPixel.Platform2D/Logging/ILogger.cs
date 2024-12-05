namespace ArbitraryPixel.Platform2D.Logging
{
    /// <summary>
    /// Represents an object responsible for logging messages.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Whether or not logged messages should prepend a time stamp.
        /// </summary>
        bool UseTimeStamps { get; set; }

        /// <summary>
        /// Write a message to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        void WriteLine(string message);
    }
}
