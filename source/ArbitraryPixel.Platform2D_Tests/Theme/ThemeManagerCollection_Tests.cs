using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Theme;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D_Tests.Theme
{
    [TestClass]
    public class ThemeManagerCollection_Tests
    {
        #region Test Enum
        private class ObjectType
        {
            public const string ObjectA = "ObjectA";
            public const string ObjectB = "ObjectB";
            public const string ObjectThatDoesNotExist = "ObjectThatDoesNotExist";
        }
        #endregion

        private IThemeManager _mockManagerA;
        private IThemeManager _mockManagerB;
        private ThemeManagerCollection _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockManagerA = NSubstitute.Substitute.For<IThemeManager>();
            _mockManagerB = NSubstitute.Substitute.For<IThemeManager>();
            _sut = new ThemeManagerCollection();

            _sut.RegisterManager(ObjectType.ObjectA, _mockManagerA);
            _sut.RegisterManager(ObjectType.ObjectB, _mockManagerB);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void IndexerWithUnregisteredManagerShouldThrowException()
        {
            IThemeManager manager = _sut[ObjectType.ObjectThatDoesNotExist];
        }

        [TestMethod]
        public void IndexerWithRegisteredManagerShouldReturnExpectedManager_ManagerA()
        {
            Assert.AreEqual<IThemeManager>(_mockManagerA, _sut[ObjectType.ObjectA]);
        }

        [TestMethod]
        public void IndexerWithRegisteredManagerShouldReturnExpectedManager_ManagerB()
        {
            Assert.AreEqual<IThemeManager>(_mockManagerB, _sut[ObjectType.ObjectB]);
        }

        [TestMethod]
        public void ManagerExistsWithManagerThatExistsShouldReturnTrue()
        {
            Assert.IsTrue(_sut.ManagerExists(ObjectType.ObjectA));
        }

        [TestMethod]
        public void ManagerExistsWithManagerThatDoesNotExistShouldReturnFalse()
        {
            Assert.IsFalse(_sut.ManagerExists(ObjectType.ObjectThatDoesNotExist));
        }
    }
}
