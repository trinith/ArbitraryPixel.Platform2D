using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Common;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Text
{
    [TestClass]
    public class TextFormatProcessor_Tests
    {
        private ITextFormatValueHandlerManager _mockManager;
        private EventHandler<ValueEventArgs<Color>> _mockColourSetSubscriber;
        private EventHandler<ValueEventArgs<double>> _mockTPCSetSubscriber;
        private EventHandler<ValueEventArgs<string>> _mockFontNameSetSubscriber;
        private EventHandler<ValueEventArgs<TextLineAlignment>> _mockLineAlignmentSetSubscriber;

        private TextFormatProcessor _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockManager = Substitute.For<ITextFormatValueHandlerManager>();

            _sut = new TextFormatProcessor(_mockManager);

            _sut.ColourFormatSet += _mockColourSetSubscriber = Substitute.For<EventHandler<ValueEventArgs<Color>>>();
            _sut.TimePerCharacterSet += _mockTPCSetSubscriber = Substitute.For<EventHandler<ValueEventArgs<double>>>();
            _sut.FontNameSet += _mockFontNameSetSubscriber = Substitute.For<EventHandler<ValueEventArgs<string>>>();
            _sut.LineAlignmentSet += _mockLineAlignmentSetSubscriber = Substitute.For<EventHandler<ValueEventArgs<TextLineAlignment>>>();
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullManagerShouldThrowException()
        {
            _sut = new TextFormatProcessor(null);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void FormatOpenShouldReturnExpectedCharacter()
        {
            Assert.AreEqual<char>('{', _sut.FormatOpen);
        }

        [TestMethod]
        public void FormatCloseShouldReturnExpectedCharacter()
        {
            Assert.AreEqual<char>('}', _sut.FormatClose);
        }

        [TestMethod]
        public void FormatSeparatorOpenShouldReturnExpectedCharacter()
        {
            Assert.AreEqual<char>(':', _sut.FormatSeparator);
        }

        [TestMethod]
        public void FormatEscapeShouldReturnExpectedCharacter()
        {
            Assert.AreEqual<char>('\\', _sut.FormatEscape);
        }
        #endregion

        #region Process Tests - InvalidFormatException
        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void ProcessWithInvalidFormatShouldThrowException_NoOpenBrace()
        {
            _sut.Process("asdf:1234}");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void ProcessWithInvalidFormatShouldThrowException_NoCloseBrace()
        {
            _sut.Process("{asdf:1234");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void ProcessWithInvalidFormatShouldThrowException_NoSeparator()
        {
            _sut.Process("{asdf1234}");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void ProcessWithInvalidFormatShouldThrowException_NoFormatName()
        {
            _sut.Process("{:1234}");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void ProcessWithInvalidFormatShouldThrowException_NoFormatValue()
        {
            _sut.Process("{asdf:}");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormatException))]
        public void ProcessWithInvalidFormatShouldThrowException_InvalidTokens()
        {
            _sut.Process("{asdf:12:34}");
        }
        #endregion

        #region Process Tests - InvalidFormatNameException
        [TestMethod]
        [ExpectedException(typeof(InvalidFormatNameException))]
        public void ProcessWithInvalidNameShouldThrowException()
        {
            _mockManager.CanHandleFormatName("asdf").Returns(false);

            _sut.Process("{asdf:1234}");
        }
        #endregion

        #region Process Tests - InvalidFormatValueException
        [TestMethod]
        [ExpectedException(typeof(InvalidFormatValueException))]
        public void ProcessWithInvalidValueShouldThrowException()
        {
            _mockManager.CanHandleFormatName("asdf").Returns(true);
            _mockManager.When(x => x.HandleValue(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<FormatValueHandledCallback>())).Do(x => throw new Exception());

            _sut.Process("{asdf:1234}");
        }

        [TestMethod]
        public void ProcessWithInvalidValueShouldThrowExceptionContainingActualException()
        {
            _mockManager.CanHandleFormatName("asdf").Returns(true);
            _mockManager.When(x => x.HandleValue(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<FormatValueHandledCallback>())).Do(x => throw new Exception("I AM A TEST"));

            Assert.ThrowsException<Exception>(
                () =>
                {
                    try
                    {
                        _sut.Process("{asdf:1234}");
                    }
                    catch (Exception e)
                    {
                        throw e.InnerException;
                    }
                },
                "I AM A TEST"
            );
        }
        #endregion

        #region Process Tests
        [TestMethod]
        public void ProcessWithRegisteredHandlerShouldCallManagerHandleValue()
        {
            _mockManager.CanHandleFormatName("asdf").Returns(true);

            _sut.Process("{asdf:1234}");

            _mockManager.Received(1).HandleValue("asdf", "1234", Arg.Any<FormatValueHandledCallback>());
        }

        [TestMethod]
        public void ProcessWithRegisteredHandlerShouldTriggerAppropriateEvent_ColourFormatSet()
        {
            FormatValueHandledCallback callback = null;
            _mockManager.CanHandleFormatName("asdf").Returns(true);
            _mockManager.When(x => x.HandleValue(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<FormatValueHandledCallback>())).Do(x => callback = x[2] as FormatValueHandledCallback);

            _sut.Process("{asdf:1234}");

            callback(SupportedFormat.Colour, Color.Pink);

            _mockColourSetSubscriber.Received(1)(_sut, Arg.Is<ValueEventArgs<Color>>(x => x.Value == Color.Pink));
        }

        [TestMethod]
        public void ProcessWithRegisteredHandlerShouldTriggerAppropriateEvent_TimePerCharacter()
        {
            FormatValueHandledCallback callback = null;
            _mockManager.CanHandleFormatName("asdf").Returns(true);
            _mockManager.When(x => x.HandleValue(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<FormatValueHandledCallback>())).Do(x => callback = x[2] as FormatValueHandledCallback);

            _sut.Process("{asdf:1234}");

            callback(SupportedFormat.TimePerCharacter, Math.PI);

            _mockTPCSetSubscriber.Received(1)(_sut, Arg.Is<ValueEventArgs<double>>(x => x.Value == Math.PI));
        }

        [TestMethod]
        public void ProcessWithRegisteredHandlerShouldTriggerAppropriateEvent_FontName()
        {
            FormatValueHandledCallback callback = null;
            _mockManager.CanHandleFormatName("asdf").Returns(true);
            _mockManager.When(x => x.HandleValue(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<FormatValueHandledCallback>())).Do(x => callback = x[2] as FormatValueHandledCallback);

            _sut.Process("{asdf:1234}");

            callback(SupportedFormat.FontName, "FakeFont");

            _mockFontNameSetSubscriber.Received(1)(_sut, Arg.Is<ValueEventArgs<string>>(x => x.Value == "FakeFont"));
        }

        [TestMethod]
        public void ProcessWithRegisteredHandlerShouldTriggerAppropriateEvent_LineAlignment()
        {
            FormatValueHandledCallback callback = null;
            _mockManager.CanHandleFormatName("asdf").Returns(true);
            _mockManager.When(x => x.HandleValue(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<FormatValueHandledCallback>())).Do(x => callback = x[2] as FormatValueHandledCallback);

            _sut.Process("{asdf:1234}");

            callback(SupportedFormat.LineAlignment, TextLineAlignment.Left);

            _mockLineAlignmentSetSubscriber.Received(1)(_sut, Arg.Is<ValueEventArgs<TextLineAlignment>>(x => x.Value == TextLineAlignment.Left));
        }
        #endregion
    }
}
