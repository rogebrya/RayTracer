using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class CSG : Shape {
        private string operation;
        private Shape left;
        private Shape right;

        public CSG(string operation, Shape left, Shape right) {
            this.operation = operation;
            this.left = left;
            this.right = right;
            left.Parent = this;
            right.Parent = this;
            ShapeType = "CSG";
        }

        public string Operation {
            get { return operation; }
            set { operation = value; }
        }

        public Shape Left {
            get { return left; }
            set { left = value; }
        }

        public Shape Right {
            get { return right; }
            set { right = value; }
        }

        public override List<Intersection> LocalIntersect(Ray localRay) {
            List<Intersection> xs = new List<Intersection>();
            List<Intersection> leftXS = Left.Intersect(localRay);
            List<Intersection> rightXS = Right.Intersect(localRay);
            xs.AddRange(leftXS);
            xs.AddRange(rightXS);
            xs.Sort(delegate (Intersection x, Intersection y) { return x.T.CompareTo(y.T); });
            return FilterIntersections(xs);
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection hit) {
            return null;
        }

        public static bool IntersectionAllowed(string op, bool lhit, bool inl, bool inr) {
            if (op == "union") {
                return (lhit && !inr) || (!lhit && !inl);
            } else if (op == "intersection") {
                return (lhit && inr) || (!lhit && inl);
            } else if (op == "difference") {
                return (lhit && !inr) || (!lhit && inl);
            }
            return false;
        }

        public List<Intersection> FilterIntersections(List<Intersection> xs) {
            bool inl = false;
            bool inr = false;
            List<Intersection> list = new List<Intersection>();

            foreach (Intersection i in xs) {
                bool lhit = Includes(Left, i.S);
                if (IntersectionAllowed(Operation, lhit, inl, inr)) {
                    list.Add(i);
                }
                if (lhit) {
                    inl = !inl;
                } else {
                    inr = !inr;
                }
            }
            return list;
        }

        public bool Includes(Shape a, Shape b) {
            if (a is Group one) {
                foreach(Shape i in one.Shapes) {
                    if (Includes(i, b)) {
                        return true;
                    }
                }
            } else if (a is CSG two) {
                if (Includes(two.Left, b) || Includes(two.Right, b)) {
                    return true;
                }
            } else if (a == b) {
                return true;
            }
            return false;
        }
    }
}
