using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class TransformationTests {
        [TestMethod()]
        public void TranslationPoint() {
            Matrix transform = Transformation.Translate(5, -3, 2);
            Tuple p = Tuple.Point(-3, 4, 5);
            Assert.AreEqual(transform * p, Tuple.Point(2, 1, 7));
        }

        [TestMethod()]
        public void TranslationInverse() {
            Matrix transform = Transformation.Translate(5, -3, 2);
            Matrix inv = transform.Inverse();
            Tuple p = Tuple.Point(-3, 4, 5);
            Assert.AreEqual(inv * p, Tuple.Point(-8, 7, 3));
        }

        [TestMethod()]
        public void TranslationDoesntAffectVectors() {
            Matrix transform = Transformation.Translate(5, -3, 2);
            Tuple v = Tuple.Vector(-3, 4, 5);
            Assert.AreEqual(transform * v, Tuple.Vector(-3, 4, 5));
        }

        [TestMethod()]
        public void ScalingPoint() {
            Matrix transform = Transformation.Scale(2, 3, 4);
            Tuple p = Tuple.Point(-4, 6, 8);
            Assert.AreEqual(transform * p, Tuple.Point(-8, 18, 32));
        }

        [TestMethod()]
        public void ScalingVector() {
            Matrix transform = Transformation.Scale(2, 3, 4);
            Tuple v = Tuple.Vector(-4, 6, 8);
            Assert.AreEqual(transform * v, Tuple.Vector(-8, 18, 32));
        }

        [TestMethod()]
        public void ScalingInverse() {
            Matrix transform = Transformation.Scale(2, 3, 4);
            Matrix inv = transform.Inverse();
            Tuple v = Tuple.Vector(-4, 6, 8);
            Assert.AreEqual(inv * v, Tuple.Vector(-2, 2, 2));
        }

        [TestMethod()]
        // Reflection
        public void ScalingNagative() {
            Matrix transform = Transformation.Scale(-1, 1, 1);
            Tuple p = Tuple.Point(2, 3, 4);
            Assert.AreEqual(transform * p, Tuple.Point(-2, 3, 4));
        }

        [TestMethod()]
        public void RotationXaxis() {
            Tuple p = Tuple.Point(0, 1, 0);
            Matrix half_quarter = Transformation.Rotate_X(Math.PI / 4);
            Matrix full_quarter = Transformation.Rotate_X(Math.PI / 2);
            Assert.AreEqual(half_quarter * p, Tuple.Point(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            Assert.AreEqual(full_quarter * p, Tuple.Point(0, 0, 1));
        }

        [TestMethod()]
        public void RotationXaxisInverse() {
            Tuple p = Tuple.Point(0, 1, 0);
            Matrix half_quarter = Transformation.Rotate_X(Math.PI / 4);
            Matrix inv = half_quarter.Inverse();
            Assert.AreEqual(inv * p, Tuple.Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2));
        }

        [TestMethod()]
        public void RotationYaxis() {
            Tuple p = Tuple.Point(0, 0, 1);
            Matrix half_quarter = Transformation.Rotate_Y(Math.PI / 4);
            Matrix full_quarter = Transformation.Rotate_Y(Math.PI / 2);
            Assert.AreEqual(half_quarter * p, Tuple.Point(Math.Sqrt(2) / 2, 0, Math.Sqrt(2) / 2));
            Assert.AreEqual(full_quarter * p, Tuple.Point(1, 0, 0));
        }

        [TestMethod()]
        public void RotationZaxis() {
            Tuple p = Tuple.Point(0, 1, 0);
            Matrix half_quarter = Transformation.Rotate_Z(Math.PI / 4);
            Matrix full_quarter = Transformation.Rotate_Z(Math.PI / 2);
            Assert.AreEqual(half_quarter * p, Tuple.Point(-Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0));
            Assert.AreEqual(full_quarter * p, Tuple.Point(-1, 0, 0));
        }

        [TestMethod()]
        public void Shear() {
            Matrix transform = Transformation.Shear(1, 0, 0, 0, 0, 0);
            Tuple p = Tuple.Point(2, 3, 4);
            Assert.AreEqual(transform * p, Tuple.Point(5, 3, 4));

            transform = Transformation.Shear(0, 1, 0, 0, 0, 0);
            p = Tuple.Point(2, 3, 4);
            Assert.AreEqual(transform * p, Tuple.Point(6, 3, 4));

            transform = Transformation.Shear(0, 0, 1, 0, 0, 0);
            p = Tuple.Point(2, 3, 4);
            Assert.AreEqual(transform * p, Tuple.Point(2, 5, 4));

            transform = Transformation.Shear(0, 0, 0, 1, 0, 0);
            p = Tuple.Point(2, 3, 4);
            Assert.AreEqual(transform * p, Tuple.Point(2, 7, 4));

            transform = Transformation.Shear(0, 0, 0, 0, 1, 0);
            p = Tuple.Point(2, 3, 4);
            Assert.AreEqual(transform * p, Tuple.Point(2, 3, 6));

            transform = Transformation.Shear(0, 0, 0, 0, 0, 1);
            p = Tuple.Point(2, 3, 4);
            Assert.AreEqual(transform * p, Tuple.Point(2, 3, 7));
        }

        [TestMethod()]
        public void CombinedTransformations() {
            Tuple p = Tuple.Point(1, 0, 1);
            Matrix a = Transformation.Rotate_X(Math.PI / 2);
            Matrix b = Transformation.Scale(5, 5, 5);
            Matrix c = Transformation.Translate(10, 5, 7);
            Tuple p2 = a * p;
            Assert.AreEqual(p2, Tuple.Point(1, -1, 0));
            Tuple p3 = b * p2;
            Assert.AreEqual(p3, Tuple.Point(5, -5, 0));
            Tuple p4 = c * p3;
            Assert.AreEqual(p4, Tuple.Point(15, 0, 7));
        }

        [TestMethod()]
        public void CombinedTransformations2() {
            Tuple p = Tuple.Point(1, 0, 1);
            Matrix a = Transformation.Rotate_X(Math.PI / 2);
            Matrix b = Transformation.Scale(5, 5, 5);
            Matrix c = Transformation.Translate(10, 5, 7);
            Matrix t = c * b * a;
            Assert.AreEqual(t * p, Tuple.Point(15, 0, 7));
        }

        [TestMethod()]
        public void DefaultView() {
            Tuple from = Tuple.Point(0, 0, 0);
            Tuple to = Tuple.Point(0, 0, -1);
            Tuple up = Tuple.Vector(0, 1, 0);
            Matrix t = Transformation.ViewTransform(from, to, up);
            Assert.AreEqual(t, Matrix.GetIdentityMatrix());
        }

        [TestMethod()]
        public void ViewTransformPZ() {
            Tuple from = Tuple.Point(0, 0, 0);
            Tuple to = Tuple.Point(0, 0, 1);
            Tuple up = Tuple.Vector(0, 1, 0);
            Matrix t = Transformation.ViewTransform(from, to, up);
            Assert.AreEqual(t, Transformation.Scale(-1, 1, -1));
        }

        [TestMethod()]
        public void ViewTransformMovesWorld() {
            Tuple from = Tuple.Point(0, 0, 8);
            Tuple to = Tuple.Point(0, 0, 0);
            Tuple up = Tuple.Vector(0, 1, 0);
            Matrix t = Transformation.ViewTransform(from, to, up);
            Assert.AreEqual(t, Transformation.Translate(0, 0, -8));
        }

        [TestMethod()]
        public void ViewTransform() {
            Tuple from = Tuple.Point(1, 3, 2);
            Tuple to = Tuple.Point(4, -2, 8);
            Tuple up = Tuple.Vector(1, 1, 0);
            Matrix t = Transformation.ViewTransform(from, to, up);
            Assert.AreEqual(t, new Matrix(-0.50709, 0.50709, 0.67612, -2.36643, 0.76772, 0.60609, 0.12122, -2.82843, -0.35857, 0.59761, -0.71714, 0.00000, 0.00000, 0.00000, 0.00000, 1.00000));
        }
    }
}