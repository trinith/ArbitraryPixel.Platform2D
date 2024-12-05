using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Text;
using NSubstitute;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D_Tests.Text
{
    [TestClass]
    public class TextFormatValueHandlerManager_Tests
    {
        private ITextFormatValueHandler _mockHandler;
        private FormatValueHandledCallback _mockCallback;
        private TextFormatValueHandlerManager _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockHandler = Substitute.For<ITextFormatValueHandler>();
            _mockCallback = Substitute.For<FormatValueHandledCallback>();
            _sut = new TextFormatValueHandlerManager();
        }

        #region CanHandleFormatName Tests
        [TestMethod]
        public void CanHandleFormatNameShouldReturnFalseForUnregisteredName()
        {
            Assert.IsFalse(_sut.CanHandleFormatName("blah"));
        }

        [TestMethod]
        public void CanHandleFormatNameShouldReturnTrueForRegisteredName()
        {
            _sut.RegisterValueHandler("blah", SupportedFormat.Colour, _mockHandler);

            Assert.IsTrue(_sut.CanHandleFormatName("blah"));
        }

        [TestMethod]
        public void CanHandleFormatNameShouldREturnTrueForRegisteredNameWithAnyCase()
        {
            _sut.RegisterValueHandler("blah", SupportedFormat.Colour, _mockHandler);

            Assert.IsTrue(_sut.CanHandleFormatName("BlAh"));
        }
        #endregion

        #region RegisterValueHandler Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterValueHandlerShouldThrowExceptionWithEmptyArgs()
        {
            _sut.RegisterValueHandler("", SupportedFormat.Colour, _mockHandler);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterValueHandlerShouldThrowExceptionWithNullHandler()
        {
            _sut.RegisterValueHandler("blah", SupportedFormat.Colour, null);
        }

        [TestMethod]
        public void RegisterValueHandlerShouldRegisterForSingleName()
        {
            _sut.RegisterValueHandler("blah", SupportedFormat.Colour, _mockHandler);

            _sut.CanHandleFormatName("blah");
        }

        [TestMethod]
        public void RegisterValueHandlerShouldRegisterForSingleNameWithAnyCase()
        {
            _sut.RegisterValueHandler("blAH", SupportedFormat.Colour, _mockHandler);

            _sut.CanHandleFormatName("blah");
        }

        [TestMethod]
        public void RegisterValueHandlerShouldRegisterForMultipleNames_First()
        {
            _sut.RegisterValueHandler("blah:asdf", SupportedFormat.Colour, _mockHandler);

            _sut.CanHandleFormatName("blah");
        }

        [TestMethod]
        public void RegisterValueHandlerShouldRegisterForMultipleNames_Second()
        {
            _sut.RegisterValueHandler("blah:asdf", SupportedFormat.Colour, _mockHandler);

            _sut.CanHandleFormatName("asdf");
        }

        [TestMethod]
        public void RegisterValueHandlerShouldNotThrowExceptionWithEmptyName()
        {
            _sut.RegisterValueHandler("blah::asdf", SupportedFormat.Colour, _mockHandler);
        }

        [TestMethod]
        public void RegisterValueHandlerShouldIgnoreBlankNames()
        {
            _sut.RegisterValueHandler("blah: :asdf", SupportedFormat.Colour, _mockHandler);

            Assert.IsFalse(_sut.CanHandleFormatName(" "));
        }

        [TestMethod]
        public void RegisterValueHandlerWithDuplicateNameShouldNotThrowException()
        {
            _sut.RegisterValueHandler("blah:blah", SupportedFormat.Colour, _mockHandler);
        }
        #endregion

        #region HandleValue Tests
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void HandleValueForUnregisteredNameShouldThrowException()
        {
            _sut.HandleValue("asdf", "1234", null);
        }

        [TestMethod]
        public void HandleValueForRegisteredNameShouldCallHandlerHandleValue_SingleNameRegister_TestA()
        {
            _sut.RegisterValueHandler("asdf", SupportedFormat.Colour, _mockHandler);

            _sut.HandleValue("asdf", "1234", _mockCallback);

            _mockHandler.Received(1).HandleValue(SupportedFormat.Colour, "1234", _mockCallback);
        }

        [TestMethod]
        public void HandleValueForRegisteredNameShouldCallHandlerHandleValue_SingleNameRegister_TestB()
        {
            _sut.RegisterValueHandler("asdf", SupportedFormat.FontName, _mockHandler);

            _sut.HandleValue("asdf", "1234", _mockCallback);

            _mockHandler.Received(1).HandleValue(SupportedFormat.FontName, "1234", _mockCallback);
        }

        [TestMethod]
        public void HandleValueForRegisteredNameShouldCallHandlerHandleValue_MultipleNameRegister_TestA()
        {
            _sut.RegisterValueHandler("a:b", SupportedFormat.Colour, _mockHandler);

            _sut.HandleValue("a", "1234", _mockCallback);

            _mockHandler.Received(1).HandleValue(SupportedFormat.Colour, "1234", _mockCallback);
        }

        [TestMethod]
        public void HandleValueForRegisteredNameShouldCallHandlerHandleValue_MultipleNameRegister_TestB()
        {
            _sut.RegisterValueHandler("a:b", SupportedFormat.Colour, _mockHandler);

            _sut.HandleValue("b", "1234", _mockCallback);

            _mockHandler.Received(1).HandleValue(SupportedFormat.Colour, "1234", _mockCallback);
        }
        #endregion
    }
}
