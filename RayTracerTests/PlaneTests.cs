using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class PlaneTests {
        [TestMethod()]
        public void PlaneNormals() {
            Plane p = new Plane();
            Tuple n1 = p.LocalNormalAt(Tuple.Point(0, 0, 0), new Intersection(0, p));
            Tuple n2 = p.LocalNormalAt(Tuple.Point(10, 0, -10), new Intersection(0, p));
            Tuple n3 = p.LocalNormalAt(Tuple.Point(-5, 0, 150), new Intersection(0, p));
            Assert.AreEqual(n1, Tuple.Vector(0, 1, 0));
            Assert.AreEqual(n2, Tuple.Vector(0, 1, 0));
            Assert.AreEqual(n3, Tuple.Vector(0, 1, 0));
        }

        [TestMethod()]
        public void IntersectParallelRay() {
            Plane p = new Plane();
            Ray r = new Ray(Tuple.Point(0, 10, 0), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = p.LocalIntersect(r);
            Assert.IsNull(xs);
        }

        [TestMethod()]
        public void IntersectCoplanarRay() {
            Plane p = new Plane();
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = p.LocalIntersect(r);
            Assert.IsNull(xs);
        }

        [TestMethod()]
        public void IntersectFromAboveRay() {
            Plane p = new Plane();
            Ray r = new Ray(Tuple.Point(0, 1, 0), Tuple.Vector(0, -1, 0));
            List<Intersection> xs = p.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 1);
            Assert.AreEqual(xs[0].T, 1);
            Assert.AreEqual(xs[0].S, p);
        }

        [TestMethod()]
        public void IntersectFromBelowRay() {
            Plane p = new Plane();
            Ray r = new Ray(Tuple.Point(0, -1, 0), Tuple.Vector(0, 1, 0));
            List<Intersection> xs = p.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 1);
            Assert.AreEqual(xs[0].T, 1);
            Assert.AreEqual(xs[0].S, p);
        }
    }
}