using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Intersection {
        private const double EPSILON = 0.0001;
        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }

        private double t;
        private Shape s;
        private double u;   // Triangles
        private double v;   // Triangles

        public Intersection(double t, Shape s) {
            this.t = t;
            this.s = s;
        }

        public Intersection(double t, Shape s, double u, double v) {
            this.t = t;
            this.s = s;
            this.u = u;
            this.v = v;
        }

        public double T {
            get { return t; }
        }

        public Shape S {
            get { return s; }
        }

        public double U {
            get { return u; }
            set { u = value; }
        }

        public double V {
            get { return v; }
            set { v = value; }
        }

        public static List<Intersection> Intersections(Intersection i1, Intersection i2) {
            List<Intersection> list = new List<Intersection>();
            list.Add(i1);
            list.Add(i2);
            return list;
        }

        public static void Intersections(List<Intersection> iList, Intersection i) {
            iList.Add(i);
        }

        public static Intersection Hit(List<Intersection> list) {
            if (list.Count == 0) {
                return null;
            } else {
                double minForewardDistance = Double.MaxValue;
                Intersection hit = null;
                foreach (Intersection i in list) {
                    if (i.T > 0.0 && i.T < minForewardDistance) {
                        minForewardDistance = i.T;
                        hit = i;
                    }
                }
                return hit;
            }
        }

        public static double Schlick(Computations comps) {
            double cos = Tuple.Dot(comps.eyev, comps.normalv);
            if (comps.n1 > comps.n2) {
                double n = comps.n1 / comps.n2;
                double sin2T = Math.Pow(n, 2.0) * (1.0 - Math.Pow(cos, 2.0));
                if (sin2T > 1.0) {
                    return 1.0;
                }
                double cosT = Math.Sqrt(1.0 - sin2T);
                cos = cosT;
            }
            double r0 = Math.Pow(((comps.n1 - comps.n2) / (comps.n1 + comps.n2)), 2.0);
            return r0 + (1 - r0) * Math.Pow((1 - cos), 5.0);
        }
    }

    public class Computations {
        private const double EPSILON = 0.0001;

        public double t;
        public Shape shape;
        public Tuple point;
        public Tuple eyev;
        public bool inside;
        public Tuple normalv;
        public Tuple overPoint;
        public Tuple reflectv;
        public double n1;
        public double n2;
        public Tuple underPoint;

        private Computations() {

        }

        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }

        public static Computations PrepareComputations(Intersection i, Ray r, List<Intersection> xs = null) {
            if (xs == null) {
                xs = new List<Intersection>();
                xs.Add(i);
            }
            Computations comps = new Computations();
            comps.t = i.T;
            comps.shape = i.S;
            comps.point = r.Position(comps.t);
            comps.eyev = -r.Direction;
            comps.normalv = comps.shape.NormalAt(comps.point, i);
            if (Tuple.Dot(comps.normalv, comps.eyev) < 0) {
                comps.inside = true;
                comps.normalv = -comps.normalv;
            } else {
                comps.inside = false;
            }
            comps.reflectv = Tuple.Reflect(r.Direction, comps.normalv);
            comps.overPoint = comps.point + comps.normalv * EPSILON;
            comps.underPoint = comps.point - comps.normalv * EPSILON;

            List<Shape> containers = new List<Shape>();
            foreach (Intersection j in xs) {
                if (EqualityOfDouble(j.T, i.T) && j.S == i.S) {
                    if (containers.Count == 0) {
                        comps.n1 = 1.0;
                    } else {
                        comps.n1 = containers[containers.Count - 1].Material.RefractiveIndex;
                    }
                }
                if (containers.Contains(j.S)) {
                    containers.Remove(j.S);
                } else {
                    containers.Add(j.S);
                }
                if (EqualityOfDouble(j.T, i.T) && j.S == i.S) {
                    if (containers.Count == 0) {
                        comps.n2 = 1.0;
                    } else {
                        comps.n2 = containers[containers.Count - 1].Material.RefractiveIndex;
                    }
                    break;
                }
            }
            return comps;
        }
    }
}
