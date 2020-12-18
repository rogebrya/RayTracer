using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class PatternTests {
        Color black = new Color(0, 0, 0);
        Color white = new Color(1, 1, 1);

        
        [TestMethod()]
        public void StripedPattern() {
            Pattern pattern = new StripedPattern(white, black);
            Assert.AreEqual(pattern.A, white);
            Assert.AreEqual(pattern.B, black);
        }

        [TestMethod()]
        public void StripedPatternYConst() {
            Pattern pattern = new StripedPattern(white, black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 1, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 2, 0)), white);
        }

        [TestMethod()]
        public void StripedPatternZConst() {
            Pattern pattern = new StripedPattern(white, black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 1)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 2)), white);
        }

        [TestMethod()]
        public void StripedPatternXChanges() {
            Pattern pattern = new StripedPattern(white, black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0.9, 0, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(1, 0, 0)), black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(-0.1, 0, 0)), black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(-1, 0, 0)), black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(-1.1, 0, 0)), white);
        }

        [TestMethod()]
        public void StripesWithObjectTransformation() {
            Sphere s = new Sphere();
            s.Transform = Transformation.Scale(2, 2, 2);
            Pattern pattern = new StripedPattern(white, black);
            Color c = pattern.PatternAtShape(s, Tuple.Point(1.5, 0, 0));
            Assert.AreEqual(c, white);
        }

        [TestMethod()]
        public void StripesWithPatternTransformation() {
            Sphere s = new Sphere();
            Pattern pattern = new StripedPattern(white, black);
            pattern.Transform = Transformation.Scale(2, 2, 2);
            Color c = pattern.PatternAtShape(s, Tuple.Point(1.5, 0, 0));
            Assert.AreEqual(c, white);
        }

        [TestMethod()]
        public void StripesWithObjectAndPatternTransformation() {
            Sphere s = new Sphere();
            s.Transform = Transformation.Scale(2, 2, 2);
            Pattern pattern = new StripedPattern(white, black);
            pattern.Transform = Transformation.Translate(0.5, 0, 0);
            Color c = pattern.PatternAtShape(s, Tuple.Point(2.5, 0, 0));
            Assert.AreEqual(c, white);
        }

        //-------------------------------------------------------------------

        [TestMethod()]
        public void TestPatternDefaultTransform() {
            TestPattern pattern = new TestPattern();
            Assert.AreEqual(pattern.Transform, Matrix.GetIdentityMatrix());
        }

        [TestMethod()]
        public void TestPatternAssignTransform() {
            TestPattern pattern = new TestPattern();
            pattern.Transform = Transformation.Translate(1, 2, 3);
            Assert.AreEqual(pattern.Transform, Transformation.Translate(1, 2, 3));
        }

        [TestMethod()]
        public void PatternAtShapeObjectTransform() {
            Sphere shape = new Sphere();
            shape.Transform = Transformation.Scale(2, 2, 2);
            TestPattern pattern = new TestPattern();
            Color c = pattern.PatternAtShape(shape, Tuple.Point(2, 3, 4));
            Assert.AreEqual(c, new Color(1, 1.5, 2));
        }

        [TestMethod()]
        public void PatternAtShapePAtternTransform() {
            Sphere shape = new Sphere();
            TestPattern pattern = new TestPattern();
            pattern.Transform = Transformation.Scale(2, 2, 2);
            Color c = pattern.PatternAtShape(shape, Tuple.Point(2, 3, 4));
            Assert.AreEqual(c, new Color(1, 1.5, 2));
        }

        [TestMethod()]
        public void PatternAtShapeBothTransform() {
            Sphere shape = new Sphere();
            shape.Transform = Transformation.Scale(2, 2, 2);
            TestPattern pattern = new TestPattern();
            pattern.Transform = Transformation.Translate(0.5, 1, 1.5);
            Color c = pattern.PatternAtShape(shape, Tuple.Point(2.5, 3, 3.5));
            Assert.AreEqual(c, new Color(0.75, 0.5, 0.25));
        }

        //-------------------------------------------------------------------

        [TestMethod()]
        public void GradientPattern() {
            GradientPattern pattern = new GradientPattern(white, black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0.25, 0, 0)), new Color(0.75, 0.75, 0.75));
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0.5, 0, 0)), new Color(0.5, 0.5, 0.5));
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0.75, 0, 0)), new Color(0.25, 0.25, 0.25));
        }

        [TestMethod()]
        public void RingPattern() {
            RingPattern pattern = new RingPattern(white, black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(1, 0, 0)), black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 1)), black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0.708, 0, 0.708)), black);
        }

        /*
        [TestMethod()]
        public void Checker3DPatternX() {
            Checker3DPattern pattern = new Checker3DPattern(white, black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0.99, 0, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(1.01, 0, 0)), black);
        }

        [TestMethod()]
        public void Checker3DPatternY() {
            Checker3DPattern pattern = new Checker3DPattern(white, black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0.99, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 1.01, 0)), black);
        }

        [TestMethod()]
        public void Checker3DPatternZ() {
            Checker3DPattern pattern = new Checker3DPattern(white, black);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 0)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 0.99)), white);
            Assert.AreEqual(pattern.PatternAt(Tuple.Point(0, 0, 1.01)), black);
        }
        */
    }
}