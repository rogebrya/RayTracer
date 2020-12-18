using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Color : Tuple {
        public Color(double r, double g, double b)
            : base(r, g, b, 0) {
        }

        public double Red {
            get { return x; }
        }

        public double Green {
            get { return y; }
        }

        public double Blue {
            get { return z; }
        }

        public static Color operator +(Color t1, Color t2) {
            return new Color((t1.Red + t2.Red), (t1.Green + t2.Green), (t1.Blue + t2.Blue));
        }

        public static Color operator -(Color t1, Color t2) {
            return new Color((t1.Red - t2.Red), (t1.Green - t2.Green), (t1.Blue - t2.Blue));
        }

        public static Color operator *(Color t1, Color t2) {
            return new Color(t1.Red * t2.Red, t1.Green * t2.Green, t1.Blue * t2.Blue);
        }

        public static Color operator *(Color t1, double d1) {
            return new Color(d1 * t1.Red, d1 * t1.Green, d1 * t1.Blue);
        }
    }
}
