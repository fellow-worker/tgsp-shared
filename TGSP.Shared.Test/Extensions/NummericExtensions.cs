using TGSP.Shared.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TGSP.Shared.Test.Extensions
{
    [TestClass]
    public class NumericExtensionsTest
    {
        [TestMethod]
        public void DoubleAlmostEqualsTest()
        {
            // define the two doubles
            var d1 = 2d; var d2 = 3d;

            // start simple tests
            Assert.IsFalse(d1.AlmostEquals(d2));
            Assert.IsFalse(d2.AlmostEquals(d1));

            Assert.IsTrue(d1.AlmostEquals(d1));
            Assert.IsTrue(d2.AlmostEquals(d2));

            Assert.IsFalse((d1 * -2).AlmostEquals(d2));
            Assert.IsFalse((d2 * -2).AlmostEquals(d1));
            Assert.IsFalse((d2 * -2).AlmostEquals(d1 * -1));

            Assert.IsTrue((d1 * -2).AlmostEquals(d1 * -2));
            Assert.IsTrue((d2 * -2).AlmostEquals(d2 * -2));

            // tests on the edge
            Assert.IsFalse(d1.AlmostEquals(d1 + NumericExtensions.Delta));
            Assert.IsFalse(d2.AlmostEquals(d2 + NumericExtensions.Delta));
            Assert.IsFalse(d1.AlmostEquals(d1 - NumericExtensions.Delta));
            Assert.IsFalse(d2.AlmostEquals(d2 - NumericExtensions.Delta));

            d1 = d1 * -1;
            d2 = d2 * -1;

            Assert.IsFalse(d1.AlmostEquals(d1 + NumericExtensions.Delta));
            Assert.IsFalse(d2.AlmostEquals(d2 + NumericExtensions.Delta));
            Assert.IsFalse(d1.AlmostEquals(d1 - NumericExtensions.Delta));
            Assert.IsFalse(d2.AlmostEquals(d2 - NumericExtensions.Delta));

            // tests over the edge
            Assert.IsTrue(d1.AlmostEquals(d1 + NumericExtensions.Delta / 8));
            Assert.IsTrue(d2.AlmostEquals(d2 + NumericExtensions.Delta / 8));
            Assert.IsTrue(d1.AlmostEquals(d1 - NumericExtensions.Delta / 8));
            Assert.IsTrue(d2.AlmostEquals(d2 - NumericExtensions.Delta / 8));

            d1 = d1 * -6;
            d2 = d2 * -6;

            Assert.IsTrue(d1.AlmostEquals(d1 + NumericExtensions.Delta / 12));
            Assert.IsTrue(d2.AlmostEquals(d2 + NumericExtensions.Delta / 12));
            Assert.IsTrue(d1.AlmostEquals(d1 - NumericExtensions.Delta / 12));
            Assert.IsTrue(d2.AlmostEquals(d2 - NumericExtensions.Delta / 12));

        }

        [TestMethod]
        public void FloatAlmostEqualsTest()
        {
            // define the two doubles
            var f1 = 1f; var f2 = 2f;

            // start simple tests
            Assert.IsFalse(f1.AlmostEquals(f2));
            Assert.IsFalse(f2.AlmostEquals(f1));

            Assert.IsTrue(f1.AlmostEquals(f1));
            Assert.IsTrue(f2.AlmostEquals(f2));

            Assert.IsFalse((f1 * -1).AlmostEquals(f2));
            Assert.IsFalse((f2 * -1).AlmostEquals(f1));
            Assert.IsFalse((f2 * -1).AlmostEquals(f1 * -1));

            Assert.IsTrue((f1 * -1).AlmostEquals(f1 * -1));
            Assert.IsTrue((f2 * -1).AlmostEquals(f2 * -1));

            // tests on the edge
            Assert.IsFalse(f1.AlmostEquals(f1 + NumericExtensions.DeltaFloat));
            Assert.IsFalse(f2.AlmostEquals(f2 + NumericExtensions.DeltaFloat));
            Assert.IsFalse(f1.AlmostEquals(f1 - NumericExtensions.DeltaFloat));
            Assert.IsFalse(f2.AlmostEquals(f2 - NumericExtensions.DeltaFloat));

            f1 = f1 * -1;
            f2 = f2 * -1;

            Assert.IsFalse(f1.AlmostEquals(f1 + NumericExtensions.DeltaFloat));
            Assert.IsFalse(f2.AlmostEquals(f2 + NumericExtensions.DeltaFloat));
            Assert.IsFalse(f1.AlmostEquals(f1 - NumericExtensions.DeltaFloat));
            Assert.IsFalse(f2.AlmostEquals(f2 - NumericExtensions.DeltaFloat));

            // tests over the edge
            Assert.IsTrue(f1.AlmostEquals(f1 + NumericExtensions.DeltaFloat / 2));
            Assert.IsTrue(f2.AlmostEquals(f2 + NumericExtensions.DeltaFloat / 2));
            Assert.IsTrue(f1.AlmostEquals(f1 - NumericExtensions.DeltaFloat / 2));
            Assert.IsTrue(f2.AlmostEquals(f2 - NumericExtensions.DeltaFloat / 2));

            f1 = f1 * -1;
            f2 = f2 * -1;

            Assert.IsTrue(f1.AlmostEquals(f1 + NumericExtensions.DeltaFloat / 2));
            Assert.IsTrue(f2.AlmostEquals(f2 + NumericExtensions.DeltaFloat / 2));
            Assert.IsTrue(f1.AlmostEquals(f1 - NumericExtensions.DeltaFloat / 2));
            Assert.IsTrue(f2.AlmostEquals(f2 - NumericExtensions.DeltaFloat / 2));
        }

        [TestMethod]
        public void DecimalAlmostEqualsTest()
        {
            // define the two doubles
            var d1 = (decimal)3d; var d2 = (decimal)4d;

            // start simple tests
            Assert.IsFalse(d1.AlmostEquals(d2));
            Assert.IsFalse(d2.AlmostEquals(d1));

            Assert.IsTrue(d1.AlmostEquals(d1));
            Assert.IsTrue(d2.AlmostEquals(d2));

            Assert.IsFalse((d1 * -8).AlmostEquals(d2));
            Assert.IsFalse((d2 * -8).AlmostEquals(d1));
            Assert.IsFalse((d2 * -8).AlmostEquals(d1 * 8));

            Assert.IsTrue((d1 * -9).AlmostEquals(d1 * -9));
            Assert.IsTrue((d2 * -9).AlmostEquals(d2 * -9));

            // tests on the edge
            Assert.IsFalse(d1.AlmostEquals(d1 + NumericExtensions.DeltaDecimal));
            Assert.IsFalse(d2.AlmostEquals(d2 + NumericExtensions.DeltaDecimal));
            Assert.IsFalse(d1.AlmostEquals(d1 - NumericExtensions.DeltaDecimal));
            Assert.IsFalse(d2.AlmostEquals(d2 - NumericExtensions.DeltaDecimal));

            d1 = d1 * -14;
            d2 = d2 * -14;

            Assert.IsFalse(d1.AlmostEquals(d1 + NumericExtensions.DeltaDecimal));
            Assert.IsFalse(d2.AlmostEquals(d2 + NumericExtensions.DeltaDecimal));
            Assert.IsFalse(d1.AlmostEquals(d1 - NumericExtensions.DeltaDecimal));
            Assert.IsFalse(d2.AlmostEquals(d2 - NumericExtensions.DeltaDecimal));

            // tests over the edge
            Assert.IsTrue(d1.AlmostEquals(d1 + NumericExtensions.DeltaDecimal / 23));
            Assert.IsTrue(d2.AlmostEquals(d2 + NumericExtensions.DeltaDecimal / 23));
            Assert.IsTrue(d1.AlmostEquals(d1 - NumericExtensions.DeltaDecimal / 23));
            Assert.IsTrue(d2.AlmostEquals(d2 - NumericExtensions.DeltaDecimal / 23));

            d1 = d1 * -6;
            d2 = d2 * -6;

            Assert.IsTrue(d1.AlmostEquals(d1 + NumericExtensions.DeltaDecimal / 25));
            Assert.IsTrue(d2.AlmostEquals(d2 + NumericExtensions.DeltaDecimal / 25));
            Assert.IsTrue(d1.AlmostEquals(d1 - NumericExtensions.DeltaDecimal / 25));
            Assert.IsTrue(d2.AlmostEquals(d2 - NumericExtensions.DeltaDecimal / 25));

        }
    
        [TestMethod]
        public void ByteEqualsTest()
        {
            var random = new Random();
            var arrayA = new byte[16];
            var arrayB = new byte[16];
            var arrayC = new byte[32];
            var arrayD = new byte[128];

            random.NextBytes(arrayA);
            random.NextBytes(arrayB);
            random.NextBytes(arrayC);
            random.NextBytes(arrayD);

            Assert.IsTrue(arrayA.ByteEquals(arrayA));
            Assert.IsTrue(arrayB.ByteEquals(arrayB));
            Assert.IsTrue(arrayC.ByteEquals(arrayC));
            Assert.IsTrue(arrayD.ByteEquals(arrayD));

            Assert.IsFalse(arrayA.ByteEquals(arrayB));
            Assert.IsFalse(arrayA.ByteEquals(arrayC));
            Assert.IsFalse(arrayA.ByteEquals(arrayD));
        }
    }
}

