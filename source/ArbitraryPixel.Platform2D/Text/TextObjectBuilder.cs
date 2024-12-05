using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ArbitraryPixel.Platform2D.Text
{


    /// <summary>
    /// An object responsible for building a list of ITextObjects from a formatted string.
    /// </summary>
    public class TextObjectBuilder : ITextObjectBuilder
    {
        private const double DEFAULT_TIME_PER_CHARACTER = 0;
        private Color DEFAULT_COLOUR = Color.White;

        private ITextFormatProcessor _formatProcessor;
        private ITextObjectFactory _textObjectFactory;

        private Color _currentColour;
        private double _currentTimePerCharacter;
        private ISpriteFont _currentFont;
        private TextLineAlignment _currentAlignment;

        private Dictionary<string, ISpriteFont> _fonts = new Dictionary<string, ISpriteFont>();
        private float _currentLineHeight;
        private float _currentLineWidth;
        private Vector2 _currentPosition;
        private StringBuilder _currentText = new StringBuilder();

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="formatProcessor">An object responisble for processing format strings.</param>
        /// <param name="textObjectFactory">An object responsible for creating ITextObjects.</param>
        public TextObjectBuilder(ITextFormatProcessor formatProcessor, ITextObjectFactory textObjectFactory)
        {
            _formatProcessor = formatProcessor ?? throw new ArgumentNullException();
            _textObjectFactory = textObjectFactory ?? throw new ArgumentNullException();

            _formatProcessor.ColourFormatSet += (sender, e) => _currentColour = e.Value;
            _formatProcessor.TimePerCharacterSet += (sender, e) => _currentTimePerCharacter = e.Value;
            _formatProcessor.LineAlignmentSet += (sender, e) => _currentAlignment = e.Value;
            _formatProcessor.FontNameSet +=
                (sender, e) =>
                {
                    if (e.Value != "" && !_fonts.ContainsKey(e.Value))
                        throw new UnregisteredFontNameException($"A font with the name '{e.Value}' has not been registered with this TextObjectBuilder.");

                    _currentFont = (e.Value == "") ? null : _fonts[e.Value];
                };

            SetDefaults();
        }

        private void SetDefaults()
        {
            _currentColour = DEFAULT_COLOUR;
            _currentTimePerCharacter = DEFAULT_TIME_PER_CHARACTER;
            _currentFont = null;
            _currentAlignment = TextLineAlignment.Left;
        }

        #region ITextObjectBuilder Implementation
        /// <summary>
        /// If set to True, states set by a format string passed to a previous Build call will remain intact for future Build calls. If set to False, these states will be reset to defaults.
        /// </summary>
        public bool PreserveState { get; set; } = false;

        /// <summary>
        /// The default font that this builder will use to build text objects when not font has been specified in the format string.
        /// </summary>
        public ISpriteFont DefaultFont { get; set; } = null;

        /// <summary>
        /// Get the font names registered with this Text Builder.
        /// </summary>
        public string[] RegisteredFontNames
        {
            get { return _fonts.Keys.ToArray(); }
        }

        /// <summary>
        /// Gets a font that has been registered with this builder by name.
        /// </summary>
        /// <param name="name">The name of the font to get.</param>
        /// <returns>The font object, if it has been registered with the supplied name. If not, an exception is thrown.</returns>
        public ISpriteFont GetRegisteredFont(string name)
        {
            return _fonts[name];
        }

        /// <summary>
        /// Register a font with this builder to make that font available for use in a format string.
        /// </summary>
        /// <param name="fontName">The name of the font.</param>
        /// <param name="font">The font object.</param>
        public void RegisterFont(string fontName, ISpriteFont font)
        {
            _fonts.Add(fontName, font);
        }

        /// <summary>
        /// Create a list of ITextObjects given the specified format string and bounds.
        /// </summary>
        /// <param name="formatString">The string specifying the text, and format, to create ITextObjects for.</param>
        /// <param name="bounds">The bounds the text will exist in. If the text is too large, it will spill over bounds.</param>
        /// <returns>A list of ITextObject objects representing the formatted string that was given.</returns>
        public List<ITextObject> Build(string formatString, RectangleF bounds)
        {
            if (this.PreserveState == false)
                SetDefaults();

            List<ITextObject> objects = new List<ITextObject>();

            _currentPosition = bounds.Location;

            formatString = formatString.Replace("\r", "");
            string[] lines = formatString.Split(new char[] { '\n' });
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    _currentLineHeight = GetFont().LineSpacing;
                }
                else
                {
                    objects.AddRange(BuildLine(line, bounds));
                }

                _currentPosition.X = bounds.Left;
                _currentPosition.Y += _currentLineHeight;
                _currentLineHeight = 0;
            }

            return objects;
        }
        #endregion

        private ISpriteFont GetFont()
        {
            return (_currentFont != null) ? _currentFont : this.DefaultFont;
        }

        private List<ITextObject> BuildLine(string line, RectangleF bounds)
        {
            List<ITextObject> objects = new List<ITextObject>();

            _currentText.Clear();
            _currentLineWidth = 0;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == _formatProcessor.FormatEscape)
                {
                    char[] escapeableChars = new char[] { _formatProcessor.FormatEscape, _formatProcessor.FormatOpen, _formatProcessor.FormatClose };
                    if (i == line.Length - 1 || !escapeableChars.Contains(line[i + 1]))
                    {
                        char? c = null;
                        if (i < line.Length - 1)
                            c = line[i + 1];

                        ThrowUnrecognizedEscapeSequenceException(line, c);
                    }
                    else
                    {
                        _currentText.Append(line[i + 1]);
                        i += 1;
                    }
                }
                else if (line[i] == _formatProcessor.FormatOpen)
                {
                    // Changing formats but have some text, so add an object for it.
                    if (!string.IsNullOrEmpty(_currentText.ToString()))
                        objects.Add(CreateTextObject());

                    // Process the new format.
                    int endIndex = line.IndexOf(_formatProcessor.FormatClose, i + 1);
                    string formatString = line.Substring(i, endIndex - i + 1);
                    _formatProcessor.Process(formatString);

                    // Advance counter to end of format string.
                    i += formatString.Length - 1;
                }
                else
                {
                    _currentText.Append(line[i]);
                }
            }

            if (!string.IsNullOrEmpty(_currentText.ToString()))
                objects.Add(CreateTextObject());

            AdjustObjectsForAlignment(objects, bounds, _currentAlignment);

            return objects;
        }

        private void ThrowUnrecognizedEscapeSequenceException(string line, char? escapeChar = null)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append($"Could not build text for line, '{line}' as it contains an unrecognized escape character");

            if (escapeChar == null)
                msg.Append(" at the end of the line.");
            else
                msg.Append($" '{escapeChar.Value}'.");

            throw new UnrecognizedEscapeSeqeunceException(msg.ToString());
        }

        private void AdjustObjectsForAlignment(List<ITextObject> objects, RectangleF bounds, TextLineAlignment alignment)
        {
            // Adjust positions if we aren't left aligned.
            if (_currentAlignment != TextLineAlignment.Left)
            {
                float leftEdge = 0;
                switch (_currentAlignment)
                {
                    case TextLineAlignment.Centre:
                        leftEdge = bounds.Left + bounds.Width / 2 - _currentLineWidth / 2;
                        break;
                    case TextLineAlignment.Right:
                        leftEdge = bounds.Right - _currentLineWidth;
                        break;
                }

                float offset = bounds.Left - leftEdge;
                for (int i = 0; i < objects.Count; i++)
                    objects[i].Location = new Vector2(objects[i].Location.X - offset, objects[i].Location.Y);
            }
        }

        private ITextObject CreateTextObject()
        {
            ITextObject newObject = null;

            if (!string.IsNullOrEmpty(_currentText.ToString()))
            {
                ISpriteFont font = GetFont();
                newObject = _textObjectFactory.Create(font, _currentText.ToString(), _currentPosition, _currentColour, _currentTimePerCharacter);

                Vector2 lineSize = font.MeasureString(_currentText.ToString());
                _currentPosition.X += lineSize.X;
                _currentLineHeight = Math.Max(_currentLineHeight, lineSize.Y);
                _currentLineWidth += lineSize.X;
                _currentText.Clear();
            }

            return newObject;
        }
    }
}
