using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class GroupTests {
        [TestMethod()]
        public void GroupTest() {
            Group g = new Group();
            Assert.AreEqual(g.Transform, Matrix.GetIdentityMatrix());
            Assert.AreEqual(g.Shapes.Count, 0);
        }

        [TestMethod()]
        public void AddChildrenToGroup() {
            Group g = new Group();
            Shape s = new TestShape();
            g.AddShape(s);
            Assert.AreNotEqual(g.Shapes.Count, 0);
            Assert.IsTrue(g.Shapes.Contains(s));
            Assert.AreEqual(s.Parent, g);
        }

        [TestMethod()]
        public void RayIntersectEmptyGroup() {
            Group g = new Group();
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = g.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod()]
        public void RayIntersectNonEmptyGroup() {
            Group g = new Group();
            Shape s1 = new Sphere();
            Shape s2 = new Sphere();
            s2.Transform = Transformation.Translate(0, 0, -3);
            Shape s3 = new Sphere();
            s3.Transform = Transformation.Translate(5, 0, 0);
            g.AddShape(s1);
            g.AddShape(s2);
            g.AddShape(s3);
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = g.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 4);
            Assert.AreEqual(xs[0].S, s2);
            Assert.AreEqual(xs[1].S, s2);
            Assert.AreEqual(xs[2].S, s1);
            Assert.AreEqual(xs[3].S, s1);
        }

        [TestMethod()]
        public void RayIntersectTransformedGroup() {
            Group g = new Group();
            g.Transform = Transformation.Scale(2, 2, 2);
            Shape s = new Sphere();
            s.Transform = Transformation.Translate(5, 0, 0);
            g.AddShape(s);
            Ray r = new Ray(Tuple.Point(10, 0, -10), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = g.Intersect(r);
            Assert.AreEqual(xs.Count, 2);
        }

        [TestMethod()]
        public void FindNormalOnChild() {
            Group g = new Group();
            g.Transform = Transformation.Scale(2, 2, 2);
            Shape s = new Sphere();
            s.Transform = Transformation.Translate(5, 0, 0);
            g.AddShape(s);
            Ray r = new Ray(Tuple.Point(10, 0, -10), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = g.Intersect(r);
            Assert.AreEqual(xs.Count, 2);
        }
    }
}