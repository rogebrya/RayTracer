using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class CanvasTests {
        [TestMethod()]
        public void CanvasTest() {
            Canvas c = new Canvas(10, 20);
            Assert.AreEqual(c.Width, 10);
            Assert.AreEqual(c.Height, 20);
            for (int i = 0; i < c.Width; i++) {
                for (int j = 0; j < c.Height; j++) {
                    c.Pixels[i, j] = new Color(0, 0, 0);
                    Assert.AreEqual(c.Pixels[i, j], new Color(0, 0, 0));
                }
            }
        }

        [TestMethod()]
        public void WritePixelsToCanvas() {
            Canvas c = new Canvas(10, 20);
            Color red = new Color(1, 0, 0);
            c.WritePixel(2, 3, red);
            Assert.AreEqual(c.PixelAt(2, 3), red);
        }
        /*
        [TestMethod()]
        public void PPM_Header() {
            Canvas c = new Canvas(5, 3);
            string ppm = c.PPM_Header();
            string expectedHeader =
                "P3" + Environment.NewLine +
                "5 3" + Environment.NewLine +
                "255" + Environment.NewLine;
            Assert.AreEqual(ppm, expectedHeader);
        }
        */
        [TestMethod()]
        public void PPM_Body() {
            Canvas c = new Canvas(5, 3);
            Color c1 = new Color(1.5, 0, 0);
            Color c2 = new Color(0, 0.5, 0);
            Color c3 = new Color(-0.5, 0, 1);
            c.WritePixel(0, 0, c1);
            c.WritePixel(2, 1, c2);
            c.WritePixel(4, 2, c3);
            string ppm = c.PPM_Body();
            string expectedBody =
                "255 0 0 0 0 0 0 0 0 0 0 0 0 0 0" + Environment.NewLine +
                "0 0 0 0 0 0 0 128 0 0 0 0 0 0 0" + Environment.NewLine +
                "0 0 0 0 0 0 0 0 0 0 0 0 0 0 255" + Environment.NewLine;
            Assert.AreEqual(ppm, expectedBody);
        }

        /*
        [TestMethod()]
        public void PPM_Body_SplitLines() {
            Canvas c = new Canvas(10, 2);
            Color c1 = new Color(1, 0.8, 0.6);
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 10; j++) {
                    c.WritePixel(j, i, c1);
                }
            }
            string ppm = c.PPM_Body_Split_Lines();
            string expectedBody =
                "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204" + Environment.NewLine +
                "153 255 204 153 255 204 153 255 204 153 255 204 153" + Environment.NewLine +
                "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204" + Environment.NewLine +
                "153 255 204 153 255 204 153 255 204 153 255 204 153" + Environment.NewLine;
            Assert.AreEqual(ppm, expectedBody);
        }
        */
        [TestMethod()]
        public void PPM_Body_SplitLines_Ends_With_NL() {
            Canvas c = new Canvas(5, 3);
            string ppm = c.CanvasToPPM();
            string expectedBodyEnd = Environment.NewLine;
            Assert.AreEqual(ppm.EndsWith(expectedBodyEnd), true);
        }
    }
}