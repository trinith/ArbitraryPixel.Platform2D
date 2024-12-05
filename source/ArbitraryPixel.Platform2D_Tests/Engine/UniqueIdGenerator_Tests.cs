using ArbitraryPixel.Common;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Engine
{
    [TestClass]
    public class UniqueIdGenerator_Tests
    {
        private UniqueIdGenerator _sut;
        private IDateTimeFactory _mockFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockFactory = Substitute.For<IDateTimeFactory>();
        }

        private void Construct()
        {
            _sut = new UniqueIdGenerator(_mockFactory);
        }

        [TestMethod]
        public void GenerateUniqueIdShouldReturnExpectedValue()
        {
            string expected = (12345).ToString("X");
            Construct();

            _mockFactory.Now.Ticks.Returns(12345);

            Assert.AreEqual<string>(expected, _sut.GenerateUniqueId());
        }

        [TestMethod]
        public void GenerateUniqueIdWhenTicksHasNotChangedShouldReturnExpectedValue_TestA()
        {
            string expected = (12345).ToString("X") + "-1";
            Construct();

            _mockFactory.Now.Ticks.Returns(12345);
            _sut.GenerateUniqueId();

            Assert.AreEqual<string>(expected, _sut.GenerateUniqueId());
        }

        [TestMethod]
        public void GenerateUniqueIdWhenTicksHasNotChangedShouldReturnExpectedValue_TestB()
        {
            string expected = (12345).ToString("X") + "-2";
            Construct();

            _mockFactory.Now.Ticks.Returns(12345);
            _sut.GenerateUniqueId();
            _sut.GenerateUniqueId();

            Assert.AreEqual<string>(expected, _sut.GenerateUniqueId());
        }

        [TestMethod]
        public void GenerateUniqueIdAfterDuplicateWithTickChangeShouldReturnExpectedValue()
        {
            string expected = (5555).ToString("X");
            Construct();

            _mockFactory.Now.Ticks.Returns(1234);
            _sut.GenerateUniqueId();
            _sut.GenerateUniqueId();
            _mockFactory.Now.Ticks.Returns(5555);

            Assert.AreEqual<string>(expected, _sut.GenerateUniqueId());
        }
    }
}
