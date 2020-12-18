using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Tuple : IEquatable<Tuple> {
        private const double EPSILON = 0.0001;

        protected double x;
        protected double y;
        protected double z;
        protected double w;

        public Tuple(double x, double y, double z, double w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Tuple Vector(double x, double y, double z) {
            return new Tuple(x, y, z, 0.0);
        }

        public static Tuple Point(double x, double y, double z) {
            return new Tuple(x, y, z, 1.0);
        }

        public double X {
            get { return x; }
            set { x = value; }
        }

        public double Y {
            get { return y; }
            set { y = value; }
        }

        public double Z {
            get { return z; }
            set { z = value; }
        }

        public double W {
            get { return w; }
            set { w = value; }
        }

        public double[] GetTuple {
            get { return new double[4] { x, y, z, w }; }
        }

        public bool IsVector() {
            if (EqualityOfDouble(w, 0.0)) {
                return true;
            } else {
                return false;
            }
        }

        public bool IsPoint() {
            if (EqualityOfDouble(w, 1.0)) {
                return true;
            } else {
                return false;
            }
        }

        // Operations
        public static Tuple operator +(Tuple t1, Tuple t2) {
            return new Tuple((t1.x + t2.x), (t1.y + t2.y), (t1.z + t2.z), (t1.w + t2.w));
        }

        public static Tuple operator -(Tuple t1, Tuple t2) {
            return new Tuple((t1.x - t2.x), (t1.y - t2.y), (t1.z - t2.z), (t1.w - t2.w));
        }

        public static Tuple operator -(Tuple t1) {
            return new Tuple(-1 * t1.x, -1 * t1.y, -1 * t1.z, -1 * t1.w);
        }

        public static Tuple operator *(Tuple t1, double d1) {
            return new Tuple(d1 * t1.x, d1 * t1.y, d1 * t1.z, d1 * t1.w);
        }

        public static Tuple operator /(Tuple t1, double d1) {
            return new Tuple(t1.x / d1, t1.y / d1, t1.z / d1, t1.w / d1);
        }

        public static double Magnitude(Tuple t1) {
            return Math.Sqrt(Math.Pow(t1.x, 2) + Math.Pow(t1.y, 2) + Math.Pow(t1.z, 2) + Math.Pow(t1.w, 2));
        }

        public static Tuple Normalize(Tuple t1) {
            double mag = Magnitude(t1);
            return new Tuple(t1.x / mag, t1.y / mag, t1.z / mag, t1.w / mag);
        }

        public static double Dot(Tuple t1, Tuple t2) {
            return (t1.x * t2.x) + (t1.y * t2.y) + (t1.z * t2.z) + (t1.w * t2.w);
        }

        public static Tuple Cross(Tuple t1, Tuple t2) {
            return Vector((t1.y * t2.z) - (t1.z * t2.y), (t1.z * t2.x) - (t1.x * t2.z), (t1.x * t2.y) - (t1.y * t2.x));
        }

        public static Tuple Reflect(Tuple incoming, Tuple normal) {
            Tuple t = incoming - normal * 2 * Dot(incoming, normal);
            return Vector(t.X, t.Y, t.Z);
        }

        // Equality Stuff
        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }

        public static bool operator ==(Tuple t1, Tuple t2) {
            return EqualityOfDouble(t1.x, t2.x) && EqualityOfDouble(t1.y, t2.y) && EqualityOfDouble(t1.z, t2.z) && EqualityOfDouble(t1.w, t2.w);
        }

        public static bool operator !=(Tuple t1, Tuple t2) {
            return !EqualityOfDouble(t1.x, t2.x) || !EqualityOfDouble(t1.y, t2.y) || !EqualityOfDouble(t1.z, t2.z) || !EqualityOfDouble(t1.w, t2.w);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tuple)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (int)Math.Pow(((x*z) * 397), y) + (int)w;
            }
        }

        public bool Equals(Tuple other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityOfDouble(x, other.x) && EqualityOfDouble(y, other.y) && EqualityOfDouble(z, other.z) && EqualityOfDouble(w, other.w);
        }
    }
}
