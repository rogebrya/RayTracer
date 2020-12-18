using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class SmoothTriangle : Shape {
        private Tuple p1;
        private Tuple p2;
        private Tuple p3;
        private Tuple e1;
        private Tuple e2;
        private Tuple n1;
        private Tuple n2;
        private Tuple n3;

        public SmoothTriangle(Tuple point1, Tuple point2, Tuple point3, Tuple normal1, Tuple normal2, Tuple normal3) {
            p1 = point1;
            p2 = point2;
            p3 = point3;
            n1 = normal1;
            n2 = normal2;
            n3 = normal3;
            e1 = p2 - p1;
            e2 = p3 - p1;
            ShapeType = "SmoothTriangle";
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

        public Tuple N1 {
            get { return n1; }
            set { n1 = value; }
        }

        public Tuple N2 {
            get { return n2; }
            set { n2 = value; }
        }

        public Tuple N3 {
            get { return n3; }
            set { n3 = value; }
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
            list.Add(new Intersection(t, this, u, v));
            return list;
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection hit) {
            return 
                N2 * hit.U +
                N3 * hit.V +
                N1 * (1 - hit.U - hit.V);
        }
    }
}
