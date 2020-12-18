using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Sphere : Shape {
        public Sphere() {
            ShapeType = "Sphere";
        }

        public static Sphere GlassSphere() {
            Sphere s = new Sphere();
            s.Material.Transparency = 1.0;
            s.Material.RefractiveIndex = 1.5;
            return s;
        }

        public override List<Intersection> LocalIntersect(Ray localRay) {
            List<Intersection> list = new List<Intersection>();
            Tuple sphereToRay = localRay.Origin - Tuple.Point(0, 0, 0);

            double a = Tuple.Dot(localRay.Direction, localRay.Direction);
            double b = 2 * Tuple.Dot(localRay.Direction, sphereToRay);
            double c = Tuple.Dot(sphereToRay, sphereToRay) - 1;

            double discriminant = Math.Pow(b, 2) - 4 * a * c;

            if (discriminant >= 0) {
                list.Add(new Intersection((-b - Math.Sqrt(discriminant)) / (2 * a), this));
                list.Add(new Intersection((-b + Math.Sqrt(discriminant)) / (2 * a), this));
            }
            return list;
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection hit) {
            return localPoint - Tuple.Point(0, 0, 0);
        }

        // Equality Stuff
        public static bool operator ==(Sphere t1, Sphere t2) {
            return t1.Transform == t2.Transform && t1.Material == t2.Material;
        }

        public static bool operator !=(Sphere t1, Sphere t2) {
            return !(t1.Transform == t2.Transform && t1.Material == t2.Material);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Sphere)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (int)Math.Pow((Transform.Determinant() * 397), Material.Ambient) + (int)Material.Shininess;
            }
        }

        public bool Equals(Sphere other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Transform == other.Transform && Material == other.Material;
        }
    }
}
