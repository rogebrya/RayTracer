using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class TriangleTests {
        private const double EPSILON = 0.0001;
        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }

        [TestMethod()]
        public void TriangleTest() {
            Tuple p1 = Tuple.Point(0, 1, 0);
            Tuple p2 = Tuple.Point(-1, 0, 0);
            Tuple p3 = Tuple.Point(1, 0, 0);
            Triangle t = new Triangle(p1, p2, p3);
            Assert.AreEqual(t.P1, p1);
            Assert.AreEqual(t.P2, p2);
            Assert.AreEqual(t.P3, p3);
            Assert.AreEqual(t.E1, Tuple.Vector(-1, -1, 0));
            Assert.AreEqual(t.E2, Tuple.Vector(1, -1, 0));
            Assert.AreEqual(t.Normal, Tuple.Vector(0, 0, -1));
        }

        [TestMethod()]
        public void TriangleNormal() {
            Triangle t = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            Tuple n1 = t.LocalNormalAt(Tuple.Point(0, 0.5, 0), new Intersection(0, t));
            Tuple n2 = t.LocalNormalAt(Tuple.Point(-0.5, 0.75, 0), new Intersection(0, t));
            Tuple n3 = t.LocalNormalAt(Tuple.Point(0.5, 0.25, 0), new Intersection(0, t));
            Assert.AreEqual(n1, t.Normal);
            Assert.AreEqual(n2, t.Normal);
            Assert.AreEqual(n3, t.Normal);
        }

        [TestMethod()]
        public void RayMissesTriangle() {
            Triangle t = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            Ray r = new Ray(Tuple.Point(0, -1, -2), Tuple.Point(0, 1, 0));
            List<Intersection> xs = t.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod()]
        public void RayMissesTriangleP1P3Edge() {
            Triangle t = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            Ray r = new Ray(Tuple.Point(1, 1, -2), Tuple.Point(0, 0, 1));
            List<Intersection> xs = t.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod()]
        public void RayMissesTriangleP1P2Edge() {
            Triangle t = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            Ray r = new Ray(Tuple.Point(-1, 1, -2), Tuple.Point(0, 0, 1));
            List<Intersection> xs = t.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod()]
        public void RayMissesTriangleP2P3Edge() {
            Triangle t = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            Ray r = new Ray(Tuple.Point(0, -1, -2), Tuple.Point(0, 0, 1));
            List<Intersection> xs = t.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod()]
        public void RayHitsTriangle() {
            Triangle t = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            Ray r = new Ray(Tuple.Point(0, 0.5, -2), Tuple.Point(0, 0, 1));
            List<Intersection> xs = t.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 1);
            Assert.IsTrue(EqualityOfDouble(xs[0].T, 2.0));
        }
    }
}