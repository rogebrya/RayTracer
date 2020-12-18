using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class WorldTests {
        [TestMethod()]
        public void WorldTest() {
            World w = new World();
            Assert.IsNull(w.Shapes);
            Assert.IsNull(w.Light);
        }

        [TestMethod()]
        public void DefaultWorld() {
            Sphere s1 = new Sphere();
            s1.Material.Color = new Color(0.8, 1.0, 0.6);
            s1.Material.Diffuse = 0.7;
            s1.Material.Specular = 0.2;
            Sphere s2 = new Sphere();
            s2.Transform = Transformation.Scale(0.5, 0.5, 0.5);
            Light light = Light.PointLight(Tuple.Point(-10, 10, -10), new Color(1, 1, 1));
            World w = World.DefaultWorld();
            Assert.AreEqual(w.Light, light);
            Assert.IsTrue(w.Shapes.Contains(s1));
            Assert.IsTrue(w.Shapes.Contains(s2));
        }

        [TestMethod()]
        public void RayIntersectWorld() {
            World w = World.DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = w.IntersectWorld(r);
            Assert.AreEqual(xs.Count, 4);
            Assert.AreEqual(xs[0].T, 4);
            Assert.AreEqual(xs[1].T, 4.5);
            Assert.AreEqual(xs[2].T, 5.5);
            Assert.AreEqual(xs[3].T, 6);
        }

        [TestMethod()]
        public void ShadeIntersectionOutside() {
            World w = World.DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Shape shape = w.Shapes[0];
            Intersection i = new Intersection(4, shape);
            Computations comps = Computations.PrepareComputations(i, r);
            Color c = w.ShadeHit(comps, 0);
            Assert.AreEqual(c, new Color(0.38066, 0.47583, 0.2855));
        }

        [TestMethod()]
        public void ShadeIntersectionInside() {
            World w = World.DefaultWorld();
            w.Light = Light.PointLight(Tuple.Point(0, 0.25, 0), new Color(1, 1, 1));
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            Shape shape = w.Shapes[1];
            Intersection i = new Intersection(0.5, shape);
            Computations comps = Computations.PrepareComputations(i, r);
            Color c = w.ShadeHit(comps, 0);
            Assert.AreEqual(c, new Color(0.90498, 0.90498, 0.90498));
        }

        [TestMethod()]
        public void ColorAtMiss() {
            World w = World.DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 1, 0));
            Color c = w.ColorAt(r, 0);
            Assert.AreEqual(c, new Color(0, 0, 0));
        }

        [TestMethod()]
        public void ColorAtHit() {
            World w = World.DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Color c = w.ColorAt(r, 0);
            Assert.AreEqual(c, new Color(0.38066, 0.47583, 0.2855));
        }

        [TestMethod()]
        public void ColorAtIntersectionBehind() {
            World w = World.DefaultWorld();
            Shape outer = w.Shapes[0];
            outer.Material.Ambient = 1;
            Shape inner = w.Shapes[1];
            inner.Material.Ambient = 1;
            Ray r = new Ray(Tuple.Point(0, 0, 0.75), Tuple.Vector(0, 0, -1));
            Color c = w.ColorAt(r, 0);
            Assert.AreEqual(c, inner.Material.Color);
        }

        [TestMethod()]
        public void NoShadowOpenSpace() {
            World w = World.DefaultWorld();
            Tuple p = Tuple.Point(0, 10, 0); 
            Assert.IsFalse(w.IsShadowed(p));
        }

        [TestMethod()]
        public void ShadowBehindObject() {
            World w = World.DefaultWorld();
            Tuple p = Tuple.Point(10, -10, 10);
            Assert.IsTrue(w.IsShadowed(p));
        }

        [TestMethod()]
        public void NoShadowBehindLight() {
            World w = World.DefaultWorld();
            Tuple p = Tuple.Point(-20, 20, -20);
            Assert.IsFalse(w.IsShadowed(p));
        }

        [TestMethod()]
        public void NoShadowBetweenLightAndObject() {
            World w = World.DefaultWorld();
            Tuple p = Tuple.Point(-2, 2, -2);
            Assert.IsFalse(w.IsShadowed(p));
        }

        [TestMethod()]
        public void ShadeHitInShadow() {
            World w = new World();
            w.Light = Light.PointLight(Tuple.Point(0, 0, -10), new Color(1, 1, 1));
            Sphere s1 = new Sphere();
            w.AddShape(s1);
            Sphere s2 = new Sphere();
            s2.Transform = Transformation.Translate(0, 0, 10);
            w.AddShape(s2);
            Ray r = new Ray(Tuple.Point(0, 0, 5), Tuple.Vector(0, 0, 1));
            Intersection i = new Intersection(4, s2);
            Computations comps = Computations.PrepareComputations(i, r);
            Color c = w.ShadeHit(comps, 0);
            Assert.AreEqual(c, new Color(0.1, 0.1, 0.1));
        }

        [TestMethod()]
        public void ReflectedColorOfNonreflectiveMaterial() {
            World w = World.DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            Shape s2 = w.Shapes[1];
            s2.Material.Ambient = 1;
            Intersection i = new Intersection(1, s2);
            Computations comps = Computations.PrepareComputations(i, r);
            Color c = w.ReflectedColor(comps, 5);
            Assert.AreEqual(c, new Color(0.0, 0.0, 0.0));
        }

        [TestMethod()]
        public void ReflectedColorOfReflectiveMaterial() {
            World w = World.DefaultWorld();
            Shape shape = new Plane();
            shape.Material.Reflectivity = 0.5;
            shape.Transform = Transformation.Translate(0, -1, 0);
            w.AddShape(shape);
            Ray r = new Ray(Tuple.Point(0, 0, -3), Tuple.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            Intersection i = new Intersection(Math.Sqrt(2), shape);
            Computations comps = Computations.PrepareComputations(i, r);
            Color c = w.ReflectedColor(comps, 5);
            Assert.AreEqual(c, new Color(0.19032, 0.2379, 0.1427));
        }

        [TestMethod()]
        public void ShadeHitWithReflection() {
            World w = World.DefaultWorld();
            Shape shape = new Plane();
            shape.Material.Reflectivity = 0.5;
            shape.Transform = Transformation.Translate(0, -1, 0);
            w.AddShape(shape);
            Ray r = new Ray(Tuple.Point(0, 0, -3), Tuple.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            Intersection i = new Intersection(Math.Sqrt(2), shape);
            Computations comps = Computations.PrepareComputations(i, r);
            Color c = w.ShadeHit(comps, 5);
            Assert.AreEqual(c, new Color(0.87677, 0.92436, 0.82918));
        }

        [TestMethod()]
        public void ColorAtWithMutualReflections() {
            World w = new World();
            w.Light = Light.PointLight(Tuple.Point(0, 0, 0), new Color(1, 1, 1));
            Shape lower = new Plane();
            lower.Material.Reflectivity = 1;
            lower.Transform = Transformation.Translate(0, -1, 0);
            w.AddShape(lower);
            Shape upper = new Plane();
            upper.Material.Reflectivity = 1;
            upper.Transform = Transformation.Translate(0, 1, 0);
            w.AddShape(upper);
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 1, 0));
            Color c = w.ColorAt(r, 5);
            Assert.IsNotNull(c);
        }

        [TestMethod()]
        public void LimitRecursion() {
            World w = World.DefaultWorld();
            Shape shape = new Plane();
            shape.Material.Reflectivity = 0.5;
            shape.Transform = Transformation.Translate(0, -1, 0);
            w.AddShape(shape);
            Ray r = new Ray(Tuple.Point(0, 0, -3), Tuple.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            Intersection i = new Intersection(Math.Sqrt(2), shape);
            Computations comps = Computations.PrepareComputations(i, r);
            Color c = w.ReflectedColor(comps, 0);
            Assert.AreEqual(c, new Color(0, 0, 0));
        }

        [TestMethod()]
        public void RefractedColorWithOpaqueSurface() {
            World w = World.DefaultWorld();
            Shape shape = w.Shapes[0];
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(4, shape));
            xs.Add(new Intersection(6, shape));
            Computations comps = Computations.PrepareComputations(xs[0], r, xs);
            Color c = w.RefractedColor(comps, 5);
            Assert.AreEqual(c, new Color(0, 0, 0));
        }

        [TestMethod()]
        public void RefractedColorWithOpaqueSurfaceMaxRecursiveDepth() {
            World w = World.DefaultWorld();
            Shape shape = w.Shapes[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(4, shape));
            xs.Add(new Intersection(6, shape));
            Computations comps = Computations.PrepareComputations(xs[0], r, xs);
            Color c = w.RefractedColor(comps, 0);
            Assert.AreEqual(c, new Color(0, 0, 0));
        }

        [TestMethod()]
        public void RefractedColorUnderTotalInternalReflection() {
            World w = World.DefaultWorld();
            Shape shape = w.Shapes[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;
            Ray r = new Ray(Tuple.Point(0, 0, Math.Sqrt(2) / 2), Tuple.Vector(0, 1, 0));
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(-Math.Sqrt(2) / 2, shape));
            xs.Add(new Intersection(Math.Sqrt(2) / 2, shape));
            Computations comps = Computations.PrepareComputations(xs[1], r, xs);
            Color c = w.RefractedColor(comps, 5);
            Assert.AreEqual(c, new Color(0, 0, 0));
        }

        [TestMethod()]
        public void RefractedColor() {
            World w = World.DefaultWorld();
            Shape a = w.Shapes[0];
            a.Material.Ambient = 1.0;
            a.Material.Pattern = new TestPattern();
            Shape b = w.Shapes[1];
            b.Material.Transparency = 1.0;
            b.Material.RefractiveIndex = 1.5;
            Ray r = new Ray(Tuple.Point(0, 0, 0.1), Tuple.Vector(0, 1, 0));
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(-0.9899, a));
            xs.Add(new Intersection(-0.4899, b));
            xs.Add(new Intersection(0.4899, b));
            xs.Add(new Intersection(0.9899, a));
            Computations comps = Computations.PrepareComputations(xs[2], r, xs);
            Color c = w.RefractedColor(comps, 5);
            Assert.AreEqual(c, new Color(0, 0.99888, 0.04725));
        }

        [TestMethod()]
        public void ShadeHitWithTransparency() {
            World w = World.DefaultWorld();
            Shape floor = new Plane();
            floor.Transform = Transformation.Translate(0, -1, 0);
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;
            w.AddShape(floor);
            Shape ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = 0.5;
            ball.Transform = Transformation.Translate(0, -3.5, -0.5);
            w.AddShape(ball);
            Ray r = new Ray(Tuple.Point(0, 0, -3), Tuple.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(Math.Sqrt(2), floor));
            Computations comps = Computations.PrepareComputations(xs[0], r, xs);
            Color c = w.ShadeHit(comps, 5);
            Assert.AreEqual(c, new Color(0.93642, 0.68642, 0.68642));
        }

        [TestMethod()]
        public void ShadeHitWithReflectiveTransparency() {
            World w = World.DefaultWorld();
            Shape floor = new Plane();
            floor.Transform = Transformation.Translate(0, -1, 0);
            floor.Material.Reflectivity = 0.5;
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;
            w.AddShape(floor);
            Shape ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = 0.5;
            ball.Transform = Transformation.Translate(0, -3.5, -0.5);
            w.AddShape(ball);
            Ray r = new Ray(Tuple.Point(0, 0, -3), Tuple.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(Math.Sqrt(2), floor));
            Computations comps = Computations.PrepareComputations(xs[0], r, xs);
            Color c = w.ShadeHit(comps, 5);
            Assert.AreEqual(c, new Color(0.93391, 0.69643, 0.69243));
        }
    }
}