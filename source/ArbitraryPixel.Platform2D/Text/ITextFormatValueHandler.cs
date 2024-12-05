namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// Describes a method that is called when a format value has been handled.
    /// </summary>
    /// <param name="formatName">The format name that the value represents.</param>
    /// <param name="value">The object created by the handler.</param>
    public delegate void FormatValueHandledCallback(SupportedFormat formatName, object value);

    /// <summary>
    /// Represnts an object that will handle a format value, parsing the provided value string.
    /// </summary>
    public interface ITextFormatValueHandler
    {
        /// <summary>
        /// Handle a value string and set the appropriate value on the supplied processor.
        /// </summary>
        /// <param name="format">The format the value string is for.</param>
        /// <param name="valueString">The value string to handle</param>
        /// <param name="callback">A callback for when handling the value is finished.</param>
        void HandleValue(SupportedFormat format, string valueString, FormatValueHandledCallback callback);
    }
}
