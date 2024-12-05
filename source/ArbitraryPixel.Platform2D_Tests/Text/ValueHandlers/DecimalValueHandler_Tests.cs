using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.Text.ValueHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace ArbitraryPixel.Platform2D_Tests.Text.ValueHandlers
{
    [TestClass]
    public class DecimalValueHandler_Tests
    {
        private DecimalValueHandler _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new DecimalValueHandler();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleValueWithNullCallbackShouldThrowException()
        {
            _sut.HandleValue(SupportedFormat.TimePerCharacter, "123", null);
        }

        [TestMethod]
        public void HandleValueWithValidDoubleShouldSendExpectedValueToCallback_WithDecimals()
        {
            FormatValueHandledCallback mockCallback = Substitute.For<FormatValueHandledCallback>();

            _sut.HandleValue(SupportedFormat.TimePerCharacter, "1.23", mockCallback);

            mockCallback.Received(1)(SupportedFormat.TimePerCharacter, 1.23);
        }

        [TestMethod]
        public void HandleValueWithValidDoubleShouldSendExpectedValueToCallback_WithoutDecimals()
        {
            FormatValueHandledCallback mockCallback = Substitute.For<FormatValueHandledCallback>();

            _sut.HandleValue(SupportedFormat.TimePerCharacter, "123", mockCallback);

            mockCallback.Received(1)(SupportedFormat.TimePerCharacter, (double)123);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void HandleValueWithInvalidDoubleShouldThrowException()
        {
            _sut.HandleValue(SupportedFormat.TimePerCharacter, "abcd", Substitute.For<FormatValueHandledCallback>());
        }
    }
}
