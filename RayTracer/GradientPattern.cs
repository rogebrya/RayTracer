using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class GradientPattern : Pattern {
        public GradientPattern(Color a, Color b) {
            A = a;
            B = b;
        }

        public override Color PatternAt(Tuple point) {
            Color distance = B - A;
            double fraction = point.X - Math.Floor(point.X);
            return A + distance * fraction;
        }
    }
}
