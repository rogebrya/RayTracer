using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class IntersectionTests {
        private const double EPSILON = 0.0001;
        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }

        [TestMethod()]
        public void IntersectionTest() {
            Sphere s = new Sphere();
            Intersection i = new Intersection(3.5, s);
            Assert.AreEqual(i.T, 3.5);
            Assert.AreEqual(i.S, s);
        }

        [TestMethod()]
        public void IntersectionAggregate() {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(1, s);
            Intersection i2 = new Intersection(2, s);
            List<Intersection> xs = Intersection.Intersections(i1, i2);
            Assert.AreEqual(xs.Count, 2);
            Assert.AreEqual(i1.T, 1);
            Assert.AreEqual(i2.T, 2);
        }

        [TestMethod()]
        public void IntersectionHitPP() {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(1, s);
            Intersection i2 = new Intersection(2, s);
            List<Intersection> xs = Intersection.Intersections(i2, i1);
            Intersection i = Intersection.Hit(xs);
            Assert.AreEqual(i, i1);
        }

        [TestMethod()]
        public void IntersectionHitPN() {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(-1, s);
            Intersection i2 = new Intersection(1, s);
            List<Intersection> xs = Intersection.Intersections(i2, i1);
            Intersection i = Intersection.Hit(xs);
            Assert.AreEqual(i, i2);
        }

        [TestMethod()]
        public void IntersectionHitNN() {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(-2, s);
            Intersection i2 = new Intersection(-1, s);
            List<Intersection> xs = Intersection.Intersections(i2, i1);
            Intersection i = Intersection.Hit(xs);
            Assert.IsNull(i);
        }

        [TestMethod()]
        public void IntersectionHitLowestP() {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(5, s);
            Intersection i2 = new Intersection(7, s);
            Intersection i3 = new Intersection(-3, s);
            Intersection i4 = new Intersection(2, s);
            List<Intersection> xs = Intersection.Intersections(i1, i2);
            Intersection.Intersections(xs, i3);
            Intersection.Intersections(xs, i4);
            Intersection i = Intersection.Hit(xs);
            Assert.AreEqual(i, i4);
        }

        [TestMethod()]
        public void PrepareComputations() {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Sphere shape = new Sphere();
            Intersection i = new Intersection(4, shape);
            Computations comps = Computations.PrepareComputations(i, r);
            Assert.AreEqual(comps.t, i.T);
            Assert.AreEqual(comps.shape, i.S);
            Assert.AreEqual(comps.point, Tuple.Point(0, 0, -1));
            Assert.AreEqual(comps.eyev, Tuple.Vector(0, 0, -1));
            Assert.AreEqual(comps.normalv, Tuple.Vector(0, 0, -1));
        }

        [TestMethod()]
        public void PrepareComputationsOutside() {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Sphere shape = new Sphere();
            Intersection i = new Intersection(4, shape);
            Computations comps = Computations.PrepareComputations(i, r);
            Assert.IsFalse(comps.inside);
        }

        [TestMethod()]
        public void PrepareComputationsInside() {
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            Sphere shape = new Sphere();
            Intersection i = new Intersection(1, shape);
            Computations comps = Computations.PrepareComputations(i, r);
            Assert.AreEqual(comps.point, Tuple.Point(0, 0, 1));
            Assert.AreEqual(comps.eyev, Tuple.Vector(0, 0, -1));
            Assert.IsTrue(comps.inside);
            Assert.AreEqual(comps.normalv, Tuple.Vector(0, 0, -1));
        }

        [TestMethod()]
        public void HitOffsetsPoint() {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Sphere shape = new Sphere();
            shape.Transform = Transformation.Translate(0, 0, 1);
            Intersection i = new Intersection(5, shape);
            Computations comps = Computations.PrepareComputations(i, r);
            Assert.IsTrue(comps.overPoint.Z < -EPSILON/2);
            Assert.IsTrue(comps.point.Z > comps.overPoint.Z);
        }

        [TestMethod()]
        public void PrecomputeReflectionVector() {
            Plane shape = new Plane();
            Ray r = new Ray(Tuple.Point(0, 1, -1), Tuple.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            Intersection i = new Intersection(Math.Sqrt(2), shape);
            Computations comps = Computations.PrepareComputations(i, r);
            Assert.AreEqual(comps.reflectv, Tuple.Vector(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        }

        [TestMethod()]
        public void FindN1AndN2AtSeveralIntersections() {
            Sphere a = Sphere.GlassSphere();
            a.Transform = Transformation.Scale(2, 2, 2);
            a.Material.RefractiveIndex = 1.5;
            Sphere b = Sphere.GlassSphere();
            b.Transform = Transformation.Translate(0, 0, -0.25);
            b.Material.RefractiveIndex = 2.0;
            Sphere c = Sphere.GlassSphere();
            c.Transform = Transformation.Translate(0, 0, 0.25);
            c.Material.RefractiveIndex = 2.5;
            Ray r = new Ray(Tuple.Point(0, 0, -4), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>(){ 
                new Intersection(2, a), 
                new Intersection(2.75, b), 
                new Intersection(3.25, c),
                new Intersection(4.75, b),
                new Intersection(5.25, c),
                new Intersection(6, a)
            };
            double[,] targets = new double[6, 2] { 
                { 1.0, 1.5 },
                { 1.5, 2.0 },
                { 2.0, 2.5 },
                { 2.5, 2.5 },
                { 2.5, 1.5 },
                { 1.5, 1.0 }
            };

            for (int i = 0; i < 6; i++) {
                Computations comps = Computations.PrepareComputations(xs[i], r, xs);
                Assert.AreEqual(comps.n1, targets[i, 0]);
                Assert.AreEqual(comps.n2, targets[i, 1]);
            }
        }

        [TestMethod()]
        public void UnderPoint() {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Sphere shape = Sphere.GlassSphere();
            shape.Transform = Transformation.Translate(0, 0, 1);
            Intersection i = new Intersection(5, shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(i);
            Computations comps = Computations.PrepareComputations(i, r, xs);
            Assert.IsTrue(comps.underPoint.Z > EPSILON / 2);
            Assert.IsTrue(comps.point.Z < comps.underPoint.Z);
        }

        [TestMethod()]
        public void SchlickUnderTotalInternalReflection() {
            Shape shape = Sphere.GlassSphere();
            Ray r = new Ray(Tuple.Point(0, 0, Math.Sqrt(2) / 2), Tuple.Vector(0, 1, 0));
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(-Math.Sqrt(2) / 2, shape));
            xs.Add(new Intersection(Math.Sqrt(2) / 2, shape));
            Computations comps = Computations.PrepareComputations(xs[1], r, xs);
            double reflectance = Intersection.Schlick(comps);
            Assert.IsTrue(EqualityOfDouble(reflectance, 1.0));
        }

        [TestMethod()]
        public void SchlickWithPerpendicularRay() {
            Shape shape = Sphere.GlassSphere();
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 1, 0));
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(-1, shape));
            xs.Add(new Intersection(1, shape));
            Computations comps = Computations.PrepareComputations(xs[1], r, xs);
            double reflectance = Intersection.Schlick(comps);
            Assert.IsTrue(EqualityOfDouble(reflectance, 0.04));
        }

        [TestMethod()]
        public void SchlickWithN2GreaterN1() {
            Shape shape = Sphere.GlassSphere();
            Ray r = new Ray(Tuple.Point(0, 0.99, -2), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(1.8589, shape));
            Computations comps = Computations.PrepareComputations(xs[0], r, xs);
            double reflectance = Intersection.Schlick(comps);
            Assert.IsTrue(EqualityOfDouble(reflectance, 0.48873));
        }

        [TestMethod()]
        public void UVProperties() {
            Shape shape = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            Intersection i = new Intersection(3.5, shape, 0.2, 0.4);
            Assert.IsTrue(EqualityOfDouble(i.U, 0.2));
            Assert.IsTrue(EqualityOfDouble(i.V, 0.4));
        }
    }
}