using Microsoft.Xna.Framework;
using System;
using System.Reflection;

namespace ArbitraryPixel.Platform2D.Text.ValueHandlers
{
    /// <summary>
    /// An object responsible for handling colour values.
    /// </summary>
    public class ColourValueHandler : ITextFormatValueHandler
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

            Color value;

            try
            {
                if (valueString.Contains(","))
                    value = BuildColourFromRGBString(valueString);
                else
                    value = BuildColourFromNameString(valueString);
            }
            catch /* any exception */
            {
                throw new InvalidColourValueStringException($"Could not convert '{valueString}' into a Color object.");
            }

            callback(format, value);
        }

        private Color BuildColourFromRGBString(string rgbString)
        {
            Color c;

            string[] tokens = rgbString.Split(new char[] { ',' });
            c = new Color(byte.Parse(tokens[0]), byte.Parse(tokens[1]), byte.Parse(tokens[2]));
            c.A = (tokens.Length == 4) ? byte.Parse(tokens[3]) : (byte)255;

            return c;
        }

        private Color BuildColourFromNameString(string nameString)
        {
            Type cType = typeof(Color);
            PropertyInfo cProperty = cType.GetRuntimeProperty(nameString);
            Color c = (Color)cProperty.GetValue(null);

            return c;
        }
    }
}
