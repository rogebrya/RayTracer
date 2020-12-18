using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class LightTests {
        [TestMethod()]
        public void LightTest() {
            Color intensity = new Color(1, 1, 1);
            Tuple position = Tuple.Point(0, 0, 0);
            Light light = Light.PointLight(position, intensity);
            Assert.AreEqual(light.Position, position);
            Assert.AreEqual(light.Intensity, intensity);
        }
    }
}