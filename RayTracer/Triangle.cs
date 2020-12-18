using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Triangle : Shape {
        private Tuple p1;
        private Tuple p2;
        private Tuple p3;
        private Tuple e1;
        private Tuple e2;
        private Tuple normal;

        public Triangle(Tuple point1, Tuple point2, Tuple point3) {
            p1 = point1;
            p2 = point2;
            p3 = point3;
            e1 = p2 - p1;
            e2 = p3 - p1;
            normal = Tuple.Normalize(Tuple.Cross(e2, e1));
            ShapeType = "Triangle";
        }

        public Tuple P1 {
            get { return p1; }
            set { p1 = value; }
        }

        public Tuple P2 {
            get { return p2; }
            set { p2 = value; }
        }

        public Tuple P3 {
            get { return p3; }
            set { p3 = value; }
        }

        public Tuple E1 {
            get { return e1; }
            set { e1 = value; }
        }

        public Tuple E2 {
            get { return e2; }
            set { e2 = value; }
        }

        public Tuple Normal {
            get { return normal; }
            set { normal = value; }
        }

        public override List<Intersection> LocalIntersect(Ray localRay) {
            List<Intersection> list = new List<Intersection>();

            Tuple dirCrossE2 = Tuple.Cross(localRay.Direction, E2);
            double det = Tuple.Dot(E1, dirCrossE2);
            if (Math.Abs(det) < EPSILON) {
                return list;
            }
            double f = 1.0 / det;
            Tuple p1ToOrigin = localRay.Origin - P1;
            double u = f * Tuple.Dot(p1ToOrigin, dirCrossE2);
            if (u < 0 || u > 1) {
                return list;
            }
            Tuple originCrossE1 = Tuple.Cross(p1ToOrigin, E1);
            double v = f * Tuple.Dot(localRay.Direction, originCrossE1);
            if (v < 0 || (u + v) > 1) {
                return list;
            }
            double t = f * Tuple.Dot(E2, originCrossE1);
            list.Add(new Intersection(t, this));
            return list;
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection hit) {
            return Normal;
        }
    }
}
