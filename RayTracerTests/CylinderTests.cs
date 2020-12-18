using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class CylinderTests {
        private const double EPSILON = 0.0001;
        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }

        [TestMethod()]
        public void RayMissesCylinder() {
            Shape c = new Cylinder();
            Tuple[,] tuples = new Tuple[3, 2] {
                { Tuple.Point(1, 0, 0), Tuple.Vector(0, 1, 0) },
                { Tuple.Point(0, 0, 0), Tuple.Vector(0, 1, 0) },
                { Tuple.Point(0, 0, -5), Tuple.Vector(1, 1, 1) }
            };
            for (int i = 0; i < 3; i++) {
                Ray r = new Ray(tuples[i, 0], tuples[i, 1]);
                List<Intersection> xs = c.LocalIntersect(r);
                Assert.AreEqual(xs.Count, 0);
            }
        }

        [TestMethod()]
        public void RayIntersectsCylinder() {
            Shape c = new Cylinder();
            Tuple[,] tuples = new Tuple[3, 2] {
                { Tuple.Point(1, 0, -5), Tuple.Vector(0, 0, 1) },
                { Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1) },
                { Tuple.Point(0.5, 0, -5), Tuple.Vector(0.1, 1, 1) }
            };
            double[,] tValues = new double[3, 2] {
                { 5.0, 5.0 },
                { 4.0, 6.0 },
                { 6.80798, 7.08872 }
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
        public void FindCylinderNormals() {
            Shape c = new Cylinder();
            Tuple[,] tuples = new Tuple[4, 2] {
                { Tuple.Point(1, 0, 0), Tuple.Vector(1, 0, 0) },
                { Tuple.Point(0, 5, -1), Tuple.Vector(0, 0, -1) },
                { Tuple.Point(0, -2, 1), Tuple.Vector(0, 0, 1) },
                { Tuple.Point(-1, 1, 0), Tuple.Vector(-1, 0, 0) }
            };
            for (int i = 0; i < 4; i++) {
                Tuple normal = c.LocalNormalAt(tuples[i, 0], new Intersection(0, c));
                Assert.AreEqual(normal, tuples[i, 1]);
            }
        }

        [TestMethod()]
        public void MinMaxBounds() {
            Cylinder cyl = new Cylinder();
            Assert.AreEqual(cyl.Minimum, double.MinValue);
            Assert.AreEqual(cyl.Maximum, double.MaxValue);
        }

        [TestMethod()]
        public void IntersectTruncatedCylinder() {
            Cylinder cyl = new Cylinder();
            cyl.Minimum = 1;
            cyl.Maximum = 2;
            Tuple[,] tuples = new Tuple[6, 2] {
                { Tuple.Point(0, 1.5, 0), Tuple.Vector(0.1, 1, 0) },
                { Tuple.Point(0, 3, -5), Tuple.Vector(0, 0, 1) },
                { Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1) },
                { Tuple.Point(0, 2, -5), Tuple.Vector(0, 0, 1) },
                { Tuple.Point(0, 1, -5), Tuple.Vector(0, 0, 1) },
                { Tuple.Point(0, 1.5, -2), Tuple.Vector(0, 0, 1) }
            };
            int[] count = new int[6] {
                0, 0, 0, 0, 0, 2
            };
            for (int i = 0; i < 6; i++) {
                Ray r = new Ray(tuples[i, 0], tuples[i, 1]);
                List<Intersection> xs = cyl.LocalIntersect(r);
                Assert.AreEqual(xs.Count, count[i]);
            }
        }

        [TestMethod()]
        public void Closed() {
            Cylinder cyl = new Cylinder();
            Assert.IsFalse(cyl.IsClosed);
        }

        [TestMethod()]
        public void IntersectCapsOfClosedCylinder() {
            Cylinder cyl = new Cylinder();
            cyl.Minimum = 1;
            cyl.Maximum = 2;
            cyl.IsClosed = true;
            Tuple[,] tuples = new Tuple[5, 2] {
                { Tuple.Point(0, 3, 0), Tuple.Vector(0, -1, 0) },
                { Tuple.Point(0, 3, -2), Tuple.Vector(0, -1, 2) },
                { Tuple.Point(0, 4, -2), Tuple.Vector(0, -1, 1) },
                { Tuple.Point(0, 0, -2), Tuple.Vector(0, 1, 2) },
                { Tuple.Point(0, -1, -2), Tuple.Vector(0, 1, 1) }
            };
            int[] count = new int[5] {
                2, 2, 2, 2, 2
            };
            for (int i = 0; i < 5; i++) {
                Ray r = new Ray(tuples[i, 0], tuples[i, 1]);
                List<Intersection> xs = cyl.LocalIntersect(r);
                Assert.AreEqual(xs.Count, count[i]);
            }
        }

        [TestMethod()]
        public void FindEndCapNormals() {
            Cylinder cyl = new Cylinder();
            cyl.Minimum = 1;
            cyl.Maximum = 2;
            cyl.IsClosed = true;
            Tuple[,] tuples = new Tuple[6, 2] {
                { Tuple.Point(0, 1, 0), Tuple.Vector(0, -1, 0) },
                { Tuple.Point(0.5, 1, 0), Tuple.Vector(0, -1, 0) },
                { Tuple.Point(0, 1, 0.5), Tuple.Vector(0, -1, 0) },
                { Tuple.Point(0, 2, 0), Tuple.Vector(0, 1, 0) },
                { Tuple.Point(0.5, 2, 0), Tuple.Vector(0, 1, 0) },
                { Tuple.Point(0, 2, 0.5), Tuple.Vector(0, 1, 0) }
            };
            for (int i = 0; i < 6; i++) {
                Tuple normal = cyl.LocalNormalAt(tuples[i, 0], new Intersection(0, cyl));
                Assert.AreEqual(normal, tuples[i, 1]);
            }
        }
    }
}