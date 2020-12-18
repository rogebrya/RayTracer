using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class SphereTests {
        [TestMethod()]
        public void SphereIntersectRay() {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 2);
            Assert.AreEqual(xs[0].T, 4.0);
            Assert.AreEqual(xs[1].T, 6.0);
        }

        [TestMethod()]
        public void SphereTangentRay() {
            Ray r = new Ray(Tuple.Point(0, 1, -5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 2);
            Assert.AreEqual(xs[0].T, 5.0);
            Assert.AreEqual(xs[1].T, 5.0);
        }

        [TestMethod()]
        public void SphereMissRay() {
            Ray r = new Ray(Tuple.Point(0, 2, -5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod()]
        public void SphereInteriorRay() {
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 2);
            Assert.AreEqual(xs[0].T, -1.0);
            Assert.AreEqual(xs[1].T, 1.0);
        }

        [TestMethod()]
        public void SphereBehindRay() {
            Ray r = new Ray(Tuple.Point(0, 0, 5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 2);
            Assert.AreEqual(xs[0].T, -6.0);
            Assert.AreEqual(xs[1].T, -4.0);
        }

        [TestMethod()]
        public void SphereRayIntersection() {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 2);
            Assert.AreEqual(xs[0].S, s);
            Assert.AreEqual(xs[1].S, s);
        }

        [TestMethod()]
        public void SphereNormalX() {
            Sphere s = new Sphere();
            Tuple n = s.LocalNormalAt(Tuple.Point(1, 0, 0), new Intersection(0, s));
            Assert.AreEqual(n, Tuple.Vector(1, 0, 0));
        }

        [TestMethod()]
        public void SphereNormalY() {
            Sphere s = new Sphere();
            Tuple n = s.LocalNormalAt(Tuple.Point(0, 1, 0), new Intersection(0, s));
            Assert.AreEqual(n, Tuple.Vector(0, 1, 0));
        }

        [TestMethod()]
        public void SphereNormalZ() {
            Sphere s = new Sphere();
            Tuple n = s.LocalNormalAt(Tuple.Point(0, 0, 1), new Intersection(0, s));
            Assert.AreEqual(n, Tuple.Vector(0, 0, 1));
        }

        [TestMethod()]
        public void SphereNormalOther() {
            Sphere s = new Sphere();
            Tuple n = s.LocalNormalAt(Tuple.Point(Math.Sqrt(3)/3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3), new Intersection(0, s));
            Assert.AreEqual(n, Tuple.Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));
        }

        [TestMethod()]
        public void SphereNormalNormalized() {
            Sphere s = new Sphere();
            Tuple n = s.LocalNormalAt(Tuple.Point(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3), new Intersection(0, s));
            Assert.AreEqual(n, Tuple.Normalize(n));
        }
    }
}