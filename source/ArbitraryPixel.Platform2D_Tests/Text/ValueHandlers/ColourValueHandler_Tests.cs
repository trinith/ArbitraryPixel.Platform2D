using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Text.ValueHandlers;
using ArbitraryPixel.Platform2D.Text;
using NSubstitute;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D_Tests.Text.ValueHandlers
{
    [TestClass]
    public class ColourValueHandler_Tests
    {
        private ColourValueHandler _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new ColourValueHandler();
        }

        #region HandleValue with Null Callback
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleValueWithNullCallbackShouldThrowException()
        {
            _sut.HandleValue(SupportedFormat.Colour, "Blah", null);
        }
        #endregion

        #region HandleValue with Name
        [TestMethod]
        public void HandleValueWithNameShouldSendExpectedColourToCallback()
        {
            // Note: There are lots of colour names we want to support as string. Lets just test one of them... testing them all wouldn't really provide a lot of value >.>

            FormatValueHandledCallback mockCallback = Substitute.For<FormatValueHandledCallback>();

            _sut.HandleValue(SupportedFormat.Colour, "Pink", mockCallback);

            mockCallback.Received(1)(SupportedFormat.Colour, Color.Pink);
        }
        #endregion

        #region HandleValue with RGB
        [TestMethod]
        public void HandleValueWithRGBStringShouldSendExpectedColourToCallback_RGB()
        {
            FormatValueHandledCallback mockCallback = Substitute.For<FormatValueHandledCallback>();

            _sut.HandleValue(SupportedFormat.Colour, "11, 22, 33", mockCallback);

            mockCallback.Received(1)(SupportedFormat.Colour, new Color(11, 22, 33));
        }

        [TestMethod]
        public void HandleValueWithRGBStringShouldSendExpectedColourToCallback_RGBA()
        {
            FormatValueHandledCallback mockCallback = Substitute.For<FormatValueHandledCallback>();

            _sut.HandleValue(SupportedFormat.Colour, "11, 22, 33, 44", mockCallback);

            mockCallback.Received(1)(SupportedFormat.Colour, new Color(11, 22, 33, 44));
        }
        #endregion

        #region HandleValue with Unknown
        [TestMethod]
        [ExpectedException(typeof(InvalidColourValueStringException))]
        public void HandleValueWithUnknownStringFormatShouldThrowException()
        {
            _sut.HandleValue(SupportedFormat.Colour, "11.22.33", Substitute.For<FormatValueHandledCallback>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidColourValueStringException))]
        public void HandleValueWithUnknownColourNameShouldThrowException()
        {
            _sut.HandleValue(SupportedFormat.Colour, "BoogerGreen", Substitute.For<FormatValueHandledCallback>());
        }
        #endregion
    }
}
