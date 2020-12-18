using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Group : Shape {
        List<Shape> shapes = new List<Shape>();

        public Group() {
            ShapeType = "Group";
        }

        public List<Shape> Shapes {
            get { return shapes; }
            set { shapes = value; }
        }

        public void AddShape(Shape s) {
            Shapes.Add(s);
            s.Parent = this;
        }
        public override List<Intersection> LocalIntersect(Ray localRay) {
            List<Intersection> list = new List<Intersection>();
            foreach (Shape s in Shapes) {
                list.AddRange(s.Intersect(localRay));
            }
            list.Sort(delegate (Intersection x, Intersection y) { return x.T.CompareTo(y.T); });
            return list;
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection hit) {
            return Tuple.Vector(localPoint.X, 0, localPoint.Z);
        }
    }
}
