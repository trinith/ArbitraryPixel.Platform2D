using System;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Text
{
    [TestClass]
    public class TextDefinition_Tests
    {
        private TextDefinition _sut;

        #region Copy Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyCtorWithNullObjectShouldThrowException()
        {
            _sut = new TextDefinition(null);
        }

        [TestMethod]
        public void CopyCtorShouldCopyIsReadOnly()
        {
            ITextDefinition other = Substitute.For<ITextDefinition>();
            other.IsReadOnly.Returns(true);

            _sut = new TextDefinition(other);

            Assert.IsTrue(_sut.IsReadOnly);
        }

        [TestMethod]
        public void CopyCtorWithForceWriteableTrueShouldOverrideOtherIsReadOnly()
        {
            ITextDefinition other = Substitute.For<ITextDefinition>();
            other.IsReadOnly.Returns(true);

            _sut = new TextDefinition(other, true);

            Assert.IsFalse(_sut.IsReadOnly);
        }

        [TestMethod]
        public void CopyCtorShouldCopyProperty_Font()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            ITextDefinition other = Substitute.For<ITextDefinition>();
            other.Font.Returns(mockFont);

            _sut = new TextDefinition(other);

            Assert.AreSame(mockFont, _sut.Font);
        }

        [TestMethod]
        public void CopyCtorShouldCopyProperty_Text()
        {
            ITextDefinition other = Substitute.For<ITextDefinition>();
            other.Text.Returns("blah");

            _sut = new TextDefinition(other);

            Assert.AreEqual<string>("blah", _sut.Text);
        }

        [TestMethod]
        public void CopyCtorShouldCopyProperty_Colour()
        {
            ITextDefinition other = Substitute.For<ITextDefinition>();
            other.Colour.Returns(Color.Pink);

            _sut = new TextDefinition(other);

            Assert.AreEqual<Color>(Color.Pink, _sut.Colour);
        }
        #endregion

        #region Property Tests - Constructor Parameters
        [TestMethod]
        public void TextShouldReturnConstructorValue()
        {
            _sut = new TextDefinition(Substitute.For<ISpriteFont>(), "blah", Color.Pink);
            Assert.AreEqual<string>("blah", _sut.Text);
        }

        [TestMethod]
        public void FontShouldReturnConstructorValue()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _sut = new TextDefinition(mockFont, "blah", Color.Pink);

            Assert.AreSame(mockFont, _sut.Font);
        }

        [TestMethod]
        public void ColourShouldReturnConstructorValue()
        {
            _sut = new TextDefinition(Substitute.For<ISpriteFont>(), "blah", Color.Pink);
            Assert.AreEqual<Color>(Color.Pink, _sut.Colour);
        }
        #endregion

        #region Property Tests - Set then get
        [TestMethod]
        public void TextShouldReturnSetValue()
        {
            _sut = new TextDefinition();

            _sut.Text = "blah";

            Assert.AreEqual<string>("blah", _sut.Text);
        }

        [TestMethod]
        public void FontShouldReturnSetValue()
        {
            _sut = new TextDefinition();

            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _sut.Font = mockFont;

            Assert.AreSame(mockFont, _sut.Font);
        }

        [TestMethod]
        public void ColourShouldReturnSetValue()
        {
            _sut = new TextDefinition();

            _sut.Colour = Color.Pink;

            Assert.AreEqual<Color>(Color.Pink, _sut.Colour);
        }
        #endregion

        #region Property Tests - Set with readonly
        [TestMethod]
        [ExpectedException(typeof(PropertyIsReadOnlyException))]
        public void ReadOnlyPropertySetShouldThrowException_Text()
        {
            _sut = new TextDefinition(Substitute.For<ISpriteFont>(), "blah", Color.Pink, true);
            _sut.Text = "asdf";
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyIsReadOnlyException))]
        public void ReadOnlyPropertySetShouldThrowException_Font()
        {
            _sut = new TextDefinition(Substitute.For<ISpriteFont>(), "blah", Color.Pink, true);
            _sut.Font = Substitute.For<ISpriteFont>();
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyIsReadOnlyException))]
        public void ReadOnlyPropertySetShouldThrowException_Colour()
        {
            _sut = new TextDefinition(Substitute.For<ISpriteFont>(), "blah", Color.Pink, true);
            _sut.Colour = Color.Purple;
        }
        #endregion
    }
}
