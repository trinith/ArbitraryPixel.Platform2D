using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Text.Factory
{
    /// <summary>
    /// Represents an object responsible for creating implementations of interfaces from the ArbitraryPixel.Platform2D.Text namespace.
    /// </summary>
    public interface ITextFactory
    {
        #region Core Objects
        /// <summary>
        /// Create a new ITextFormatProcessor object.
        /// </summary>
        /// <param name="textFormatValueHandlerManager">An object responsible for managing ITextFormatValueHandler objects.</param>
        /// <returns>The newly created object.</returns>
        ITextFormatProcessor CreateTextFormatProcessor(ITextFormatValueHandlerManager textFormatValueHandlerManager);

        /// <summary>
        /// Create a new ITextFormatValueHandlerManager object.
        /// </summary>
        /// <returns>The newly created object.</returns>
        ITextFormatValueHandlerManager CreateTextFormatValueHandlerManager();

        /// <summary>
        /// Create a new ITextFormatValueHandlerManager object, then register handlers according to the provided format map.
        /// </summary>
        /// <param name="formatMap">A dictionary mapping supported formats to format strings. Handlers will be created as dictated by the specified SupportedFormat.</param>
        /// <returns>The newly created object, registered with handlers created according to the supplied format map.</returns>
        ITextFormatValueHandlerManager CreateTextFormatValueHandlerManagerWithRegisteredHandlers(Dictionary<SupportedFormat, string> formatMap);

        /// <summary>
        /// Create a new ITextObjectBuilder object.
        /// </summary>
        /// <param name="textFormatProcessor">An object responsible for processing text formats.</param>
        /// <param name="textObjectFactory">An object responisble for creating ITextObject objects.</param>
        /// <returns>The newly created object.</returns>
        ITextObjectBuilder CreateTextObjectBuilder(ITextFormatProcessor textFormatProcessor, ITextObjectFactory textObjectFactory);

        /// <summary>
        /// Create a new ITextObjectFactory object.
        /// </summary>
        /// <returns>The newly created object.</returns>
        ITextObjectFactory CreateTextObjectFactory();
        #endregion

        #region Format Value Handlers
        /// <summary>
        /// Create a new ITextFormatValueHandler object responsible for handling colour values.
        /// </summary>
        /// <returns>The newly created object.</returns>
        ITextFormatValueHandler CreateColourValueHandler();

        /// <summary>
        /// Create a new ITextFormatValueHandler object responsible for handling decimal values.
        /// </summary>
        /// <returns>The newly created object.</returns>
        ITextFormatValueHandler CreateDecimalValueHandler();

        /// <summary>
        /// Create a new ITextFormatValueHandler object responsible for handling string values.
        /// </summary>
        /// <returns>The newly created object.</returns>
        ITextFormatValueHandler CreateStringValueHandler();

        /// <summary>
        /// Create a new ITextFormatValueHandler object responsible for handling TextLineAlignment values.
        /// </summary>
        /// <returns>The newly created object.</returns>
        ITextFormatValueHandler CreateTextLineAlignmentValueHandler();

        /// <summary>
        /// Create a default mapping of supported formats to format strings.
        /// </summary>
        /// <returns>The newly created object.</returns>
        Dictionary<SupportedFormat, string> CreateDefaultValueHandlerFormatMap();
        #endregion
    }
}