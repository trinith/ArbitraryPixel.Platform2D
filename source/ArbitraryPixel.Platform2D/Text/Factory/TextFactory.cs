using ArbitraryPixel.Platform2D.Text.ValueHandlers;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Text.Factory
{
    /// <summary>
    /// An object responsible for creating objects from the ArbitraryPixel.Platform2D.Text namespace.
    /// </summary>
    public class TextFactory : ITextFactory
    {
        #region Core Objects
        /// <summary>
        /// Create a new ITextObjectBuilder object.
        /// </summary>
        /// <param name="textFormatProcessor">An object responsible for processing text formats.</param>
        /// <param name="textObjectFactory">An object responisble for creating ITextObject objects.</param>
        /// <returns>The newly created object.</returns>
        public virtual ITextObjectBuilder CreateTextObjectBuilder(ITextFormatProcessor textFormatProcessor, ITextObjectFactory textObjectFactory)
        {
            return new TextObjectBuilder(textFormatProcessor, textObjectFactory);
        }

        /// <summary>
        /// Create a new ITextFormatProcessor object.
        /// </summary>
        /// <param name="textFormatValueHandlerManager">An object responsible for managing ITextFormatValueHandler objects.</param>
        /// <returns>The newly created object.</returns>
        public virtual ITextFormatProcessor CreateTextFormatProcessor(ITextFormatValueHandlerManager textFormatValueHandlerManager)
        {
            return new TextFormatProcessor(textFormatValueHandlerManager);
        }

        /// <summary>
        /// Create a new ITextFormatValueHandlerManager object.
        /// </summary>
        /// <returns>The newly created object.</returns>
        public virtual ITextFormatValueHandlerManager CreateTextFormatValueHandlerManager()
        {
            return new TextFormatValueHandlerManager();
        }

        /// <summary>
        /// Create a new ITextFormatValueHandlerManager object, then register handlers according to the provided format map.
        /// </summary>
        /// <param name="formatMap">A dictionary mapping supported formats to format strings. Handlers will be created as dictated by the specified SupportedFormat.</param>
        /// <returns>The newly created object, registered with handlers created according to the supplied format map.</returns>
        public virtual ITextFormatValueHandlerManager CreateTextFormatValueHandlerManagerWithRegisteredHandlers(Dictionary<SupportedFormat, string> formatMap)
        {
            ITextFormatValueHandlerManager manager = this.CreateTextFormatValueHandlerManager();

            foreach (SupportedFormat format in formatMap.Keys)
            {
                ITextFormatValueHandler handler = null;
                switch (format)
                {
                    case SupportedFormat.Colour:
                        handler = CreateColourValueHandler();
                        break;
                    case SupportedFormat.TimePerCharacter:
                        handler = CreateDecimalValueHandler();
                        break;
                    case SupportedFormat.FontName:
                        handler = CreateStringValueHandler();
                        break;
                    case SupportedFormat.LineAlignment:
                        handler = CreateTextLineAlignmentValueHandler();
                        break;
                    default:
                        throw new Exception($"Format, '{format.ToString()}' is currently not supported.");
                }

                manager.RegisterValueHandler(formatMap[format], format, handler);
            }

            return manager;
        }

        /// <summary>
        /// Create a new ITextObjectFactory object.
        /// </summary>
        /// <returns>The newly created object.</returns>
        public virtual ITextObjectFactory CreateTextObjectFactory()
        {
            return new TextObjectFactory();
        }
        #endregion

        #region Format Value Handlers
        /// <summary>
        /// Create a new ITextFormatValueHandler object responsible for handling colour values.
        /// </summary>
        /// <returns>The newly created object.</returns>
        public virtual ITextFormatValueHandler CreateColourValueHandler()
        {
            return new ColourValueHandler();
        }

        /// <summary>
        /// Create a new ITextFormatValueHandler object responsible for handling decimal values.
        /// </summary>
        /// <returns>The newly created object.</returns>
        public virtual ITextFormatValueHandler CreateDecimalValueHandler()
        {
            return new DecimalValueHandler();
        }

        /// <summary>
        /// Create a new ITextFormatValueHandler object responsible for handling string values.
        /// </summary>
        /// <returns>The newly created object.</returns>
        public virtual ITextFormatValueHandler CreateStringValueHandler()
        {
            return new StringValueHandler();
        }

        /// <summary>
        /// Create a new ITextFormatValueHandler object responsible for handling TextLineAlignment values.
        /// </summary>
        /// <returns>The newly created object.</returns>
        public virtual ITextFormatValueHandler CreateTextLineAlignmentValueHandler()
        {
            return new TextLineAlignmentValueHandler();
        }

        /// <summary>
        /// Create a default mapping of supported formats to format strings.
        /// </summary>
        /// <returns>The newly created object.</returns>
        public virtual Dictionary<SupportedFormat, string> CreateDefaultValueHandlerFormatMap()
        {
            Dictionary<SupportedFormat, string> formatMap = new Dictionary<SupportedFormat, string>();

            formatMap.Add(SupportedFormat.Colour, "colour:color:c");
            formatMap.Add(SupportedFormat.TimePerCharacter, "timepercharacter:tpc");
            formatMap.Add(SupportedFormat.FontName, "font:f");
            formatMap.Add(SupportedFormat.LineAlignment, "alignment:a");

            return formatMap;
        }
        #endregion
    }
}
