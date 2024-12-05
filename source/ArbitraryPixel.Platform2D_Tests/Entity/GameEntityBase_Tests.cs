using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using NSubstitute;
using ArbitraryPixel.Common.Drawing;

namespace ArbitraryPixel.Platform2D_Tests.Entity
{
    [TestClass]
    public class GameEntityBase_Tests
    {
        private RectangleF _bounds = RectangleF.Empty;
        private IEngine _mockEngine = null;
        private GameEntityBase _sut = null;

        [TestInitialize]
        public void Initialize()
        {
            _bounds = new RectangleF(0, 0, 100, 100);
            _mockEngine = Substitute.For<IEngine>();
            _sut = new GameEntityBase(_mockEngine, _bounds);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullHostShouldThrowException()
        {
            _sut = new GameEntityBase(null, _bounds);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithEmptyBoundsShouldThrowException()
        {
            _sut = new GameEntityBase(_mockEngine, RectangleF.Empty);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void BoundsShouldReturnSetValue_TestA()
        {
            _sut.Bounds = new RectangleF(0, 0, 400, 400);

            Assert.AreEqual<RectangleF>(new RectangleF(0, 0, 400, 400), _sut.Bounds);
        }

        [TestMethod]
        public void BoundsShouldReturnSetValue_TestB()
        {
            _sut.Bounds = new RectangleF(100, 100, 250, 175);

            Assert.AreEqual<RectangleF>(new RectangleF(100, 100, 250, 175), _sut.Bounds);
        }
        #endregion
    }
}
