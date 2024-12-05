using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// An object responsible for managing ITextFormatValueHandler objects.
    /// </summary>
    public class TextFormatValueHandlerManager : ITextFormatValueHandlerManager
    {
        private Dictionary<string, SupportedFormat> _formatNameMap = new Dictionary<string, SupportedFormat>();
        private Dictionary<SupportedFormat, ITextFormatValueHandler> _handlers = new Dictionary<SupportedFormat, ITextFormatValueHandler>();

        /// <summary>
        /// A character that separates format names when registering a handler.
        /// </summary>
        public char SeparatorChar => ':';

        /// <summary>
        /// Register a ValueHandler for a format against the strings it is associated with. Names will be considered case insenstive.
        /// </summary>
        /// <param name="stringNames">A string containing names, separated by SeparatorChar, which can set the value associated with the format name</param>
        /// <param name="format">The format name describing the value to set.</param>
        /// <param name="handler">The object that will handle values set for this format name.</param>
        public void RegisterValueHandler(string stringNames, SupportedFormat format, ITextFormatValueHandler handler)
        {
            string[] tokens = stringNames.Split(this.SeparatorChar.ToString().ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0)
                throw new ArgumentException();

            foreach (string token in tokens)
            {
                string formatName = token.Trim();

                if (string.IsNullOrEmpty(formatName) == false && _formatNameMap.ContainsKey(token.ToLower()) == false)
                    _formatNameMap.Add(token.ToLower(), format);
            }

            _handlers.Add(format, handler ?? throw new ArgumentNullException());
        }

        /// <summary>
        /// Whether or not a handler has been registered for the given format name string. Case insensitive.
        /// </summary>
        /// <param name="formatName">The format name string to check.</param>
        /// <returns>True if the provided format name can be handled, false otherwise.</returns>
        public bool CanHandleFormatName(string formatName)
        {
            return _formatNameMap.ContainsKey(formatName.ToLower());
        }

        /// <summary>
        /// Call the handler associated with the given format name to process the given value string. When processing is complete, the provided callback is called.
        /// </summary>
        /// <param name="formatName">The format name the value string is associated with.</param>
        /// <param name="valueString">A string containing the value to handle.</param>
        /// <param name="callback">A method to call when handling the value is complete.</param>
        public void HandleValue(string formatName, string valueString, FormatValueHandledCallback callback)
        {
            SupportedFormat format = _formatNameMap[formatName];
            _handlers[format].HandleValue(format, valueString, callback);
        }
    }
}
