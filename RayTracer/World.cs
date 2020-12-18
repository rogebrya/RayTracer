using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class World {
        List<Shape> shapes;
        Light light;

        public World() {

        }

        public World(List<Shape> shapes, Light light) {
            this.shapes = shapes;
            this.light = light;
        }

        public static World DefaultWorld() {
            List<Shape> sList = new List<Shape>();
            Sphere s1 = new Sphere();
            s1.Material.Color = new Color(0.8, 1.0, 0.6);
            s1.Material.Diffuse = 0.7;
            s1.Material.Specular = 0.2;
            Sphere s2 = new Sphere();
            s2.Transform = Transformation.Scale(0.5, 0.5, 0.5);
            sList.Add(s1);
            sList.Add(s2);
            Light l = Light.PointLight(Tuple.Point(-10, 10, -10), new Color(1, 1, 1));
            return new World(sList, l);
        }

        public List<Shape> Shapes {
            get { return shapes; }
            set { shapes = value; }
        }

        public Light Light {
            get { return light; }
            set { light = value; }
        }

        public void AddShape(Shape s) {
            if (shapes == null) {
                shapes = new List<Shape>();
            }
            shapes.Add(s);
        }

        public List<Intersection> IntersectWorld(Ray r) {
            List<Intersection> list = new List<Intersection>();
            foreach (Shape s in shapes) {
                List<Intersection> partialList = s.Intersect(r);
                if (partialList != null) {
                    list.AddRange(partialList);
                }
            }
            list.Sort(delegate (Intersection x, Intersection y) { return x.T.CompareTo(y.T); });
            return list;
        }

        // Possibly change lighting call point -> overpoint to fix checkboard pattern
        public Color ShadeHit(Computations comps, int remaining) {
            bool shadowed = IsShadowed(comps.overPoint);
            Color surfaceColor = comps.shape.Material.Lighting(comps.shape, light, comps.overPoint, comps.eyev, comps.normalv, shadowed);
            Color reflectedColor = ReflectedColor(comps, remaining);
            Color refractedColor = RefractedColor(comps, remaining);

            Material material = comps.shape.Material;
            if (material.Reflectivity > 0 && material.Transparency > 0) {
                double reflectance = Intersection.Schlick(comps);
                return surfaceColor + reflectedColor * reflectance + refractedColor * (1.0 - reflectance);
            }
            return surfaceColor + reflectedColor + refractedColor;
        }

        public Color ColorAt(Ray r, int remaining) {
            List<Intersection> xs = IntersectWorld(r);
            Intersection i = Intersection.Hit(xs);
            if (i == null) {
                return new Color(0, 0, 0);
            } else {
                Computations comps = Computations.PrepareComputations(i, r, xs);
                return ShadeHit(comps, remaining);
            }
        }

        public bool IsShadowed(Tuple point) {
            Tuple v = light.Position - point;
            double distance = Tuple.Magnitude(v);
            Tuple direction = Tuple.Normalize(v);

            Ray r = new Ray(point, direction);
            List<Intersection> intersections = IntersectWorld(r);

            Intersection h = Intersection.Hit(intersections);
            if (h != null && h.T < distance) {
                return true;
            } else {
                return false;
            }
        }

        public Color ReflectedColor(Computations comps, int remaining) {
            if (remaining <= 0 || comps.shape.Material.Reflectivity == 0) {
                return new Color(0, 0, 0);
            } else {
                Ray reflectRay = new Ray(comps.overPoint, comps.reflectv);
                Color color = ColorAt(reflectRay, remaining - 1);
                return color * comps.shape.Material.Reflectivity;
            }
        }

        public Color RefractedColor(Computations comps, int remaining) {
            if (remaining <= 0 || comps.shape.Material.Transparency == 0) {
                return new Color(0, 0, 0);
            } else {
                double nRatio = comps.n1 / comps.n2;
                double cosI = Tuple.Dot(comps.eyev, comps.normalv);
                double sin2T = Math.Pow(nRatio, 2.0) * (1.0 - Math.Pow(cosI, 2.0));
                if (sin2T > 1) {
                    return new Color(0, 0, 0);
                }

                double cosT = Math.Sqrt(1.0 - sin2T);
                Tuple direction = comps.normalv * (nRatio * cosI - cosT) - comps.eyev * nRatio;
                Ray refractedRay = new Ray(comps.underPoint, direction);
                Color color = ColorAt(refractedRay, remaining - 1) * comps.shape.Material.Transparency;
                return color;
            }
        }
    }
}
