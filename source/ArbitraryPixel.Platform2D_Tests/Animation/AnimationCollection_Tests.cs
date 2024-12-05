using System;
using System.Collections.Generic;
using ArbitraryPixel.Platform2D.Animation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.Platform2D_Tests.Animation
{
    [TestClass]
    public class AnimationCollection_Tests
    {
        // Test with floats... because why not?

        private AnimationCollection<float> _sut;

        [TestInitialize]
        public void Initialize()
        {
        }

        private void Construct()
        {
            _sut = new AnimationCollection<float>();
        }

        #region Positive Tests
        // NOTE: Since this class effectively wraps a dictionary, a lot of the tests are gonna run together. Just gonna do the best we can!

        [TestMethod]
        public void AddShouldShowAnimationExistsForSuppliedName()
        {
            Construct();

            _sut.Add("test", Substitute.For<IValueAnimation<float>>());

            Assert.IsTrue(_sut.Contains("test"));
        }

        [TestMethod]
        public void RemoveShouldShowAnimationNotExistsForSuppliedName()
        {
            Construct();
            _sut.Add("test", Substitute.For<IValueAnimation<float>>());

            _sut.Remove("test");

            Assert.IsFalse(_sut.Contains("test"));
        }

        [TestMethod]
        public void AnimationShouldNotExistWhenNoAnimationHasBeenAdded()
        {
            Construct();

            Assert.IsFalse(_sut.Contains("missing"));
        }

        [TestMethod]
        public void AnimationShouldExistForAddedAnimation()
        {
            Construct();

            _sut.Add("test", Substitute.For<IValueAnimation<float>>());

            Assert.IsTrue(_sut.Contains("test"));
        }

        [TestMethod]
        public void IndexGetShouldReturnExpectedAnimationForExistingAnimation()
        {
            Construct();
            IValueAnimation<float> mockAnimation = Substitute.For<IValueAnimation<float>>();
            _sut.Add("test", mockAnimation);

            Assert.AreSame(mockAnimation, _sut["test"]);
        }

        [TestMethod]
        public void RemoveWithExistingAnimationShouldReturnTrue()
        {
            Construct();
            _sut.Add("test", Substitute.For<IValueAnimation<float>>());

            Assert.IsTrue(_sut.Remove("test"));
        }
        #endregion

        #region Fail Tests
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void IndexerGetForAnimationThatDoesNotExistShouldThrowExpectedException()
        {
            Construct();

            var anim = _sut["missing"];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWithExistingAnimationNameShouldThrowExpectedException()
        {
            Construct();
            _sut.Add("test", Substitute.For<IValueAnimation<float>>());

            _sut.Add("test", Substitute.For<IValueAnimation<float>>());
        }

        [TestMethod]
        public void RemoveWithAnimationThatDoesNotExistShouldReturnFalse()
        {
            Construct();

            Assert.IsFalse(_sut.Remove("missing"));
        }
        #endregion
    }
}
