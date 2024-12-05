using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArbitraryPixel.Platform2D_Tests.Text
{
    [TestClass]
    public class TextObjectBuilder_Tests
    {
        private ITextFormatProcessor _mockFormatProcessor;
        private ITextObjectFactory _mockTextObjectFactory;

        private ITextObject _lastObjectCreated;

        private ISpriteFont _mockDefaultFont;
        private ISpriteFont _mockFontA;
        private ISpriteFont _mockFontB;

        private RectangleF _bounds = new RectangleF(10, 10, 200, 100);

        private TextObjectBuilder _sut;

        #region Helper Methotds
        private void TestTextObjectCreateAny(int expectedCount)
        {
            _mockTextObjectFactory.Received(expectedCount).Create(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>(), Arg.Any<double>());
        }

        private void StagePreserveStateTest(bool preserveState)
        {
            _mockTextObjectFactory.Create(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>(), Arg.Any<double>()).Returns(
                x =>
                {
                    ITextObject mockTextObject = Substitute.For<ITextObject>();
                    mockTextObject.TextDefinition.Font.Returns((ISpriteFont)x[0]);
                    mockTextObject.TextDefinition.Text.Returns((string)x[1]);
                    mockTextObject.Location.Returns((Vector2)x[2]);
                    mockTextObject.TextDefinition.Colour.Returns((Color)x[3]);
                    mockTextObject.TimePerCharacter.Returns((double)x[4]);

                    return mockTextObject;
                }
            );

            _mockFormatProcessor.When(x => x.Process("{C:Red}")).Do(x => _mockFormatProcessor.ColourFormatSet += Raise.Event<EventHandler<ValueEventArgs<Color>>>(new ValueEventArgs<Color>(Color.Red)));
            _mockFormatProcessor.When(x => x.Process("{TPC:0.25}")).Do(x => _mockFormatProcessor.TimePerCharacterSet += Raise.Event<EventHandler<ValueEventArgs<double>>>(new ValueEventArgs<double>(0.25)));
            _mockFormatProcessor.When(x => x.Process("{Font:Other}")).Do(x => _mockFormatProcessor.FontNameSet += Raise.Event<EventHandler<ValueEventArgs<string>>>(new ValueEventArgs<string>("Other")));
            _mockFormatProcessor.When(x => x.Process("{Alignment:Centre}")).Do(x => _mockFormatProcessor.LineAlignmentSet += Raise.Event<EventHandler<ValueEventArgs<TextLineAlignment>>>(new ValueEventArgs<TextLineAlignment>(TextLineAlignment.Centre)));

            ISpriteFont otherFont = Substitute.For<ISpriteFont>();
            _sut.RegisterFont("Other", otherFont);

            _sut.PreserveState = preserveState;
            var initial = _sut.Build("{C:Red}{TPC:0.25}{Font:Other}{Alignment:Centre}test", new RectangleF(0, 0, 300, 200));

            _mockFormatProcessor.ClearReceivedCalls();
            _mockTextObjectFactory.ClearReceivedCalls();
        }
        #endregion

        [TestInitialize]
        public void Initialize()
        {
            _mockFormatProcessor = Substitute.For<ITextFormatProcessor>();
            _mockFormatProcessor.FormatOpen.Returns('{');
            _mockFormatProcessor.FormatClose.Returns('}');
            _mockFormatProcessor.FormatSeparator.Returns(':');
            _mockFormatProcessor.FormatEscape.Returns('\\');

            _mockTextObjectFactory = Substitute.For<ITextObjectFactory>();
            _mockTextObjectFactory.Create(Arg.Any<ISpriteFont>(), Arg.Any<string>(), Arg.Any<Vector2>(), Arg.Any<Color>(), Arg.Any<double>()).Returns(_lastObjectCreated = Substitute.For<ITextObject>());

            _mockDefaultFont = Substitute.For<ISpriteFont>();
            _mockFontA = Substitute.For<ISpriteFont>();
            _mockFontB = Substitute.For<ISpriteFont>();

            _sut = new TextObjectBuilder(_mockFormatProcessor, _mockTextObjectFactory);
            _sut.DefaultFont = _mockDefaultFont;
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullFormatProcessorShouldThrowException()
        {
            _sut = new TextObjectBuilder(null, _mockTextObjectFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullTextObjectFactoryShouldThrowException()
        {
            _sut = new TextObjectBuilder(_mockFormatProcessor, null);
        }

        [TestMethod]
        public void ConstructShouldAttachToProcessorEvent_ColourFormatSet()
        {
            _mockFormatProcessor.Received(1).ColourFormatSet += Arg.Any<EventHandler<ValueEventArgs<Color>>>();
        }

        [TestMethod]
        public void ConstructShouldAttachToProcessorEvent_TimePerCharacterSet()
        {
            _mockFormatProcessor.Received(1).TimePerCharacterSet += Arg.Any<EventHandler<ValueEventArgs<double>>>();
        }

        [TestMethod]
        public void ConstructShouldAttachToProcessorEvent_FontNameSet()
        {
            _mockFormatProcessor.Received(1).FontNameSet += Arg.Any<EventHandler<ValueEventArgs<string>>>();
        }
        #endregion

        #region RegisterFont Tests
        [TestMethod]
        public void RegisterFontShouldNotThrowException()
        {
            _sut.RegisterFont("asdf", _mockFontA);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterDuplicateFontNameShouldThrowException()
        {
            _sut.RegisterFont("asdf", _mockFontA);
            _sut.RegisterFont("asdf", _mockFontB);
        }
        #endregion

        #region RegisteredFontNames Tests
        [TestMethod]
        public void RegisteredFontNameWithNoRegisteredFontsReturnsEmptyArray()
        {
            Assert.IsTrue(_sut.RegisteredFontNames.SequenceEqual(new string[] { }));
        }

        [TestMethod]
        public void RegisteredFontNamesReturnsExpectedValue_SingleFont()
        {
            _sut.RegisterFont("MyFont", Substitute.For<ISpriteFont>());

            Assert.IsTrue(_sut.RegisteredFontNames.SequenceEqual(new string[] { "MyFont" }));
        }

        [TestMethod]
        public void RegisteredFontNamesReturnsExpectedValue_MultipleFonts()
        {
            _sut.RegisterFont("FontA", Substitute.For<ISpriteFont>());
            _sut.RegisterFont("FontB", Substitute.For<ISpriteFont>());

            Assert.IsTrue(_sut.RegisteredFontNames.SequenceEqual(new string[] { "FontA", "FontB" }));
        }
        #endregion

        #region GetRegisteredFont Tests
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetRegisteredFontWithUnregisteredNameShouldThrowException()
        {
            _sut.GetRegisteredFont("IDoNotExist");
        }

        [TestMethod]
        public void GetRegisteredFontReturnsExpectedFont()
        {
            ISpriteFont expected = Substitute.For<ISpriteFont>();
            _sut.RegisterFont("MyFont", expected);

            Assert.AreSame(expected, _sut.GetRegisteredFont("MyFont"));
        }
        #endregion

        #region Build Tests
        #region Simple Formats
        [TestMethod]
        public void BuildWithNoFormatsShouldCreateOnlyOneTextObject()
        {
            _sut.Build("Test text", _bounds);

            TestTextObjectCreateAny(1);
        }

        [TestMethod]
        public void BuildWithNoFormatsShouldCreateExpectedTextObject()
        {
            _sut.Build("Test text", _bounds);

            _mockTextObjectFactory.Received(1).Create(_mockDefaultFont, "Test text", _bounds.Location, Color.White, 0);
        }

        [TestMethod]
        public void BuildWithColourFormatShouldCreateOnlyOneTextObject()
        {
            _mockFormatProcessor.When(x => x.Process("{C:Pink}")).Do(x =>_mockFormatProcessor.ColourFormatSet += Raise.Event<EventHandler<ValueEventArgs<Color>>>(new ValueEventArgs<Color>(Color.Pink)));
            _sut.Build("{C:Pink}Test text", _bounds);

            TestTextObjectCreateAny(1);
        }

        [TestMethod]
        public void BuildWithColourFormatShouldCreateExpectedTextObject()
        {
            _mockFormatProcessor.When(x => x.Process("{C:Pink}")).Do(x => _mockFormatProcessor.ColourFormatSet += Raise.Event<EventHandler<ValueEventArgs<Color>>>(new ValueEventArgs<Color>(Color.Pink)));
            _sut.Build("{C:Pink}Test text", _bounds);

            _mockTextObjectFactory.Received(1).Create(_mockDefaultFont, "Test text", _bounds.Location, Color.Pink, 0);
        }

        [TestMethod]
        public void BuildWithTimePerCharacterFormatShouldCreateOnlyOneTextObject()
        {
            _mockFormatProcessor.When(x => x.Process("{TPC:123}")).Do(x => _mockFormatProcessor.TimePerCharacterSet += Raise.Event<EventHandler<ValueEventArgs<double>>>(new ValueEventArgs<double>(123)));
            _sut.Build("{TPC:123}Test text", _bounds);

            TestTextObjectCreateAny(1);
        }

        [TestMethod]
        public void BuildWithTimePerCharacterFormatShouldCreateExpectedTextObject()
        {
            _mockFormatProcessor.When(x => x.Process("{TPC:123}")).Do(x => _mockFormatProcessor.TimePerCharacterSet += Raise.Event<EventHandler<ValueEventArgs<double>>>(new ValueEventArgs<double>(123)));
            _sut.Build("{TPC:123}Test text", _bounds);

            _mockTextObjectFactory.Received(1).Create(_mockDefaultFont, "Test text", _bounds.Location, Color.White, 123);
        }

        [TestMethod]
        public void BuildWithFontNameFormatShouldCreateOnlyOneTextObject()
        {
            _sut.RegisterFont("asdf", _mockFontA);
            _mockFormatProcessor.When(x => x.Process("{Font:asdf}")).Do(x => _mockFormatProcessor.FontNameSet += Raise.Event<EventHandler<ValueEventArgs<string>>>(new ValueEventArgs<string>("asdf")));
            _sut.Build("{Font:asdf}Test text", _bounds);

            TestTextObjectCreateAny(1);
        }

        [TestMethod]
        public void BuildWithFontNameFormatShouldCreateExpectedObject()
        {
            _sut.RegisterFont("asdf", _mockFontA);
            _mockFormatProcessor.When(x => x.Process("{Font:asdf}")).Do(x => _mockFormatProcessor.FontNameSet += Raise.Event<EventHandler<ValueEventArgs<string>>>(new ValueEventArgs<string>("asdf")));
            _sut.Build("{Font:asdf}Test text", _bounds);

            _mockTextObjectFactory.Received(1).Create(_mockFontA, "Test text", _bounds.Location, Color.White, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(UnregisteredFontNameException))]
        public void BuildWithInvalidFontShouldThrowException()
        {
            try
            {
                _mockFormatProcessor.When(x => x.Process("{Font:asdf}")).Do(x => _mockFormatProcessor.FontNameSet += Raise.Event<EventHandler<ValueEventArgs<string>>>(new ValueEventArgs<string>("asdf")));
                _sut.Build("{Font:asdf}Test text", _bounds);
            }
            catch (InvalidFormatValueException e)
            {
                throw e.InnerException;
            }
        }

        [TestMethod]
        public void BuildWithCentreAlignmentShouldCreateExpectedObject()
        {
            _mockDefaultFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(10, 10));
            _mockFormatProcessor.When(x => x.Process("{Alignment:Centre}")).Do(x => _mockFormatProcessor.LineAlignmentSet += Raise.Event<EventHandler<ValueEventArgs<TextLineAlignment>>>(new ValueEventArgs<TextLineAlignment>(TextLineAlignment.Centre)));

            _sut.Build("{Alignment:Centre}Test", _bounds);

            _lastObjectCreated.Received(1).Location = Arg.Is<Vector2>(x => x.X == 95);
        }

        [TestMethod]
        public void BuildWithRightAlignmentShouldCreateExpectedObject()
        {
            _mockDefaultFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(10, 10));
            _mockFormatProcessor.When(x => x.Process("{Alignment:Right}")).Do(x => _mockFormatProcessor.LineAlignmentSet += Raise.Event<EventHandler<ValueEventArgs<TextLineAlignment>>>(new ValueEventArgs<TextLineAlignment>(TextLineAlignment.Right)));

            _sut.Build("{Alignment:Right}Test", _bounds);

            _lastObjectCreated.Received(1).Location = Arg.Is<Vector2>(x => x.X == 190);
        }

        [TestMethod]
        public void BuildWithMultipleAlignmentSetsShouldUseLastSetAlignmentToCreateExpectedObject()
        {
            _mockDefaultFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(10, 10));
            _mockFormatProcessor.When(x => x.Process("{Alignment:Centre}")).Do(x => _mockFormatProcessor.LineAlignmentSet += Raise.Event<EventHandler<ValueEventArgs<TextLineAlignment>>>(new ValueEventArgs<TextLineAlignment>(TextLineAlignment.Centre)));
            _mockFormatProcessor.When(x => x.Process("{Alignment:Right}")).Do(x => _mockFormatProcessor.LineAlignmentSet += Raise.Event<EventHandler<ValueEventArgs<TextLineAlignment>>>(new ValueEventArgs<TextLineAlignment>(TextLineAlignment.Right)));

            _sut.Build("{Alignment:Centre}{Alignment:Right}Test", _bounds);

            _lastObjectCreated.Received(1).Location = Arg.Is<Vector2>(x => x.X == 190);
        }
        #endregion

        #region Complex Formats
        [TestMethod]
        public void BuildWithMultipleFormatsShouldCreateExpectedTextObjectCount()
        {
            _mockDefaultFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 10));
            _mockFormatProcessor.When(x => x.Process("{c:Pink}")).Do(x => _mockFormatProcessor.ColourFormatSet += Raise.EventWith<ValueEventArgs<Color>>(new ValueEventArgs<Color>(Color.Pink)));
            _mockFormatProcessor.When(x => x.Process("{c:Blue}")).Do(x => _mockFormatProcessor.ColourFormatSet += Raise.EventWith<ValueEventArgs<Color>>(new ValueEventArgs<Color>(Color.Blue)));

            _sut.Build("{c:Pink}This is a {c:Blue}test!", _bounds);

            TestTextObjectCreateAny(2);
        }

        [TestMethod]
        public void BuildWithMultipleFormatsShouldCreateExpectedTextObjects_FirstObject()
        {
            _mockDefaultFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 10));
            _mockFormatProcessor.When(x => x.Process("{c:Pink}")).Do(x => _mockFormatProcessor.ColourFormatSet += Raise.EventWith<ValueEventArgs<Color>>(new ValueEventArgs<Color>(Color.Pink)));
            _mockFormatProcessor.When(x => x.Process("{c:Blue}")).Do(x => _mockFormatProcessor.ColourFormatSet += Raise.EventWith<ValueEventArgs<Color>>(new ValueEventArgs<Color>(Color.Blue)));

            _sut.Build("{c:Pink}This is a {c:Blue}test!", _bounds);

            _mockTextObjectFactory.Received(1).Create(_mockDefaultFont, "This is a ", _bounds.Location, Color.Pink, 0);
        }

        [TestMethod]
        public void BuildWithMultipleFormatsShouldCreateExpectedTextObjects_SecondObject()
        {
            _mockDefaultFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 10));
            _mockFormatProcessor.When(x => x.Process("{c:Pink}")).Do(x => _mockFormatProcessor.ColourFormatSet += Raise.EventWith<ValueEventArgs<Color>>(new ValueEventArgs<Color>(Color.Pink)));
            _mockFormatProcessor.When(x => x.Process("{c:Blue}")).Do(x => _mockFormatProcessor.ColourFormatSet += Raise.EventWith<ValueEventArgs<Color>>(new ValueEventArgs<Color>(Color.Blue)));

            _sut.Build("{c:Pink}This is a {c:Blue}test!", _bounds);

            _mockTextObjectFactory.Received(1).Create(_mockDefaultFont, "test!", _bounds.Location + new Vector2(100, 0), Color.Blue, 0);
        }
        #endregion

        #region Multiple Lines
        [TestMethod]
        public void BuildWithMultipleLinesShouldCreateExpectedTextObjectCount()
        {
            _mockDefaultFont.LineSpacing.Returns(15);
            _sut.Build("Line1\nLine2", _bounds);

            TestTextObjectCreateAny(2);
        }

        [TestMethod]
        public void BuildWithMultipleLinesShouldCreateExpectedTextObject_FirstObject()
        {
            _mockDefaultFont.LineSpacing.Returns(15);
            _sut.Build("Line1\nLine2", _bounds);

            _mockTextObjectFactory.Received(1).Create(_mockDefaultFont, "Line1", _bounds.Location, Color.White, 0);
        }

        [TestMethod]
        public void BuildWithMultipleLinesShouldCreateExpectedTextObject_SecondObject()
        {
            _mockDefaultFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 15));
            _sut.Build("Line1\nLine2", _bounds);

            _mockTextObjectFactory.Received(1).Create(_mockDefaultFont, "Line2", _bounds.Location + new Vector2(0, 15), Color.White, 0);
        }

        [TestMethod]
        public void BuildWithMultipleLinesUsingEmptyLineShouldCreateExpectedTextObject_SecondObject()
        {
            _mockDefaultFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 15));
            _mockDefaultFont.LineSpacing.Returns(15);
            _sut.Build("Line1\n\nLine2", _bounds);

            _mockTextObjectFactory.Received(1).Create(_mockDefaultFont, "Line2", _bounds.Location + new Vector2(0, 30), Color.White, 0);
        }

        [TestMethod]
        public void BuildWithMultipleLinesUsingWindowsLineBreakShouldCreateExpectedTextObjects()
        {
            StringBuilder testFormat = new StringBuilder();
            testFormat.AppendLine("Line1");
            testFormat.AppendLine();
            testFormat.AppendLine("Line2");

            _mockDefaultFont.MeasureString(Arg.Any<string>()).Returns(new SizeF(100, 15));
            _mockDefaultFont.LineSpacing.Returns(15);
            _sut.Build(testFormat.ToString(), _bounds);

            TestTextObjectCreateAny(2);
        }
        #endregion

        #region Escape Tests
        [TestMethod]
        public void BuildWithEscapedOpenCharacterShouldCreateExpectedObject()
        {
            _sut.Build("te\\{st", _bounds);

            _mockTextObjectFactory.Received(1).Create(Arg.Any<ISpriteFont>(), "te{st", Arg.Any<Vector2>(), Arg.Any<Color>(), Arg.Any<double>());
        }

        [TestMethod]
        public void BuildWithEscapedCloseCharacterShouldCreateExpectedObject()
        {
            _sut.Build("te\\}st", _bounds);

            _mockTextObjectFactory.Received(1).Create(Arg.Any<ISpriteFont>(), "te}st", Arg.Any<Vector2>(), Arg.Any<Color>(), Arg.Any<double>());
        }

        [TestMethod]
        public void BuildWithEscapedEscapeCharacterShouldCreateExpectedObject()
        {
            _sut.Build("te\\\\st", _bounds);

            _mockTextObjectFactory.Received(1).Create(Arg.Any<ISpriteFont>(), "te\\st", Arg.Any<Vector2>(), Arg.Any<Color>(), Arg.Any<double>());
        }

        [TestMethod]
        [ExpectedException(typeof(UnrecognizedEscapeSeqeunceException))]
        public void BuildWithEscapeAtEndOfStringShouldThrowException()
        {
            _sut.Build("blah\\", _bounds);
        }

        [TestMethod]
        [ExpectedException(typeof(UnrecognizedEscapeSeqeunceException))]
        public void BuildWithInvalidEscapeCharacterShouldThrowException()
        {
            _sut.Build("bl\\ah", _bounds);
        }
        #endregion
        #endregion

        #region Preserve State Tests
        [TestMethod]
        public void PreserveStateShouldDefaultToExpectedValue()
        {
            Assert.IsFalse(_sut.PreserveState);
        }

        [TestMethod]
        public void PreserveStateShouldReturnSetValue()
        {
            _sut.PreserveState = true;

            Assert.IsTrue(_sut.PreserveState);
        }

        [TestMethod]
        public void BuildWithPreserveStateTrueShouldUseStatesFromPreviousBuild_Colour()
        {
            StagePreserveStateTest(true);

            var result = _sut.Build("blah", new RectangleF(0, 0, 300, 200));

            Assert.AreEqual<Color>(Color.Red, result[0].TextDefinition.Colour);
        }

        [TestMethod]
        public void BuildWithPreserveStateTrueShouldUseStatesFromPreviousBuild_TimePerCharacter()
        {
            StagePreserveStateTest(true);

            var result = _sut.Build("blah", new RectangleF(0, 0, 300, 200));

            Assert.AreEqual<double>(0.25, result[0].TimePerCharacter);
        }

        [TestMethod]
        public void BuildWithPreserveStateTrueShouldUseStatesFromPreviousBuild_Font()
        {
            StagePreserveStateTest(true);
            ISpriteFont expectedFont = _sut.GetRegisteredFont("Other");

            var result = _sut.Build("blah", new RectangleF(0, 0, 300, 200));

            Assert.AreSame(expectedFont, result[0].TextDefinition.Font);
        }

        [TestMethod]
        public void BuildWithPreserveStateTrueShouldUseStatesFromPreviousBuild_Alignment()
        {
            StagePreserveStateTest(true);
            RectangleF bounds = new RectangleF(0, 0, 300, 200);
            _sut.GetRegisteredFont("Other").MeasureString("blah").Returns(new SizeF(50, 20));

            var result = _sut.Build("blah", bounds);

            Vector2 expectedLocation = new Vector2(bounds.Centre.X - 50f / 2f, 0);

            Assert.AreEqual<Vector2>(expectedLocation, result[0].Location);
        }

        [TestMethod]
        public void BuildWithPreserveStateFalseShouldResetStates_Colour()
        {
            StagePreserveStateTest(false);

            var result = _sut.Build("blah", new RectangleF(0, 0, 300, 200));

            Assert.AreEqual<Color>(Color.White, result[0].TextDefinition.Colour);
        }

        [TestMethod]
        public void BuildWithPreserveStateFalseShouldResetStates_TimePerCharacter()
        {
            StagePreserveStateTest(false);

            var result = _sut.Build("blah", new RectangleF(0, 0, 300, 200));

            Assert.AreEqual<double>(0, result[0].TimePerCharacter);
        }

        [TestMethod]
        public void BuildWithPreserveStateFalseShouldResetStates_Font()
        {
            StagePreserveStateTest(false);

            var result = _sut.Build("blah", new RectangleF(0, 0, 300, 200));

            Assert.AreSame(_mockDefaultFont, result[0].TextDefinition.Font);
        }

        [TestMethod]
        public void BuildWithPreserveStateFalseShouldResetStates_Alignment()
        {
            StagePreserveStateTest(false);

            var result = _sut.Build("blah", new RectangleF(0, 0, 300, 200));

            Assert.AreEqual<Vector2>(new Vector2(0, 0), result[0].Location);
        }
        #endregion
    }
}
