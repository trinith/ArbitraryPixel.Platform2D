using System;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// An exception that is thrown when a format string tries to set a font via a name that is not registered with the ITextObjectBuilder building ITextObjects for that format string.
    /// </summary>
    public class UnregisteredFontNameException : Exception
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="msg">The message associated with this exception.</param>
        public UnregisteredFontNameException(string msg) : base(msg) { }
    }

    /// <summary>
    /// An exception that is thrown when a format string contains an unrecognized escape sequence.
    /// </summary>
    public class UnrecognizedEscapeSeqeunceException : Exception
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="msg">The message associated with this exception.</param>
        public UnrecognizedEscapeSeqeunceException(string msg) : base(msg) { }
    }
}
