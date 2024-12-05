using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Text.ValueHandlers;
using ArbitraryPixel.Platform2D.Text;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Text.ValueHandlers
{
    [TestClass]
    public class TextLineAlignmentValueHandler_Tests
    {
        private TextLineAlignmentValueHandler _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new TextLineAlignmentValueHandler();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleValueWithNullCallbackShouldThrowException()
        {
            _sut.HandleValue(SupportedFormat.LineAlignment, "asdf", null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTextLineAlignmentValueStringException))]
        public void HandleValueWithInvalidStringShouldThrowException()
        {
            _sut.HandleValue(SupportedFormat.LineAlignment, "asdf", Substitute.For<FormatValueHandledCallback>());
        }

        [TestMethod]
        public void HandleValueWithValidStringShouldSendExpectedValueToCallback()
        {
            FormatValueHandledCallback mockCallback = Substitute.For<FormatValueHandledCallback>();

            _sut.HandleValue(SupportedFormat.LineAlignment, "Centre", mockCallback);

            mockCallback.Received(1)(SupportedFormat.LineAlignment, TextLineAlignment.Centre);
        }
    }
}
