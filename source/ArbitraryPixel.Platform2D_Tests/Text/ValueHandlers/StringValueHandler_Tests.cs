using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Text.ValueHandlers;
using ArbitraryPixel.Platform2D.Text;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Text.ValueHandlers
{
    [TestClass]
    public class StringValueHandler_Tests
    {
        private StringValueHandler _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new StringValueHandler();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleValueWithNullCallbackShouldThrowException()
        {
            _sut.HandleValue(SupportedFormat.FontName, "abcd", null);
        }

        [TestMethod]
        public void HandleValueShouldProvideValueToCallback()
        {
            FormatValueHandledCallback mockCallback = Substitute.For<FormatValueHandledCallback>();

            _sut.HandleValue(SupportedFormat.FontName, "abcd", mockCallback);

            mockCallback.Received(1)(SupportedFormat.FontName, "abcd");
        }
    }
}
