namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// Represents an object that will manage ITextFormatValueHandler objects.
    /// </summary>
    public interface ITextFormatValueHandlerManager
    {
        /// <summary>
        /// A character that separates format names when registering a handler.
        /// </summary>
        char SeparatorChar { get; }

        /// <summary>
        /// Register a ValueHandler for a format against the strings it is associated with. Names will be considered case insenstive.
        /// </summary>
        /// <param name="stringNames">A string containing names, separated by SeparatorChar, which can set the value associated with the format name</param>
        /// <param name="format">The format name describing the value to set.</param>
        /// <param name="handler">The object that will handle values set for this format name.</param>
        void RegisterValueHandler(string stringNames, SupportedFormat format, ITextFormatValueHandler handler);

        /// <summary>
        /// Whether or not a handler has been registered for the given format name string. Case insensitive.
        /// </summary>
        /// <param name="formatName">The format name string to check.</param>
        /// <returns>True if the provided format name can be handled, false otherwise.</returns>
        bool CanHandleFormatName(string formatName);

        /// <summary>
        /// Call the handler associated with the given format name to process the given value string. When processing is complete, the provided callback is called.
        /// </summary>
        /// <param name="formatName">The format name the value string is associated with.</param>
        /// <param name="valueString">A string containing the value to handle.</param>
        /// <param name="callback">A method to call when handling the value is complete.</param>
        void HandleValue(string formatName, string valueString, FormatValueHandledCallback callback);
    }
}
