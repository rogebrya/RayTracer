using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class SmoothTriangleTests {
        private const double EPSILON = 0.0001;
        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }

        static Tuple p1 = Tuple.Point(0, 1, 0);
        static Tuple p2 = Tuple.Point(-1, 0, 0);
        static Tuple p3 = Tuple.Point(1, 0, 0);
        static Tuple n1 = Tuple.Point(0, 1, 0);
        static Tuple n2 = Tuple.Point(-1, 0, 0);
        static Tuple n3 = Tuple.Point(1, 0, 0);
        SmoothTriangle tri = new SmoothTriangle(p1, p2, p3, n1, n2, n3);

        [TestMethod()]
        public void SmoothTriangleTest() {
            Assert.AreEqual(tri.P1, p1);
            Assert.AreEqual(tri.P2, p2);
            Assert.AreEqual(tri.P3, p3);
            Assert.AreEqual(tri.N1, n1);
            Assert.AreEqual(tri.N2, n2);
            Assert.AreEqual(tri.N3, n3);
        }

        [TestMethod()]
        public void SmoothTriangleIntersection() {
            Ray r = new Ray(Tuple.Point(-0.2, 0.3, -2), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = tri.LocalIntersect(r);
            Assert.IsTrue(EqualityOfDouble(xs[0].U, 0.45));
            Assert.IsTrue(EqualityOfDouble(xs[0].V, 0.25));
        }

        [TestMethod()]
        public void SmoothTriangleNormals() {
            Intersection i = new Intersection(1, tri, 0.45, 0.25);
            Tuple n = tri.NormalAt(Tuple.Point(0, 0, 0), i);
            Assert.AreEqual(n, Tuple.Vector(-0.5547, 0.83205, 0));
        }

        [TestMethod()]
        public void SmoothTrianglePrepComps() {
            Intersection i = new Intersection(1, tri, 0.45, 0.25);
            Ray r = new Ray(Tuple.Point(-0.2, 0.3, -2), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();
            xs.Add(i);
            Computations comps = Computations.PrepareComputations(i, r, xs);
            Assert.AreEqual(comps.normalv, Tuple.Vector(-0.5547, 0.83205, 0));
        }
    }
}