using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class ShapeTests {
        [TestMethod()]
        public void Transform() {
            TestShape s = new TestShape();
            Assert.AreEqual(s.Transform, Matrix.GetIdentityMatrix());
        }

        [TestMethod()]
        public void ChangeTransform() {
            TestShape s = new TestShape();
            s.Transform = Transformation.Translate(2, 3, 4);
            Assert.AreEqual(s.Transform, Transformation.Translate(2, 3, 4));
        }

        [TestMethod()]
        public void Material() {
            TestShape s = new TestShape();
            Material m = s.Material;
            Assert.AreEqual(m, new Material());
        }

        [TestMethod()]
        public void ChangeMaterial() {
            TestShape s = new TestShape();
            Material m = new Material();
            m.Ambient = 1;
            s.Material = m;
            Assert.AreEqual(s.Material, m);
        }

        [TestMethod()]
        public void ScaledShapeIntersectRay() {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            TestShape s = new TestShape();
            s.Transform = Transformation.Scale(2, 2, 2);
            List<Intersection> xs = s.Intersect(r);
            Assert.AreEqual(s.savedRay.Origin, Tuple.Point(0, 0, -2.5));
            Assert.AreEqual(s.savedRay.Direction, Tuple.Vector(0, 0, 0.5));
        }

        [TestMethod()]
        public void TranslatedShapeIntersectRay() {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            TestShape s = new TestShape();
            s.Transform = Transformation.Translate(5, 0, 0);
            List<Intersection> xs = s.Intersect(r);
            Assert.AreEqual(s.savedRay.Origin, Tuple.Point(-5, 0, -5));
            Assert.AreEqual(s.savedRay.Direction, Tuple.Vector(0, 0, 1));
        }

        [TestMethod()]
        public void NormalTranslated() {
            TestShape s = new TestShape();
            s.Transform = Transformation.Translate(0, 1, 0);
            Tuple n = s.NormalAt(Tuple.Point(0, 1.70711, -0.70711), new Intersection(0, s));
            Assert.AreEqual(n, Tuple.Vector(0, 0.70711, -0.70711));
        }

        [TestMethod()]
        public void NormalTransformed() {
            TestShape s = new TestShape();
            Matrix m = Transformation.Scale(1, 0.5, 1) * Transformation.Rotate_Z(Math.PI / 5);
            s.Transform = m;
            Tuple n = s.NormalAt(Tuple.Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2), new Intersection(0, s));
            Assert.AreEqual(n, Tuple.Vector(0, 0.97014, -0.24254));
        }

        [TestMethod()]
        public void IsAShape() {
            TestShape ts = new TestShape();
            Sphere sp = new Sphere();
            Assert.IsTrue(ts.IsAShape());
            Assert.IsTrue(sp.IsAShape());
        }

        [TestMethod()]
        public void ShapeHasParentAttribute() {
            Shape s = new TestShape();
            Assert.IsNull(s.Parent);
        }

        [TestMethod()]
        public void ConvertPointFromWorldToObject() {
            Group g1 = new Group();
            g1.Transform = Transformation.Rotate_Y(Math.PI / 2);
            Group g2 = new Group();
            g2.Transform = Transformation.Scale(2, 2, 2);
            g1.AddShape(g2);
            Sphere s = new Sphere();
            s.Transform = Transformation.Translate(5, 0, 0);
            g2.AddShape(s);
            Tuple p = s.WorldToObject(Tuple.Point(-2, 0, -10));
            Assert.AreEqual(p, Tuple.Point(0, 0, -1));
        }

        [TestMethod()]
        public void ConvertNormalFromObjectToWorld() {
            Group g1 = new Group();
            g1.Transform = Transformation.Rotate_Y(Math.PI / 2);
            Group g2 = new Group();
            g2.Transform = Transformation.Scale(1, 2, 3);
            g1.AddShape(g2);
            Sphere s = new Sphere();
            s.Transform = Transformation.Translate(5, 0, 0);
            g2.AddShape(s);
            Tuple n = s.NormalToWorld(Tuple.Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));
            Assert.AreEqual(n, Tuple.Vector(0.2857, 0.4286, -0.8571));
        }

        [TestMethod()]
        public void ConvertNormalOnChild() {
            Group g1 = new Group();
            g1.Transform = Transformation.Rotate_Y(Math.PI / 2);
            Group g2 = new Group();
            g2.Transform = Transformation.Scale(1, 2, 3);
            g1.AddShape(g2);
            Sphere s = new Sphere();
            s.Transform = Transformation.Translate(5, 0, 0);
            g2.AddShape(s);
            Tuple n = s.NormalAt(Tuple.Point(1.7321, 1.1547, -5.5774), new Intersection(0, s));
            Assert.AreEqual(n, Tuple.Vector(0.2857, 0.4286, -0.8571));
        }
    }
}