using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Theme;

namespace ArbitraryPixel.Platform2D_Tests.Theme
{
    [TestClass]
    public class ThemeBase_Tests
    {
        #region WrapperClass
        private class ThemeBase_Testable : ThemeBase
        {
            public string InjectedAssetPathPrefix { get; set; } = "";

            public override string ThemeID => "TestTheme";
            public override string ObjectID => "TestObject";
            protected override string AssetPathPrefix => this.InjectedAssetPathPrefix;
        }
        #endregion

        private ThemeBase_Testable _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new ThemeBase_Testable();
        }

        [TestMethod]
        public void GetFullAssetNameWithEmptyPrefixShouldGiveExpectedValue()
        {
            Assert.AreEqual<string>(@"TestObject\TestTheme\assetName", _sut.GetFullAssetName("assetName"));
        }

        [TestMethod]
        public void GetFullAssetNameWithPrefixShouldGiveExpectedValue()
        {
            _sut.InjectedAssetPathPrefix = "Prefix";
            Assert.AreEqual<string>(@"Prefix\TestObject\TestTheme\assetName", _sut.GetFullAssetName("assetName"));
        }

        [TestMethod]
        public void GetFullAssetNameWithPrefixEndingInBackslashShouldGiveExpectedValue()
        {
            _sut.InjectedAssetPathPrefix = @"Prefix\";
            Assert.AreEqual<string>(@"Prefix\TestObject\TestTheme\assetName", _sut.GetFullAssetName("assetName"));
        }

        [TestMethod]
        public void GetFullAssetNameWithPrefixEndingInMultipleBackslashesShouldGiveExpectedValue()
        {
            _sut.InjectedAssetPathPrefix = @"Prefix\\";
            Assert.AreEqual<string>(@"Prefix\TestObject\TestTheme\assetName", _sut.GetFullAssetName("assetName"));
        }
    }
}
