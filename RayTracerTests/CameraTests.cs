using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class CameraTests {
        [TestMethod()]
        public void CameraTest() {
            int hsize = 160;
            int vsize = 120;
            double fieldOfView = Math.PI / 2;
            Camera c = new Camera(hsize, vsize, fieldOfView);
            Assert.AreEqual(c.Hsize, 160);
            Assert.AreEqual(c.Vsize, 120);
            Assert.AreEqual(c.FieldOfView, Math.PI / 2);
            Assert.AreEqual(c.Transform, Matrix.GetIdentityMatrix());
        }

        [TestMethod()]
        public void PixelSizeHorizontal() {
            Camera c = new Camera(200, 125, Math.PI / 2);
            Assert.IsTrue(Tuple.EqualityOfDouble(c.PixelSize, 0.01));
        }

        [TestMethod()]
        public void PixelSizeVertical() {
            Camera c = new Camera(125, 200, Math.PI / 2);
            Assert.IsTrue(Tuple.EqualityOfDouble(c.PixelSize, 0.01));
        }

        [TestMethod()]
        public void RayThroughCanvasCenter() {
            Camera c = new Camera(201, 101, Math.PI / 2);
            Ray r = c.RayForPixel(100, 50);
            Assert.AreEqual(r.Origin, Tuple.Point(0, 0, 0));
            Assert.AreEqual(r.Direction, Tuple.Vector(0, 0, -1));
        }

        [TestMethod()]
        public void RayThroughCanvasCorner() {
            Camera c = new Camera(201, 101, Math.PI / 2);
            Ray r = c.RayForPixel(0, 0);
            Assert.AreEqual(r.Origin, Tuple.Point(0, 0, 0));
            Assert.AreEqual(r.Direction, Tuple.Vector(0.66519, 0.33259, -0.66851));
        }

        [TestMethod()]
        public void RayWithTransformedCamera() {
            Camera c = new Camera(201, 101, Math.PI / 2);
            c.Transform = Transformation.Rotate_Y(Math.PI / 4) * Transformation.Translate(0, -2, 5);
            Ray r = c.RayForPixel(100, 50);
            Assert.AreEqual(r.Origin, Tuple.Point(0, 2, -5));
            Assert.AreEqual(r.Direction, Tuple.Vector(Math.Sqrt(2) / 2, 0, -Math.Sqrt(2) / 2));
        }

        [TestMethod()]
        public void RenderWorldWithCamera() {
            World w = World.DefaultWorld();
            Camera c = new Camera(11, 11, Math.PI / 2);
            Tuple from = Tuple.Point(0, 0, -5);
            Tuple to = Tuple.Point(0, 0, 0);
            Tuple up = Tuple.Vector(0, 1, 0);
            c.Transform = Transformation.ViewTransform(from, to, up);
            Canvas image = c.Render(w);
            Assert.AreEqual(image.PixelAt(5, 5), new Color(0.38066, 0.47583, 0.2855));
        }
    }
}