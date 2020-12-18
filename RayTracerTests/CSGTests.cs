using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class CSGTests {
        private const double EPSILON = 0.0001;
        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }

        [TestMethod()]
        public void CSGTest() {
            Shape s1 = new Sphere();
            Shape s2 = new Cube();
            CSG c = new CSG("union", s1, s2);
            Assert.AreEqual(c.Operation, "union");
            Assert.AreEqual(c.Left, s1);
            Assert.AreEqual(c.Right, s2);
            Assert.AreEqual(s1.Parent, c);
            Assert.AreEqual(s2.Parent, c);
        }

        [TestMethod()]
        public void UnionOperation() {
            bool[,] bools = new bool[8, 4] {
                //lhit  inl   inr   result
                { true, true, true, false },
                { true, true, false, true },
                { true, false, true, false },
                { true, false, false, true },
                { false, true, true, false },
                { false, true, false, false },
                { false, false, true, true },
                { false, false, false, true }
            };
            for (int i = 0; i < 8; i++) {
                bool result = CSG.IntersectionAllowed("union", bools[i, 0], bools[i, 1], bools[i, 2]);
                Assert.AreEqual(result, bools[i, 3]);
            }
        }

        [TestMethod()]
        public void IntersectionOperation() {
            bool[,] bools = new bool[8, 4] {
                //lhit  inl   inr   result
                { true, true, true, true },
                { true, true, false, false },
                { true, false, true, true },
                { true, false, false, false },
                { false, true, true, true },
                { false, true, false, true },
                { false, false, true, false },
                { false, false, false, false }
            };
            for (int i = 0; i < 8; i++) {
                bool result = CSG.IntersectionAllowed("intersection", bools[i, 0], bools[i, 1], bools[i, 2]);
                Assert.AreEqual(result, bools[i, 3]);
            }
        }

        [TestMethod()]
        public void DifferenceOperation() {
            bool[,] bools = new bool[8, 4] {
                //lhit  inl   inr   result
                { true, true, true, false },
                { true, true, false, true },
                { true, false, true, false },
                { true, false, false, true },
                { false, true, true, true },
                { false, true, false, true },
                { false, false, true, false },
                { false, false, false, false }
            };
            for (int i = 0; i < 8; i++) {
                bool result = CSG.IntersectionAllowed("difference", bools[i, 0], bools[i, 1], bools[i, 2]);
                Assert.AreEqual(result, bools[i, 3]);
            }
        }

        [TestMethod()]
        public void FilterIntersectionList() {
            string[] str = new string[3] { "union", "intersection", "difference" };
            int[,] intersectionIndices = new int[3, 2] {
                //lhit  inl   inr   result
                { 0, 3 },
                { 1, 2 },
                { 0, 1 }
            };
            Shape s1 = new Sphere();
            Shape s2 = new Cube();
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(1, s1));
            xs.Add(new Intersection(2, s2));
            xs.Add(new Intersection(3, s1));
            xs.Add(new Intersection(4, s2));
            for (int i = 0; i < 3; i++) {
                CSG c = new CSG(str[i], s1, s2);
                List<Intersection> result = c.FilterIntersections(xs);
                Assert.AreEqual(result.Count, 2);
                Assert.AreEqual(result[0], xs[intersectionIndices[i, 0]]);
                Assert.AreEqual(result[1], xs[intersectionIndices[i, 1]]);
            }
        }

        [TestMethod()]
        public void RayMissesCSG() {
            CSG c = new CSG("union", new Sphere(), new Cube());
            Ray r = new Ray(Tuple.Point(0, 2, -5), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = c.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod()]
        public void RayHitsCSG() {
            Shape s1 = new Sphere();
            Shape s2 = new Sphere();
            s2.Transform = Transformation.Translate(0, 0, 0.5);
            CSG c = new CSG("union", s1, s2);
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = c.LocalIntersect(r);
            Assert.AreEqual(xs.Count, 2);
            Assert.IsTrue(EqualityOfDouble(xs[0].T, 4));
            Assert.AreEqual(xs[0].S, s1);
            Assert.IsTrue(EqualityOfDouble(xs[1].T, 6.5));
            Assert.AreEqual(xs[1].S, s2);
        }
    }
}