
using NUnit.Framework;
using System;

namespace LinearExpansivityTests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        [TestCase(1, 1, 2)]
        [TestCase(0, 1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(-1, -0, -1)]
        [TestCase(-1, -0, -1)]
        [TestCase(-1, -0, -1)]
        [TestCase(-0, -1, -1)]
        [TestCase(-1, -1, -2)]
       
        public void AddTwoIntegers_WhenCalled_ReturnTheSum(int x, int y, long expectedResult)
        {
            var result = Math.AddTwoIntegers(x, y);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(1, 2, 1)]
        [TestCase(2, 1, 1)]
        [TestCase(-1, 2, 3)]
        [TestCase(-1, -2, 1)]
        [TestCase(-2, -1, 1)]
        [TestCase(-1, -1, 0)]
        [TestCase(1, 1, 0)]
        [TestCase(0, 0, 0)]
        public void SubtractTwoIntegers_WhenCalled_ReturnSubtraction(int x, int y, int expectedResult)
        {
            var result = Math.SubtractTwoIntegers(x, y);
            Assert.AreEqual(expectedResult, result);
        }


    }
public class Math
{
    public static long AddTwoIntegers(int x, int y) { return x + y; }
    public static int SubtractTwoIntegers(int x, int y)
    {
        if (x.Equals(y)) return 0;
        return x > y ? x - y : y - x;
    }
}
}
