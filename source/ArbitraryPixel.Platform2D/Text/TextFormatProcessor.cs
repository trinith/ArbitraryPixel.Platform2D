using ArbitraryPixel.Common;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// An object responsible for processing text format strings.
    /// </summary>
    public class TextFormatProcessor : ITextFormatProcessor
    {
        private ITextFormatValueHandlerManager _handlerManager;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="handlerManager">An object responsible for handling the value managers this object will use when processing format strings.</param>
        public TextFormatProcessor(ITextFormatValueHandlerManager handlerManager)
        {
            _handlerManager = handlerManager ?? throw new ArgumentNullException();
        }

        #region ITextFormatProcessor Implementation
        /// <summary>
        /// A character representing the start of a format sequence.
        /// </summary>
        public char FormatOpen => '{';

        /// <summary>
        /// A character representing the end of a format sequence.
        /// </summary>
        public char FormatClose => '}';

        /// <summary>
        /// A character used to separate items in a format.
        /// </summary>
        public char FormatSeparator => ':';

        /// <summary>
        /// A character used to escape the next character in a string.
        /// </summary>
        public char FormatEscape => '\\';

        /// <summary>
        /// An event that occurs when this object processes a Colour format value.
        /// </summary>
        public event EventHandler<ValueEventArgs<Color>> ColourFormatSet;

        /// <summary>
        /// An event that occurs when this object processes a TimePerCharacter format value.
        /// </summary>
        public event EventHandler<ValueEventArgs<double>> TimePerCharacterSet;

        /// <summary>
        /// An event that occurs when this object processes a FontName format value.
        /// </summary>
        public event EventHandler<ValueEventArgs<string>> FontNameSet;

        /// <summary>
        /// An event that occurs when this object processes a LineAlignment format value.
        /// </summary>
        public event EventHandler<ValueEventArgs<TextLineAlignment>> LineAlignmentSet;

        /// <summary>
        /// Process a format specifier and take the appropriate action.
        /// </summary>
        /// <param name="formatSpecifier">The format specifier to process.</param>
        public void Process(string formatSpecifier)
        {
            if (!formatSpecifier.StartsWith(this.FormatOpen.ToString()) || !formatSpecifier.EndsWith(this.FormatClose.ToString()) || !formatSpecifier.Contains(this.FormatSeparator))
                ThrowInvalidFormatException();

            string keyValue = formatSpecifier.Substring(1, formatSpecifier.Length - 2);
            string[] tokens = keyValue.Split(this.FormatSeparator);

            if (tokens.Length != 2)
                ThrowInvalidFormatException();

            string formatName = tokens[0].Trim().ToLower();
            string valueString = tokens[1].Trim();

            if (string.IsNullOrEmpty(formatName) || string.IsNullOrEmpty(valueString))
                ThrowInvalidFormatException();

            if (!_handlerManager.CanHandleFormatName(formatName))
            {
                ThrowInValidFormatNameException(formatName);
            }
            else
            {
                try
                {
                    _handlerManager.HandleValue(formatName, valueString, SetValue);
                }
                catch (Exception e)
                {
                    throw new InvalidFormatValueException($"The handler for a format name of '{formatName}' has thrown an exception while processing a value of '{valueString}'.", e);
                }
            }
        }
        #endregion

        private void SetValue(SupportedFormat formatName, object value)
        {
            switch (formatName)
            {
                case SupportedFormat.Colour:
                    OnValueSetEvent<Color>(this.ColourFormatSet, (Color)value);
                    break;
                case SupportedFormat.TimePerCharacter:
                    OnValueSetEvent<double>(this.TimePerCharacterSet, (double)value);
                    break;
                case SupportedFormat.FontName:
                    OnValueSetEvent<string>(this.FontNameSet, (string)value);
                    break;
                case SupportedFormat.LineAlignment:
                    OnValueSetEvent<TextLineAlignment>(this.LineAlignmentSet, (TextLineAlignment)value);
                    break;
            }
        }

        private void OnValueSetEvent<T>(EventHandler<ValueEventArgs<T>> handler, T value)
        {
            if (handler != null)
                handler(this, new ValueEventArgs<T>(value));
        }

        private void ThrowInvalidFormatException()
        {
            throw new InvalidFormatException($"Format string must be in the form '{this.FormatOpen}Name{this.FormatSeparator}Value{this.FormatClose}'");
        }

        private void ThrowInValidFormatNameException(string name)
        {
            throw new InvalidFormatNameException($"Format name, '{name}', not supported.");
        }
    }
}
