using System;

namespace ArbitraryPixel.Platform2D.Text.ValueHandlers
{
    /// <summary>
    /// An object responsible for handling TextLineAlignment values.
    /// </summary>
    public class TextLineAlignmentValueHandler : ITextFormatValueHandler
    {
        /// <summary>
        /// Handle a value string and set the appropriate value on the supplied processor.
        /// </summary>
        /// <param name="format">The format the value string is for.</param>
        /// <param name="valueString">The value string to handle</param>
        /// <param name="callback">A callback for when handling the value is finished.</param>
        public void HandleValue(SupportedFormat format, string valueString, FormatValueHandledCallback callback)
        {
            if (callback == null)
                throw new ArgumentNullException();

            try
            {
                callback(SupportedFormat.LineAlignment, (TextLineAlignment)Enum.Parse(typeof(TextLineAlignment), valueString));
            }
            catch /* any exception */
            {
                throw new InvalidTextLineAlignmentValueStringException($"Cannot convert '{valueString}' to a TextLineAlignmentValue.");
            }
        }
    }
}
