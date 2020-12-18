using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class StripedPattern : Pattern {
        public StripedPattern(Color a, Color b) {
            A = a;
            B = b;
        }

        public override Color PatternAt(Tuple point) {
            if (Math.Floor(point.X) % 2 == 0) {
                return A;
            } else {
                return B;
            }
        }
    }
}
