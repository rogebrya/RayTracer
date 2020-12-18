using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Checker3DPattern : Pattern {
        public Checker3DPattern(Color a, Color b) {
            A = a;
            B = b;
        }
        /*  Original
        (((Math.Floor(point.X) + Math.Floor(point.Y) + Math.Floor(point.Z)) % 2) == 0)
        */
        /* Weird Pattern
        ((Math.Floor(point.X + point.Y) % 2) == 0) && ((Math.Floor(point.X - point.Y) % 2) == 0) ||
        ((Math.Floor(point.Y + point.Z) % 2) == 0) && ((Math.Floor(point.Y - point.Z) % 2) == 0) ||
        ((Math.Floor(point.X + point.Z) % 2) == 0) && ((Math.Floor(point.X - point.Z) % 2) == 0))
        */
        /*  Works, but jagged edges and can't transform
        (((Math.Floor(point.X + point.Y + point.Z) % 2) == 0) && !((Math.Floor(point.X - point.Y - point.Z) % 2) == 0)) ||
        !((Math.Floor(point.X + point.Y + point.Z) % 2) == 0) && ((Math.Floor(point.X - point.Y - point.Z) % 2) == 0)
        */
        /*  Works, but symetric about each axis; HOWEVER, translating away from the axes works.
        (((Math.Floor(Math.Abs(point.X)) + Math.Floor(Math.Abs(point.Y)) + Math.Floor(Math.Abs(point.Z))) % 2) == 0)
        */
        public override Color PatternAt(Tuple point) {
            if (((Math.Floor(Math.Abs(point.X)) + Math.Floor(Math.Abs(point.Y)) + Math.Floor(Math.Abs(point.Z))) % 2) == 0) {
                return A;
            } else {
                return B;
            }
        }
    }
}
