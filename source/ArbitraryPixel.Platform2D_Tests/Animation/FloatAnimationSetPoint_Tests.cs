using ArbitraryPixel.Platform2D.Animation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitraryPixel.Platform2D_Tests.Animation
{
    [TestClass]
    public class FloatAnimationSetPoint_Tests
    {
        private AnimationSetPoint<float> _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new AnimationSetPoint<float>(5, 3);
        }

        [TestMethod]
        public void ConstructShouldSetPropertyToParameterValue_Target()
        {
            Assert.AreEqual<float>(5, _sut.Target);
        }

        [TestMethod]
        public void ConstructShouldSetPropertyToParameterValue_Time()
        {
            Assert.AreEqual<float>(3, _sut.Time);
        }
    }
}
