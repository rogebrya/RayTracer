using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Plane : Shape {

        public Plane() {
            ShapeType = "Plane";
        }

        public override List<Intersection> LocalIntersect(Ray localRay) {
            if (EqualityOfDouble(Math.Abs(localRay.Direction.Y), 0)) {
                return null;
            } else {
                double t = -localRay.Origin.Y / localRay.Direction.Y;
                List<Intersection> xs = new List<Intersection>();
                xs.Add(new Intersection(t, this));
                return xs;
            }
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection hit) {
            return Tuple.Vector(0, 1, 0);
        }
    }
}
