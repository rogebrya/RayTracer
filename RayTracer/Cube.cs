using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Cube : Shape {
        public Cube() {
            ShapeType = "Cube";
        }

        public static Cube GlassCube() {
            Cube c = new Cube();
            c.Material.Transparency = 1.0;
            c.Material.RefractiveIndex = 1.5;
            return c;
        }

        public override List<Intersection> LocalIntersect(Ray localRay) {
            List<Intersection> list = new List<Intersection>();
            double[] x = new double[2];
            double[] y = new double[2];
            double[] z = new double[2];

            x = CheckAxis(localRay.Origin.X, localRay.Direction.X);
            y = CheckAxis(localRay.Origin.Y, localRay.Direction.Y);
            z = CheckAxis(localRay.Origin.Z, localRay.Direction.Z);

            double tmin = Math.Max(Math.Max(x[0], y[0]), z[0]);
            double tmax = Math.Min(Math.Min(x[1], y[1]), z[1]);

            if (tmin > tmax) {
                return list;
            }
            list.Add(new Intersection(tmin, this));
            list.Add(new Intersection(tmax, this));
            return list;
        }

        public double[] CheckAxis(double origin, double direction) {
            double tmin;
            double tmax;
            double tminNumerator = (-1.0 - origin);
            double tmaxNumerator = (1.0 - origin);

            if (Math.Abs(direction) >= EPSILON) {
                tmin = tminNumerator / direction;
                tmax = tmaxNumerator / direction;
            } else {
                tmin = tminNumerator * Double.MaxValue;
                tmax = tmaxNumerator * Double.MaxValue;
            }

            if (tmin > tmax) {
                double temp = tmin;
                tmin = tmax;
                tmax = temp;
            }

            return new double[2] { tmin, tmax };
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection hit) {
            double maxc = Math.Max(Math.Max(Math.Abs(localPoint.X), Math.Abs(localPoint.Y)), Math.Abs(localPoint.Z));
            if (EqualityOfDouble(maxc, Math.Abs(localPoint.X))) {
                return Tuple.Vector(localPoint.X, 0, 0);
            } else if (EqualityOfDouble(maxc, Math.Abs(localPoint.Y))) {
                return Tuple.Vector(0, localPoint.Y, 0);
            } else {
                return Tuple.Vector(0, 0, localPoint.Z);
            }
        }

        // Equality Stuff
        public static bool operator ==(Cube t1, Cube t2) {
            return t1.Transform == t2.Transform && t1.Material == t2.Material;
        }

        public static bool operator !=(Cube t1, Cube t2) {
            return !(t1.Transform == t2.Transform && t1.Material == t2.Material);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Cube)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (int)Math.Pow((Transform.Determinant() * 391), Material.Ambient) + (int)Material.Shininess;
            }
        }

        public bool Equals(Cube other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Transform == other.Transform && Material == other.Material;
        }
    }
}
