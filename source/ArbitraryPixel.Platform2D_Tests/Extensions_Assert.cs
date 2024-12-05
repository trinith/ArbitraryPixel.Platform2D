using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ArbitraryPixel.Platform2D_Tests
{
    public static class Extensions_Assert
    {
        public static void AlmostEqual(this Assert assert, float expected, float actual, float tol = 0.00001f)
        {
            if (Math.Abs(expected - actual) > tol)
                throw new AssertFailedException(string.Format("Actual value, {0}, is not within {1} of the expected value, {2}.\n\nNOTE: Exception output values may not reflect actual values. You should probably inspect within the debugger :D", actual, tol, expected));
        }
    }
}
