using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Ray {
        private Tuple origin;
        private Tuple direction;

        public Ray(Tuple origin, Tuple direction) {
            this.origin = origin;
            this.direction = direction;
        }

        public Tuple Origin {
            get { return origin; }
        }

        public Tuple Direction {
            get { return direction; }
        }

        public Tuple Position(double t) {
            return origin + (direction * t);
        }

        public Ray Transform(Matrix transformation) {
            return new Ray(transformation * origin, transformation * direction);
        }
    }
}
