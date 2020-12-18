using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class MaterialTests {
        Material m = new Material();
        Tuple position = Tuple.Point(0, 0, 0);

        [TestMethod()]
        public void MaterialTest() {
            Assert.AreEqual(m.Color, new Color(1, 1, 1));
            Assert.AreEqual(m.Ambient, 0.1);
            Assert.AreEqual(m.Diffuse, 0.9);
            Assert.AreEqual(m.Specular, 0.9);
            Assert.AreEqual(m.Shininess, 200.0);
        }

        [TestMethod()]
        public void LightingLES() {
            Tuple eyev = Tuple.Vector(0, 0, -1);
            Tuple normalv = Tuple.Vector(0, 0, -1);
            Light light = Light.PointLight(Tuple.Point(0, 0, -10), new Color(1, 1, 1));
            bool inShadow = false;
            Sphere s = new Sphere();
            Color result = m.Lighting(s, light, position, eyev, normalv, inShadow);
            Assert.AreEqual(result, new Color(1.9, 1.9, 1.9));
        }

        [TestMethod()]
        public void LightingL_Offset_ES() {
            Tuple eyev = Tuple.Vector(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            Tuple normalv = Tuple.Vector(0, 0, -1);
            Light light = Light.PointLight(Tuple.Point(0, 0, -10), new Color(1, 1, 1));
            bool inShadow = false;
            Sphere s = new Sphere();
            Color result = m.Lighting(s, light, position, eyev, normalv, inShadow);
            Assert.AreEqual(result, new Color(1.0, 1.0, 1.0));
        }

        [TestMethod()]
        public void LightingLE_Offset_S() {
            Tuple eyev = Tuple.Vector(0, 0, -1);
            Tuple normalv = Tuple.Vector(0, 0, -1);
            Light light = Light.PointLight(Tuple.Point(0, 10, -10), new Color(1, 1, 1));
            bool inShadow = false;
            Sphere s = new Sphere();
            Color result = m.Lighting(s, light, position, eyev, normalv, inShadow);
            Assert.AreEqual(result, new Color(0.7364, 0.7364, 0.7364));
        }

        [TestMethod()]
        public void LightingL_Offset_E_Offset_S() {
            Tuple eyev = Tuple.Vector(0, -Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            Tuple normalv = Tuple.Vector(0, 0, -1);
            Light light = Light.PointLight(Tuple.Point(0, 10, -10), new Color(1, 1, 1));
            bool inShadow = false;
            Sphere s = new Sphere();
            Color result = m.Lighting(s, light, position, eyev, normalv, inShadow);
            Assert.AreEqual(result, new Color(1.6364, 1.6364, 1.6364));
        }

        [TestMethod()]
        public void LightingESL() {
            Tuple eyev = Tuple.Vector(0, 0, -1);
            Tuple normalv = Tuple.Vector(0, 0, -1);
            Light light = Light.PointLight(Tuple.Point(0, 0, 10), new Color(1, 1, 1));
            bool inShadow = false;
            Sphere s = new Sphere();
            Color result = m.Lighting(s, light, position, eyev, normalv, inShadow);
            Assert.AreEqual(result, new Color(0.1, 0.1, 0.1));
        }

        [TestMethod()]
        public void ShadowLightingLES() {
            Tuple eyev = Tuple.Vector(0, 0, -1);
            Tuple normalv = Tuple.Vector(0, 0, -1);
            Light light = Light.PointLight(Tuple.Point(0, 0, -10), new Color(1, 1, 1));
            bool inShadow = true;
            Sphere s = new Sphere();
            Color result = m.Lighting(s, light, position, eyev, normalv, inShadow);
            Assert.AreEqual(result, new Color(0.1, 0.1, 0.1));
        }

        [TestMethod()]
        public void LightingWithPattern() {
            m.Pattern = new StripedPattern(new Color(1, 1, 1), new Color(0, 0, 0));
            m.Ambient = 1;
            m.Diffuse = 0;
            m.Specular = 0;
            Tuple eyev = Tuple.Vector(0, 0, -1);
            Tuple normalv = Tuple.Vector(0, 0, -1);
            Light light = Light.PointLight(Tuple.Point(0, 0, -10), new Color(1, 1, 1));
            bool inShadow = false;
            Sphere s = new Sphere();
            Color c1 = m.Lighting(s, light, Tuple.Point(0.9, 0, 0), eyev, normalv, inShadow);
            Color c2 = m.Lighting(s, light, Tuple.Point(1.1, 0, 0), eyev, normalv, inShadow);
            Assert.AreEqual(c1, new Color(1, 1, 1));
            Assert.AreEqual(c2, new Color(0, 0, 0));
        }

        [TestMethod()]
        public void ReflectivityDefault() {
            Assert.AreEqual(m.Reflectivity, 0.0);
        }

        [TestMethod()]
        public void RefractionDefaults() {
            Assert.AreEqual(m.Transparency, 0.0);
            Assert.AreEqual(m.RefractiveIndex, 1.0);
        }
    }
}