using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class ColorTests {
        [TestMethod()]
        public void ColorTest() {
            Color c = new Color(-0.5, 0.4, 1.7);
            Assert.AreEqual(c.Red, -0.5);
            Assert.AreEqual(c.Green, 0.4);
            Assert.AreEqual(c.Blue, 1.7);
        }

        [TestMethod()]
        public void Addition() {
            Color c1 = new Color(0.9, 0.6, 0.75);
            Color c2 = new Color(0.7, 0.1, 0.25);
            Assert.AreEqual(c1 + c2, new Color(1.6, 0.7, 1.0));
        }

        [TestMethod()]
        public void Subtraction() {
            Color c1 = new Color(0.9, 0.6, 0.75);
            Color c2 = new Color(0.7, 0.1, 0.25);
            Assert.AreEqual(c1 - c2, new Color(0.2, 0.5, 0.5));
        }

        [TestMethod()]
        public void ScalarMultiply() {
            Color c = new Color(0.2, 0.3, 0.4);
            Assert.AreEqual(c * 2, new Color(0.4, 0.6, 0.8));
        }

        [TestMethod()]
        public void VectorMultiply() {
            Color c1 = new Color(1, 0.2, 0.4);
            Color c2 = new Color(0.9, 1, 0.1);
            Assert.AreEqual(c1 * c2, new Color(0.9, 0.2, 0.04));
        }
    }
}