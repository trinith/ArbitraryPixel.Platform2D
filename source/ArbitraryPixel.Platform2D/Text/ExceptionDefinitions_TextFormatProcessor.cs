using System;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// An exception that is thrown when an invalid format string is processed.
    /// </summary>
    public class InvalidFormatException : Exception
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="msg">The message associated with this exception.</param>
        public InvalidFormatException(string msg) : base(msg) { }
    }

    /// <summary>
    /// An exception that is thrown when a format string being processed has format name that is not registered with the processor.
    /// </summary>
    public class InvalidFormatNameException : Exception
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="msg">The message associated with this exception.</param>
        public InvalidFormatNameException(string msg) : base(msg) { }
    }

    /// <summary>
    /// An exception that is thrown when the handler registered with a given format name throws an exception while handling the value string.
    /// </summary>
    public class InvalidFormatValueException : Exception
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="msg">The message associated with this exception.</param>
        /// <param name="innerException">The exception that occurred when converting the value.</param>
        public InvalidFormatValueException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
