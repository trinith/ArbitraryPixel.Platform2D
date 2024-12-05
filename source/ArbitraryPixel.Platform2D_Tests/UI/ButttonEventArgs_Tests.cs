using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.Platform2D_Tests.UI
{
    [TestClass]
    public class ButttonEventArgs_Tests
    {
        [TestMethod]
        public void TouchLocationShouldHaveValueSetFromConstructor_TestA()
        {
            ButtonEventArgs sut = new ButtonEventArgs(new Vector2(1, 2));

            Assert.AreEqual<Vector2>(new Vector2(1, 2), sut.Location);
        }

        [TestMethod]
        public void TouchLocationShouldHaveValueSetFromConstructor_TestB()
        {
            ButtonEventArgs sut = new ButtonEventArgs(new Vector2(3, 4));

            Assert.AreEqual<Vector2>(new Vector2(3, 4), sut.Location);
        }
    }
}
