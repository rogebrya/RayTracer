using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public abstract class Shape {
        protected const double EPSILON = 0.0001;

        private Matrix transform = Matrix.GetIdentityMatrix();
        private Material material = new Material();
        private Shape parent;
        private string shapeType;

        public Matrix Transform {
            get { return transform; }
            set { transform = value; }
        }

        public Material Material {
            get { return material; }
            set { material = value; }
        }

        public Shape Parent {
            get { return parent; }
            set { parent = value; }
        }

        public string ShapeType {
            get { return shapeType; }
            set { shapeType = value; }
        }

        public List<Intersection> Intersect(Ray r) {
            Ray localRay = r.Transform(transform.Inverse());

            return LocalIntersect(localRay);
        }

        public abstract List<Intersection> LocalIntersect(Ray localRay);

        public Tuple NormalAt(Tuple point, Intersection hit) {
            Tuple localPoint = WorldToObject(point);
            Tuple localNormal = LocalNormalAt(localPoint, hit);
            return NormalToWorld(localNormal);
        }

        public abstract Tuple LocalNormalAt(Tuple localPoint, Intersection hit);

        public bool IsAShape() {
            return GetType().IsSubclassOf(typeof(Shape))
           || GetType() == typeof(Shape);
        }

        public Tuple WorldToObject(Tuple point) {
            if (!(Parent is null)) {
                point = Parent.WorldToObject(point);
            }
            return Transform.Inverse() * point;
        }

        public Tuple NormalToWorld(Tuple normal) {
            normal = Transform.Inverse().TransposeMatrix() * normal;
            normal.W = 0;
            normal = Tuple.Normalize(normal);
            if (!(Parent is null)) {
                normal = Parent.NormalToWorld(normal);
            }
            return normal;
        }

        // Equality Stuff
        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }
        
        public static bool operator ==(Shape t1, Shape t2) {
            return t1.Transform == t2.Transform && t1.Material == t2.Material && t1.ShapeType == t2.ShapeType;
        }

        public static bool operator !=(Shape t1, Shape t2) {
            return !(t1.Transform == t2.Transform && t1.Material == t2.Material && t1.ShapeType == t2.ShapeType);
        }
        
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Shape)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (int)Math.Pow((Transform.Determinant() * 397), Material.Ambient) + (int)Material.Shininess;
            }
        }

        public bool Equals(Shape other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Transform == Transform && other.Material == other.Material && ShapeType == other.ShapeType;
        }
    }

    public class TestShape : Shape {
        public Ray savedRay;
        public Tuple savedNormal;

        public TestShape() {
            ShapeType = "TestShape";
        }

        public override List<Intersection> LocalIntersect(Ray localRay) {
            savedRay = localRay;
            return null;
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection hit) {
            savedNormal = localPoint;
            return localPoint - Tuple.Point(0, 0, 0); ;
        }
    }
}
