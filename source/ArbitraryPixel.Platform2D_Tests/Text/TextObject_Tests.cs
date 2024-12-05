using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Common.Graphics;
using NSubstitute;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D_Tests.Text
{
    [TestClass]
    public class TextObject_Tests
    {
        private ISpriteFont _mockSpriteFont;
        private TextObject _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockSpriteFont = Substitute.For<ISpriteFont>();
            _sut = new TextObject(_mockSpriteFont, "Test", new Vector2(100, 200), Color.Pink, 0.5);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullFontShouldThrowException()
        {
            _sut = new TextObject(null, "Test", Vector2.Zero, Color.Pink, 0.5);
        }

        [TestMethod]
        public void ConstructWithTPCNotZeroShouldSetShowLengthToZero()
        {
            _sut = new TextObject(_mockSpriteFont, "Test", Vector2.Zero, Color.Pink, 0.5);
            Assert.AreEqual<int>(0, _sut.ShowLength);
        }

        [TestMethod]
        public void ConstructWithTPCZeroShouldSetLengthToFullLength()
        {
            _sut = new TextObject(_mockSpriteFont, "Test", Vector2.Zero, Color.Pink, 0);
            Assert.AreEqual<int>(4, _sut.ShowLength);
        }

        [TestMethod]
        public void ConstructorShouldCreateTextDefinitionThatIsReadOnly()
        {
            Assert.IsTrue(_sut.TextDefinition.IsReadOnly);
        }
        #endregion

        #region Property Tests - Parameters
        [TestMethod]
        public void ColourShouldReturnValueFromConstructor()
        {
            Assert.AreEqual<Color>(Color.Pink, _sut.TextDefinition.Colour);
        }

        [TestMethod]
        public void FontShouldReturnValueFromConstructor()
        {
            Assert.AreSame(_mockSpriteFont, _sut.TextDefinition.Font);
        }

        [TestMethod]
        public void LocationShouldReturnValueFromConstructor()
        {
            Assert.AreEqual<Vector2>(new Vector2(100, 200), _sut.Location);
        }

        [TestMethod]
        public void FullTextShouldReturnValueFromConstructor()
        {
            Assert.AreEqual<string>("Test", _sut.TextDefinition.Text);
        }

        [TestMethod]
        public void TimePerCharacterShouldReturnValueFromConstructor()
        {
            Assert.AreEqual<double>(0.5, _sut.TimePerCharacter);
        }

        [TestMethod]
        public void TimePerCharacterShouldReturnSetValue()
        {
            _sut.TimePerCharacter = 12;
            Assert.AreEqual<double>(12, _sut.TimePerCharacter);
        }
        #endregion

        #region Property Tests - Calculated
        [TestMethod]
        public void ShowLengthShouldReturnSetValue()
        {
            Assert.AreEqual<int>(3, _sut.ShowLength = 3);
        }

        [TestMethod]
        public void ShowLengthClampsToExpectedLowerBound()
        {
            _sut.ShowLength = -10;
            Assert.AreEqual<int>(0, _sut.ShowLength);
        }

        [TestMethod]
        public void ShowLengthClampsToExpectedUpperBound()
        {
            _sut.ShowLength = 10;
            Assert.AreEqual<int>(4, _sut.ShowLength);
        }

        [TestMethod]
        public void TextShouldReturnSubstringOfFullTextBasedOnShowLength_Test1()
        {
            _sut.ShowLength = 4;
            Assert.AreEqual<string>("Test", _sut.CurrentText);
        }

        [TestMethod]
        public void TextShouldReturnSubstringOfFullTextBasedOnShowLength_Test2()
        {
            _sut.ShowLength = 2;

            Assert.AreEqual<string>("Te", _sut.CurrentText);
        }

        [TestMethod]
        public void IsCompleteShouldDefaultFalse()
        {
            Assert.IsFalse(_sut.IsComplete);
        }

        [TestMethod]
        public void IsCompleteShouldReturnFalse_ShowLengthAndTimePerCharacterCheckFalse()
        {
            _sut.ShowLength = 3;
            _sut.TimePerCharacter = 0.5;

            Assert.IsFalse(_sut.IsComplete);
        }

        [TestMethod]
        public void IsCompleteShouldReturnTrue_ShowLengthCheckFalse_TimePerCharacterCheckTrue()
        {
            _sut.ShowLength = 3;
            _sut.TimePerCharacter = 0;

            Assert.IsTrue(_sut.IsComplete);
        }

        [TestMethod]
        public void IsCompleteShouldReturnTrue_ShowLengthCheckTrue_TimePerCharacterCheckFalse()
        {
            _sut.ShowLength = 4;
            _sut.TimePerCharacter = 0.3;

            Assert.IsTrue(_sut.IsComplete);
        }

        [TestMethod]
        public void IsCompleteShouldReturnTrue_ShowLengthCheckTrue_TimePerCharacterCheckTrue()
        {
            _sut.ShowLength = 4;
            _sut.TimePerCharacter = 0;

            Assert.IsTrue(_sut.IsComplete);
        }
        #endregion
    }
}
