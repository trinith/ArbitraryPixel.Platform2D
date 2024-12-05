using System;
using System.Collections.Generic;
using ArbitraryPixel.Platform2D.Animation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Animation
{
    [TestClass]
    public class FloatValueAnimation_Tests
    {
        public FloatValueAnimation _sut;

        [TestInitialize]
        public void Init()
        {
            _sut = new FloatValueAnimation(1f, CreatePoints());
            _sut.IsLooping = true;
        }

        private IEnumerable<IAnimationSetPoint<float>> CreatePoints()
        {
            IAnimationSetPoint<float>[] points = new IAnimationSetPoint<float>[]
            {
                Substitute.For<IAnimationSetPoint<float>>(),
                Substitute.For<IAnimationSetPoint<float>>(),
            };

            points[0].Target.Returns(2);
            points[0].Time.Returns(2);

            points[1].Target.Returns(1.5f);
            points[1].Time.Returns(1);

            return points;
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SetPoints()
        {
            _sut = new FloatValueAnimation(123, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithSetPointSequenceContainingNullShouldThrowException_TestA()
        {
            _sut = new FloatValueAnimation(
                123,
                new IAnimationSetPoint<float>[]
                {
                    null,
                    Substitute.For<IAnimationSetPoint<float>>(),
                }
            );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructWithSetPointSequenceContainingNullShouldThrowException_TestB()
        {
            _sut = new FloatValueAnimation(
                123,
                new IAnimationSetPoint<float>[]
                {
                    Substitute.For<IAnimationSetPoint<float>>(),
                    null,
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetValueToStartValue()
        {
            Assert.AreEqual<float>(1f, _sut.Value);
        }

        [TestMethod]
        public void ConstructShouldCreateSetFirstSetPointIndexToStartValueWithZeroTime()
        {
            Assert.IsTrue(
                true
                && _sut.SetPoints[0].Time == 0f
                && _sut.SetPoints[0].Target == 1f
            );
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWithElapsedTimeBeforeCurrentSetPointTimeShouldUpdateValuesAsExpected_TestA()
        {
            // Update for 1/2x target, test values
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1)));

            Assert.AreEqual<float>(0.5f, _sut.Factor);
            Assert.AreEqual<float>(MathHelper.SmoothStep(1, 2, 0.5f), _sut.Value);
            Assert.AreEqual<int>(0, _sut.CurrentSetPoint);
            Assert.IsFalse(_sut.IsComplete);
        }

        [TestMethod]
        public void UpdateWithElapsedTimeBeforeCurrentSetPointTimeShouldUpdateValuesAsExpected_TestB()
        {
            // Update for 3/4x target, test values
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1.5f)));

            Assert.AreEqual<float>(0.75f, _sut.Factor);
            Assert.AreEqual<float>(MathHelper.SmoothStep(1, 2, 0.75f), _sut.Value);
            Assert.AreEqual<int>(0, _sut.CurrentSetPoint);
            Assert.IsFalse(_sut.IsComplete);
        }

        [TestMethod]
        public void UpdateWithElapsedTimeAfterCurrentSetPointTimeShouldUpdateValuesAsExpected_TestA()
        {
            // Update for 1x target, test values
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2f)));

            Assert.AreEqual<float>(1f, _sut.Factor);
            Assert.AreEqual<float>(MathHelper.SmoothStep(1, 2, 1f), _sut.Value);
            Assert.AreEqual<int>(0, _sut.CurrentSetPoint);
            Assert.IsFalse(_sut.IsComplete);
        }

        [TestMethod]
        public void UpdateWithElapsedTimeAfterCurrentSetPointTimeShouldUpdateValuesAsExpected_TestB()
        {
            // Update for 1.5x target, test values
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f)));

            Assert.AreEqual<float>(1f, _sut.Factor);
            Assert.AreEqual<float>(MathHelper.SmoothStep(1, 2, 1f), _sut.Value);
            Assert.AreEqual<int>(0, _sut.CurrentSetPoint);
            Assert.IsFalse(_sut.IsComplete);
        }

        [TestMethod]
        public void UpdateForNextSetPointShouldSetValuesAsExpected()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f)));
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5f)));

            Assert.AreEqual<float>(0.5f, _sut.Factor);
            Assert.AreEqual<float>(MathHelper.SmoothStep(2f, 1.5f, 0.5f), _sut.Value);
            Assert.AreEqual<int>(1, _sut.CurrentSetPoint);
            Assert.IsFalse(_sut.IsComplete);
        }

        [TestMethod]
        public void UpdateWhenCurrentSetPointGreaterThanSetPointCountAndNotLoopingShouldSetExpectedValues()
        {
            _sut.IsLooping = false;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // To max at setpoint 0
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // Advance to setpoint 1 and max out
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // Advance to setpoint 2 and max out
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // Does not advance

            Assert.AreEqual<float>(1, _sut.Factor);
            Assert.AreEqual<float>(1.5f, _sut.Value);
            Assert.AreEqual<int>(2, _sut.CurrentSetPoint);
            Assert.IsTrue(_sut.IsComplete);
        }

        [TestMethod]
        public void UpdateAfterFactorReachesOneShouldAdvanceToNextSetPoint()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f)));   // To max at setpoint 0
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.1f))); // Advance to setpoint 1 and work a little

            Assert.AreEqual<int>(1, _sut.CurrentSetPoint);
        }

        [TestMethod]
        public void UpdateAfterFactorReachesOneWithLoopingAndSetPointsAtEndShouldSetCurrentSetPointToZero()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // To max at setpoint 0
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // Advance to setpoint 1 and max out
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5f))); // Advance to setpoint 2, which should reset, then do a little work.

            Assert.AreEqual<float>(0.25f, _sut.Factor);
            Assert.AreEqual<float>(MathHelper.SmoothStep(1, 2, 0.25f), _sut.Value);
            Assert.AreEqual<int>(0, _sut.CurrentSetPoint);
            Assert.IsFalse(_sut.IsComplete);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldSetValueToStartValue()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // Do some work

            _sut.Reset();

            Assert.AreEqual<float>(1f, _sut.Value);
        }

        [TestMethod]
        public void ResetShouldSetCurrentSetPointToExpectedValue()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // Do some work

            _sut.Reset();

            Assert.AreEqual<int>(0, _sut.CurrentSetPoint);
        }

        [TestMethod]
        public void ResetWhenCompleteShouldSetIsCompleteToFalse()
        {
            _sut.IsLooping = false;

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // To max at setpoint 0
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // Advance to setpoint 1 and max out
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(3f))); // Advance to setpoint 2 and max out

            _sut.Reset();

            Assert.IsFalse(_sut.IsComplete);
        }

        [TestMethod]
        public void ResetAfterUpdateShouldSetFactorToZero()
        {
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5f))); // Do some work

            _sut.Reset();

            Assert.AreEqual<float>(0, _sut.Factor);
        }
        #endregion
    }
}
