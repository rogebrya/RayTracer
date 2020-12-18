using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public abstract class Pattern {
        private Color a;
        private Color b;
        private Matrix transform = Matrix.GetIdentityMatrix();

        public Color A {
            get { return a; }
            set { a = value; }
        }

        public Color B {
            get { return b; }
            set { b = value; }
        }

        public Matrix Transform {
            get { return transform; }
            set { transform = value; }
        }

        public abstract Color PatternAt(Tuple point);

        // Probably fixed for groups
        public Color PatternAtShape(Shape s, Tuple worldPoint) {
            Tuple objectPoint = s.WorldToObject(worldPoint);
            Tuple patternPoint = Transform.Inverse() * objectPoint;

            return PatternAt(patternPoint);
        }
    }

    public class TestPattern : Pattern {
        public TestPattern() {

        }

        public override Color PatternAt(Tuple point) {
            return new Color(point.X, point.Y, point.Z);
        }
    }
}
