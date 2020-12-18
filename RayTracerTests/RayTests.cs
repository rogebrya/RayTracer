using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class RayTests {
        [TestMethod()]
        public void RayTest() {
            Tuple origin = Tuple.Point(1, 2, 3);
            Tuple direction = Tuple.Vector(4, 5, 6);
            Ray r = new Ray(origin, direction);
            Assert.AreEqual(r.Origin, origin);
            Assert.AreEqual(r.Direction, direction);
        }

        [TestMethod()]
        public void PointPositionTest() {
            Ray r = new Ray(Tuple.Point(2, 3, 4), Tuple.Vector(1, 0, 0));
            Assert.AreEqual(r.Position(0), Tuple.Point(2, 3, 4));
            Assert.AreEqual(r.Position(1), Tuple.Point(3, 3, 4));
            Assert.AreEqual(r.Position(-1), Tuple.Point(1, 3, 4));
            Assert.AreEqual(r.Position(2.5), Tuple.Point(4.5, 3, 4));
        }

        [TestMethod()]
        public void TranslateRayTest() {
            Ray r = new Ray(Tuple.Point(1, 2, 3), Tuple.Vector(0, 1, 0));
            Matrix m = Transformation.Translate(3, 4, 5);
            Ray r2 = r.Transform(m);
            Assert.AreEqual(r2.Origin, Tuple.Point(4, 6, 8));
            Assert.AreEqual(r2.Direction, Tuple.Vector(0, 1, 0));
        }

        [TestMethod()]
        public void ScaleRayTest() {
            Ray r = new Ray(Tuple.Point(1, 2, 3), Tuple.Vector(0, 1, 0));
            Matrix m = Transformation.Scale(2, 3, 4);
            Ray r2 = r.Transform(m);
            Assert.AreEqual(r2.Origin, Tuple.Point(2, 6, 12));
            Assert.AreEqual(r2.Direction, Tuple.Vector(0, 3, 0));
        }
    }
}