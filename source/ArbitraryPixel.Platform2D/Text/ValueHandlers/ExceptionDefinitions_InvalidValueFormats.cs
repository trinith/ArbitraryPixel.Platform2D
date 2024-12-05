using System;

namespace ArbitraryPixel.Platform2D.Text.ValueHandlers
{
    /// <summary>
    /// An exception that is thrown when ColourValueHandler tries to handle a string that it cannot convert to a Color object.
    /// </summary>
    public class InvalidColourValueStringException : Exception
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="msg">The message associated with this exception.</param>
        public InvalidColourValueStringException(string msg) : base(msg) { }
    }

    /// <summary>
    /// An exception that is thrown when TExtLineAlignmentValueHadler tries to handle a string that it cannot convert to a TextLineAlignment value.
    /// </summary>
    public class InvalidTextLineAlignmentValueStringException : Exception
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="msg">The message associated with this exception.</param>
        public InvalidTextLineAlignmentValueStringException(string msg) : base(msg) { }
    }
}
