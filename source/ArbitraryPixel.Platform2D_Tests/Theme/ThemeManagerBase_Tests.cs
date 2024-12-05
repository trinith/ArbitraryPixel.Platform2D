using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Theme;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Theme
{
    [TestClass]
    public class ThemeManagerBase_Tests
    {
        private ITheme _mockThemeA;
        private ITheme _mockThemeB;
        private ThemeManagerBase _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockThemeA = Substitute.For<ITheme>();
            _mockThemeA.ThemeID.Returns("ThemeA");
            _mockThemeB = Substitute.For<ITheme>();
            _mockThemeA.ThemeID.Returns("ThemeB");

            _sut = new ThemeManagerBase();
        }

        [TestMethod]
        public void GetCurrentThemeWithUnregisteredThemeShouldReturnDefaultTheme_DefaultNull()
        {
            _sut.CurrentThemeID = "IDoNotExist";
            Assert.IsNull(_sut.GetCurrentTheme());
        }

        [TestMethod]
        public void GetCurrentThemeWithUnregisteredThemeShouldReturnDefaultTheme_ValidDefault()
        {
            ITheme mockTheme = Substitute.For<ITheme>();
            _sut.DefaultTheme = mockTheme;
            _sut.CurrentThemeID = "IDoNotExist";

            Assert.AreEqual<ITheme>(mockTheme, _sut.GetCurrentTheme());
        }

        [TestMethod]
        public void GetCurrentThemeWithRegisteredThemeShouldReturnExpectedTheme_ThemeA()
        {
            _sut.RegisterTheme(_mockThemeA);
            _sut.RegisterTheme(_mockThemeB);

            _sut.CurrentThemeID = _mockThemeA.ThemeID;

            Assert.AreEqual<ITheme>(_mockThemeA, _sut.GetCurrentTheme());
        }

        [TestMethod]
        public void GetCurrentThemeWithRegisteredThemeShouldReturnExpectedTheme_ThemeB()
        {
            _sut.RegisterTheme(_mockThemeA);
            _sut.RegisterTheme(_mockThemeB);

            _sut.CurrentThemeID = _mockThemeB.ThemeID;

            Assert.AreEqual<ITheme>(_mockThemeB, _sut.GetCurrentTheme());
        }
    }
}
