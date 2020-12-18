using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class TupleTests {
        [TestMethod()]
        public void W1isPoint() {
            Tuple a = new Tuple(4.3, -4.2, 3.1, 1.0);
            Assert.AreEqual(a.X, 4.3);
            Assert.AreEqual(a.Y, -4.2);
            Assert.AreEqual(a.Z, 3.1);
            Assert.AreEqual(a.W, 1.0);
            Assert.AreEqual(a.IsPoint(), true);
            Assert.AreEqual(a.IsVector(), false);
        }

        [TestMethod()]
        public void W0isVector() {
            Tuple a = new Tuple(4.3, -4.2, 3.1, 0.0);
            Assert.AreEqual(a.X, 4.3);
            Assert.AreEqual(a.Y, -4.2);
            Assert.AreEqual(a.Z, 3.1);
            Assert.AreEqual(a.W, 0.0);
            Assert.AreEqual(a.IsPoint(), false);
            Assert.AreEqual(a.IsVector(), true);
        }

        [TestMethod()]
        public void IsPoint() {
            Tuple p = Tuple.Point(4, -4, 3);
            Assert.AreEqual(p, new Tuple(4, -4, 3, 1));
        }

        [TestMethod()]
        public void IsVector() {
            Tuple v = Tuple.Vector(4, -4, 3);
            Assert.AreEqual(v, new Tuple(4, -4, 3, 0));
        }

        [TestMethod()]
        public void Add() {
            Tuple u = new Tuple(3, -2, 5, 1);
            Tuple v = new Tuple(-2, 3, 1, 0);
            Assert.AreEqual(u+v, new Tuple(1, 1, 6, 1));
        }

        [TestMethod()]
        public void Subtract() {
            Tuple u = Tuple.Point(3, 2, 1);
            Tuple v = Tuple.Point(5, 6, 7);
            Assert.AreEqual(u - v, Tuple.Vector(-2, -4, -6));
            Assert.AreNotEqual(u - v, Tuple.Point(-2, -4, -6));

            u = Tuple.Point(3, 2, 1);
            v = Tuple.Vector(5, 6, 7);
            Assert.AreEqual(u - v, Tuple.Point(-2, -4, -6));
            Assert.AreNotEqual(u - v, Tuple.Vector(-2, -4, -6));

            u = Tuple.Vector(3, 2, 1);
            v = Tuple.Vector(5, 6, 7);
            Assert.AreEqual(u - v, Tuple.Vector(-2, -4, -6));
            Assert.AreNotEqual(u - v, Tuple.Point(-2, -4, -6));
        }

        [TestMethod()]
        public void Negation() {
            Tuple zero = Tuple.Vector(0, 0, 0);
            Tuple v = Tuple.Vector(1, -2, 3);
            Assert.AreEqual(zero - v, Tuple.Vector(-1, 2, -3));

            v = new Tuple(1, -2, 3, -4);
            Assert.AreEqual(-v, new Tuple(-1, 2, -3, 4));
        }

        [TestMethod()]
        public void ScalarMultiply() {
            Tuple v = new Tuple(1, -2, 3, -4);
            Assert.AreEqual(v * 3.5, new Tuple(3.5, -7, 10.5, -14));

            v = new Tuple(1, -2, 3, -4);
            Assert.AreEqual(v * 0.5, new Tuple(0.5, -1, 1.5, -2));
        }

        [TestMethod()]
        public void ScalarDivide() {
            Tuple v = new Tuple(1, -2, 3, -4);
            Assert.AreEqual(v / 2, new Tuple(0.5, -1, 1.5, -2));
        }

        [TestMethod()]
        public void Magnitude() {
            Tuple v = Tuple.Vector(1, 0, 0);
            Assert.AreEqual(Tuple.Magnitude(v), 1);

            v = Tuple.Vector(0, 1, 0);
            Assert.AreEqual(Tuple.Magnitude(v), 1);

            v = Tuple.Vector(0, 0, 1);
            Assert.AreEqual(Tuple.Magnitude(v), 1);

            v = Tuple.Vector(1, 2, 3);
            Assert.AreEqual(Tuple.Magnitude(v), Math.Sqrt(14));

            v = Tuple.Vector(-1, -2, -3);
            Assert.AreEqual(Tuple.Magnitude(v), Math.Sqrt(14));
        }

        [TestMethod()]
        public void Normalization() {
            Tuple v = Tuple.Vector(4, 0, 0);
            Assert.AreEqual(Tuple.Normalize(v), Tuple.Vector(1, 0, 0));

            v = Tuple.Vector(1, 2, 3);
            v = Tuple.Normalize(v);
            Assert.AreEqual(v, Tuple.Vector(1/Math.Sqrt(14), 2/Math.Sqrt(14), 3/Math.Sqrt(14)));
            //Console.WriteLine(v.X + "____" + v.Y + "____" + v.Z + "____" + v.W + "____" + Environment.NewLine);
            //Console.WriteLine(1 / Math.Sqrt(14) + "____" + 2 / Math.Sqrt(14) + "____" + 3 / Math.Sqrt(14) + "____" + v.W + "____" + Environment.NewLine);

            v = Tuple.Vector(1, 2, 3);
            Tuple norm = Tuple.Normalize(v);
            Assert.AreEqual(Tuple.Magnitude(norm), 1);
        }

        [TestMethod()]
        public void DotProduct() {
            Tuple u = Tuple.Vector(1, 2, 3);
            Tuple v = Tuple.Vector(2, 3, 4);
            Assert.AreEqual(Tuple.Dot(u, v), 20);
        }

        [TestMethod()]
        public void CrossProduct() {
            Tuple u = Tuple.Vector(1, 2, 3);
            Tuple v = Tuple.Vector(2, 3, 4);
            Assert.AreEqual(Tuple.Cross(u, v), Tuple.Vector(-1, 2, -1));
            Assert.AreEqual(Tuple.Cross(v, u), Tuple.Vector(1, -2, 1));
        }

        [TestMethod()]
        public void Reflect45() {
            Tuple v = Tuple.Vector(1, -1, 0);
            Tuple n = Tuple.Vector(0, 1, 0);
            Tuple r = Tuple.Reflect(v, n);
            Assert.AreEqual(r, Tuple.Vector(1, 1, 0));
        }

        [TestMethod()]
        public void ReflectOther() {
            Tuple v = Tuple.Vector(0, -1, 0);
            Tuple n = Tuple.Vector(Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0);
            Tuple r = Tuple.Reflect(v, n);
            Assert.AreEqual(r, Tuple.Vector(1, 0, 0));
        }
    }
}