using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class CubeTests {
        [TestMethod()]
        public void RayIntersectsCube() {
            Shape c = new Cube();
            Tuple[,] tuples = new Tuple[7, 2] {
                { Tuple.Point(5, 0.5, 0), Tuple.Vector(-1, 0, 0) },
                { Tuple.Point(-5, 0.5, 0), Tuple.Vector(1, 0, 0) },
                { Tuple.Point(0.5, 5, 0), Tuple.Vector(0, -1, 0) },
                { Tuple.Point(0.5, -5, 0), Tuple.Vector(0, 1, 0) },
                { Tuple.Point(0.5, 0, 5), Tuple.Vector(0, 0, -1) },
                { Tuple.Point(0.5, 0, -5), Tuple.Vector(0, 0, 1) },
                { Tuple.Point(0, 0.5, 0), Tuple.Vector(0, 0, 1) }
            };
            double[,] tValues = new double[7, 2] {
                { 4.0, 6.0 },
                { 4.0, 6.0 },
                { 4.0, 6.0 },
                { 4.0, 6.0 },
                { 4.0, 6.0 },
                { 4.0, 6.0 },
                { -1.0, 1.0 }
            };
            for (int i = 0; i < 7; i++) {
                Ray r = new Ray(tuples[i, 0], tuples[i, 1]);
                List<Intersection> xs = c.LocalIntersect(r);
                Assert.AreEqual(xs[0].T, tValues[i, 0]);
                Assert.AreEqual(xs[1].T, tValues[i, 1]);
            }
        }

        [TestMethod()]
        public void RayMissesCube() {
            Shape c = new Cube();
            Tuple[,] tuples = new Tuple[6, 2] {
                { Tuple.Point(-2, 0, 0), Tuple.Vector(0.2673, 0.5345, 0.8018) },
                { Tuple.Point(0, -2, 0), Tuple.Vector(0.8018, 0.2673, 0.5345) },
                { Tuple.Point(0, 0, -2), Tuple.Vector(0.5345, 0.8018, 0.2673) },
                { Tuple.Point(2, 0, 2), Tuple.Vector(0, 0, -1) },
                { Tuple.Point(0, 2, 2), Tuple.Vector(0, -1, 0) },
                { Tuple.Point(2, 2, 0), Tuple.Vector(-1, 0, 0) }
            };
            for (int i = 0; i < 6; i++) {
                Ray r = new Ray(tuples[i, 0], tuples[i, 1]);
                List<Intersection> xs = c.LocalIntersect(r);
                Assert.AreEqual(xs.Count, 0);
            }
        }

        [TestMethod()]
        public void FindCubeNormals() {
            Shape c = new Cube();
            Tuple[,] tuples = new Tuple[8, 2] {
                { Tuple.Point(1, 0.5, -0.8), Tuple.Vector(1, 0, 0) },
                { Tuple.Point(-1, -0.2, 0.9), Tuple.Vector(-1, 0, 0) },
                { Tuple.Point(-0.4, 1, -0.1), Tuple.Vector(0, 1, 0) },
                { Tuple.Point(0.3, -1, -0.7), Tuple.Vector(0, -1, 0) },
                { Tuple.Point(-0.6, 0.3, 1), Tuple.Vector(0, 0, 1) },
                { Tuple.Point(0.4, 0.4, -1), Tuple.Vector(0, 0, -1) },
                { Tuple.Point(1, 1, 1), Tuple.Vector(1, 0, 0) },
                { Tuple.Point(-1, -1, -1), Tuple.Vector(-1, 0, 0) }
            };
            for (int i = 0; i < 8; i++) {
                Tuple p = tuples[i, 0];
                Tuple normal = c.LocalNormalAt(p, new Intersection(0, c));
                Assert.AreEqual(normal, tuples[i, 1]);
            }
        }
    }
}