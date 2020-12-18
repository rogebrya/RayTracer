using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class ConeTests {
        private const double EPSILON = 0.0001;
        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }

        [TestMethod()]
        public void RayIntersectsCone() {
            Shape c = new Cone();
            Tuple[,] tuples = new Tuple[3, 2] {
                { Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1) },
                { Tuple.Point(0, 0, -5), Tuple.Vector(1, 1, 1) },
                { Tuple.Point(1, 1, -5), Tuple.Vector(-0.5, -1, 1) }
            };
            double[,] tValues = new double[3, 2] {
                { 5.0, 5.0 },
                { 8.66025, 8.66025 },
                { 4.55006, 49.44994 }
            };
            for (int i = 0; i < 3; i++) {
                Ray r = new Ray(tuples[i, 0], Tuple.Normalize(tuples[i, 1]));
                List<Intersection> xs = c.LocalIntersect(r);
                Assert.AreEqual(xs.Count, 2);
                Assert.IsTrue(EqualityOfDouble(xs[0].T, tValues[i, 0]));
                Assert.IsTrue(EqualityOfDouble(xs[1].T, tValues[i, 1]));
            }
        }

        [TestMethod()]
        public void RayIntersectsConeParallelToHalf() {
            Shape c = new Cone();
            Ray r = new Ray(Tuple.Point(0, 0, -1), Tuple.Normalize(Tuple.Vector(0, 1, 1)));
            List<Intersection> xs = c.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 1);
            Assert.IsTrue(EqualityOfDouble(xs[0].T, 0.35355));
        }

        [TestMethod()]
        public void RayIntersectsConeEndCaps() {
            Cone c = new Cone();
            c.Minimum = -0.5;
            c.Maximum = 0.5;
            c.IsClosed = true;
            Tuple[,] tuples = new Tuple[3, 2] {
                { Tuple.Point(0, 0, -5), Tuple.Vector(0, 1, 0) },
                { Tuple.Point(0, 0, -0.25), Tuple.Vector(0, 1, 1) },
                { Tuple.Point(0, 0, -0.25), Tuple.Vector(0, 1, 0) }
            };
            int[] count = new int[3] {
                0, 2, 4
            };
            for (int i = 0; i < 3; i++) {
                Ray r = new Ray(tuples[i, 0], Tuple.Normalize(tuples[i, 1]));
                List<Intersection> xs = c.LocalIntersect(r);
                Assert.AreEqual(xs.Count, count[i]);;
            }
        }

        [TestMethod()]
        public void ConeNormals() {
            Shape c = new Cone();
            Tuple[,] tuples = new Tuple[3, 2] {
                { Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 0) },
                { Tuple.Point(1, 1, 1), Tuple.Vector(1, -Math.Sqrt(2), 1) },
                { Tuple.Point(-1, -1, 0), Tuple.Vector(-1, 1, 0) }
            };
            for (int i = 0; i < 3; i++) {
                Tuple normal = c.LocalNormalAt(tuples[i, 0], new Intersection(0,c));
                Assert.AreEqual(normal, tuples[i, 1]);
            }
        }
    }
}