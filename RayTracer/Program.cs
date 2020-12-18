// p 133
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RayTracer {
    class Program {
        static void Main(string[] args) {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("Hello World!");

            //Final1();

            /*
            string[] str = new string[3] { "union", "intersection", "difference" };
            int[,] intersectionIndices = new int[3, 2] {
                //lhit  inl   inr   result
                { 0, 3 },
                { 1, 2 },
                { 0, 1 }
            };
            Shape s1 = new Sphere();
            Shape s2 = new Cube();
            List<Intersection> xs = new List<Intersection>();
            xs.Add(new Intersection(1, s1));
            xs.Add(new Intersection(2, s2));
            xs.Add(new Intersection(3, s1));
            xs.Add(new Intersection(4, s2));
            for (int i = 0; i < 3; i++) {
                CSG c = new CSG(str[i], s1, s2);
                List<Intersection> result = c.FilterIntersections(xs);
                Console.WriteLine(result.Count);
                if (result.Count > 0) {
                    Console.WriteLine(result[0].T + " " + xs[intersectionIndices[i, 0]].T);
                    Console.WriteLine(result[1].T + " " + xs[intersectionIndices[i, 1]].T);
                }
                Console.WriteLine();
            }
            */

            OBJ();

            /*
            Group g = new Group();
            Shape s1 = new Sphere();
            Shape s2 = new Sphere();
            s2.Transform = Transformation.Translate(0, 0, -3);
            Shape s3 = new Sphere();
            s3.Transform = Transformation.Translate(5, 0, 0);
            g.AddShape(s1);
            g.AddShape(s2);
            g.AddShape(s3);
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = g.LocalIntersect(r);
            Console.WriteLine(xs.Count);
            foreach (Intersection i in xs) {
                Console.WriteLine(i.T + " " + i.S.Transform.GetMatrix[0, 3] + " " + i.S.Transform.GetMatrix[1, 3] + " " + i.S.Transform.GetMatrix[2, 3]);
            }
            */

            //GlassyPatternScene();
            //FancyPatternScene();
            //PatternScene();
            //SphereSceneWithPlane();
            //SphereScene();
            //Single3DSphere();
            //RedSphereSilhouette();

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds / 1000);
            Console.ReadKey();
        }

        public void Final1() {
            Plane floor = new Plane();
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;
            //floor.Material.Pattern = new Checker3DPattern(new Color(0.8, 0.2, 0.3), new Color(1, 1, 1));
            //floor.Material.Pattern.Transform = Transformation.Translate(100.0, 100.0, 100.0);
            floor.Material.Pattern = new RingPattern(new Color(0.8, 0.2, 0.3), new Color(1, 1, 1));
            floor.Material.Reflectivity = 0.5;
            floor.Material.Transparency = 0;

            Cylinder middle = new Cylinder();
            middle.Material.Color = new Color(1, 0, 1);
            middle.Material.Diffuse = 0.5;
            middle.Material.Specular = 0.3;
            middle.IsClosed = true;
            middle.Minimum = -10;
            middle.Maximum = 10;
            middle.Transform =
                Transformation.Scale(0.3, 0.3, 0.3) *
                Transformation.Rotate_X(Math.PI / 3) *
                Transformation.Translate(0, 0, 1);

            Cylinder middle2 = new Cylinder();
            middle2.Material.Color = new Color(1, 1, 0);
            middle2.Material.Diffuse = 0.5;
            middle2.Material.Specular = 0.3;
            middle2.IsClosed = true;
            middle.Minimum = -10;
            middle.Maximum = 10;
            middle2.Transform =
                Transformation.Scale(0.5, 0.5, 0.5) *
                Transformation.Rotate_X(Math.PI);

            Cube back = new Cube();
            back.Material.Color = new Color(1, 1, 1);
            back.Material.Diffuse = 0.5;
            back.Material.Specular = 0.3;
            back.Material.Reflectivity = 1.0;
            //back.Transform = Transformation.Translate(0, 1.5, 2);

            CSG c = new CSG("difference", back, middle);
            CSG c2 = new CSG("difference", c, middle2);
            c2.Transform = Transformation.Translate(-1, 1.2, 2);

            Cone right = new Cone();
            right.Transform =
                Transformation.Translate(0.5, 1, -0.5);
            right.Material.Color = new Color(0, 0, 1);
            right.Material.Diffuse = 0.5;
            right.Material.Specular = 0.3;
            right.Material.Transparency = 0.8;
            right.IsClosed = true;
            right.Minimum = -1;
            right.Maximum = 0;


            Cylinder left = new Cylinder();
            left.Transform =
                Transformation.Translate(-2.0, 0, -1.0) *
                Transformation.Scale(0.33, 0.33, 0.33);
            left.Material.Color = new Color(0, 1, 0);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;
            left.IsClosed = true;
            left.Minimum = 0;
            left.Maximum = 4;

            World w = new World();
            w.Light = Light.PointLight(Tuple.Point(-10, 10, -10), new Color(1, 1, 1));
            w.AddShape(floor);
            w.AddShape(c2);
            //w.AddShape(back);
            //w.AddShape(middle);
            w.AddShape(right);
            w.AddShape(left);

            Camera camera = new Camera(400, 200, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                    Tuple.Point(-2, 4, -7),
                    Tuple.Point(0, 1, 0),
                    Tuple.Vector(0, 1, 0)
                    );
            Canvas canvas = camera.RenderMultithread(w, 4);

            using (StreamWriter sw = new StreamWriter(@"C:\Users\prome\Desktop\GlassyPatternSceneSpeedTest.ppm", false, Encoding.ASCII, 65536)) {
                sw.Write(canvas.CanvasToPPM());
            }

            //System.IO.File.WriteAllText(@"C:\Users\prome\Desktop\Final1.ppm", canvas.CanvasToPPM());

            Console.WriteLine("File Written!");
        }

        public static void OBJ() {
            Plane floor = new Plane();
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Transform =
                Transformation.Translate(0, -5, 0);

            Plane rightWall = new Plane();
            rightWall.Transform =
                Transformation.Translate(0, 0, 15) *
                Transformation.Rotate_Y(Math.PI / 4) *
                Transformation.Rotate_X(Math.PI / 2);
            rightWall.Material.Color = new Color(0, 0.3, 0.5);

            string[] objInput = System.IO.File.ReadAllLines("C:\\Users\\prome\\source\\repos\\CS 4458\\RayTracer\\RayTracer\\objTests\\humanoid_tri.obj");
            OBJ_File parser = OBJ_File.ParseOBJFile(objInput);
            Group g = OBJ_File.OBJtoGroup(parser);
            g.Transform =
                Transformation.Translate(0, -7, 0) *
                Transformation.Rotate_Z(Math.PI / 2) *
                Transformation.Rotate_Y(Math.PI / 2);

            World w = new World();
            w.Light = Light.PointLight(Tuple.Point(-10, 10, -10), new Color(1, 1, 1));
            w.AddShape(floor);
            w.AddShape(rightWall);
            w.AddShape(g);

            Camera cameraOBJ = new Camera(200, 100, Math.PI / 3);
            cameraOBJ.Transform = Transformation.ViewTransform(
                Tuple.Point(10, 3, -40),
                Tuple.Point(0, 1, 0),
                Tuple.Vector(0, 1, 0)
                );

            Canvas canvas = cameraOBJ.RenderMultithread(w, 10);

            using (StreamWriter sw = new StreamWriter(@"C:\Users\prome\Desktop\OBJ.ppm", false, Encoding.ASCII, 65536)) {
                sw.Write(canvas.CanvasToPPM());
            }

            Console.WriteLine("File Written!");
        }

        public void GlassyPatternScene() {
            Plane floor = new Plane();
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;
            //floor.Material.Pattern = new Checker3DPattern(new Color(0.8, 0.2, 0.3), new Color(1, 1, 1));
            //floor.Material.Pattern.Transform = Transformation.Translate(100.0, 100.0, 100.0);
            floor.Material.Pattern = new RingPattern(new Color(0.8, 0.2, 0.3), new Color(1, 1, 1));
            floor.Material.Reflectivity = 0.5;
            floor.Material.Transparency = 0;

            Sphere middle = new Sphere();
            middle.Transform = Transformation.Translate(-0.5, 1, 0.5);
            middle.Material.Color = new Color(0, 0, 0);
            middle.Material.Diffuse = 0.0;
            middle.Material.Specular = 0.3;
            middle.Material.Transparency = 0.9;
            middle.Material.RefractiveIndex = 1.5;

            Cube back = new Cube();
            back.Transform = Transformation.Translate(-0.5, 1, 5);
            back.Material.Color = new Color(1, 1, 0);
            back.Material.Diffuse = 0.5;
            back.Material.Specular = 0.3;

            Sphere right = new Sphere();
            right.Transform =
                Transformation.Translate(1.5, 0.5, -0.5) *
                Transformation.Scale(0.5, 0.5, 0.5);
            right.Material.Color = new Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;
            right.Material.Pattern = new RingPattern(new Color(0, 0.4, 0.8), new Color(0, 0, 0));
            right.Material.Pattern.Transform = Transformation.Scale(0.3, 0.3, 0.3) * Transformation.Rotate_Z(-Math.PI / 5);

            Sphere left = new Sphere();
            left.Transform =
                Transformation.Translate(-1.5, 0.33, -0.75) *
                Transformation.Scale(0.33, 0.33, 0.33);
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;
            left.Material.Pattern = new StripedPattern(new Color(0.5, 1, 0.5), new Color(1, 1, 1));
            left.Material.Pattern.Transform = Transformation.Rotate_Z(Math.PI / 3) * Transformation.Scale(0.1, 0.1, 0.1);

            World w = new World();
            w.Light = Light.PointLight(Tuple.Point(-10, 10, -10), new Color(1, 1, 1));
            w.AddShape(floor);
            w.AddShape(middle);
            w.AddShape(back);
            w.AddShape(right);
            w.AddShape(left);

            Camera camera = new Camera(400, 200, Math.PI / 3);
            if (true) {
                camera.Transform = Transformation.ViewTransform(
                    Tuple.Point(3, 2, -7),
                    Tuple.Point(0, 1, 0),
                    Tuple.Vector(0, 1, 0)
                    );
            } else {
                camera.Transform = Transformation.ViewTransform(
                                Tuple.Point(0, 100, 0),
                                Tuple.Point(0, 0, 0),
                                Tuple.Vector(0, 0, 1)
                                );
            }
            Canvas canvas = camera.RenderMultithread(w, 4);

            using (StreamWriter sw = new StreamWriter(@"C:\Users\prome\Desktop\GlassyPatternSceneSpeedTest.ppm", false, Encoding.ASCII, 65536)) {
                sw.Write(canvas.CanvasToPPM());
            }

            //System.IO.File.WriteAllText(@"C:\Users\prome\Desktop\GlassyPatternScene.ppm", canvas.CanvasToPPM());

            Console.WriteLine("File Written!");
        }

        public void FancyPatternScene() {
            Plane floor = new Plane();
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;
            floor.Material.Pattern = new Checker3DPattern(new Color(0.8, 0.2, 0.3), new Color(1, 1, 1));
            floor.Material.Pattern.Transform = Transformation.Translate(100.0, 100.0, 100.0);
                //Transformation.Scale(0.5, 0.5, 0.5)
                //Transformation.Rotate_X(Math.PI / 4);

            Plane leftWall = new Plane();
            leftWall.Transform =
                Transformation.Translate(0, 0, 5) *
                Transformation.Rotate_Y(-Math.PI / 4) *
                Transformation.Rotate_X(Math.PI / 2);
            leftWall.Material.Color = new Color(1, 0.9, 0.9);
            leftWall.Material.Specular = 0;
            leftWall.Material.Pattern = new GradientPattern(new Color(1, 1, 1), new Color(0, 1, 0));
            leftWall.Material.Pattern.Transform = Transformation.Scale(2, 1, 1);

            Plane rightWall = new Plane();
            rightWall.Transform =
                Transformation.Translate(0, 0, 5) *
                Transformation.Rotate_Y(Math.PI / 4) *
                Transformation.Rotate_X(Math.PI / 2);
            rightWall.Material.Color = new Color(1, 0.9, 0.9);
            rightWall.Material.Specular = 0;
            rightWall.Material.Pattern = new Checker3DPattern(new Color(0.8, 0.2, 0.3), new Color(1, 1, 1));

            Sphere middle = new Sphere();
            middle.Transform = Transformation.Translate(-0.5, 1, 0.5);
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            middle.Material.Pattern = new StripedPattern(new Color(0.5, 1, 0.5), new Color(1, 1, 1));
            middle.Material.Pattern.Transform = Transformation.Rotate_Z(Math.PI / 3) * Transformation.Scale(0.1, 0.1, 0.1);

            Sphere right = new Sphere();
            right.Transform =
                Transformation.Translate(1.5, 0.5, -0.5) *
                Transformation.Scale(0.5, 0.5, 0.5);
            right.Material.Color = new Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;
            right.Material.Pattern = new RingPattern(new Color(0, 0.4, 0.8), new Color(0, 0, 0));
            right.Material.Pattern.Transform = Transformation.Scale(0.3, 0.3, 0.3) * Transformation.Rotate_Z(-Math.PI / 5);

            Sphere left = new Sphere();
            left.Transform =
                Transformation.Translate(-1.5, 0.33, -0.75) *
                Transformation.Scale(0.33, 0.33, 0.33);
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;
            left.Material.Pattern = new GradientPattern(new Color(0, 0.4, 0.8), new Color(1, 1, 0));
            left.Material.Pattern.Transform = Transformation.Scale(3.0, 1.0, 1.0);
            
            World w = new World();
            w.Light = Light.PointLight(Tuple.Point(-10, 10, -10), new Color(1, 1, 1));
            w.AddShape(floor);
            w.AddShape(leftWall);
            w.AddShape(rightWall);
            w.AddShape(left);
            w.AddShape(middle);
            w.AddShape(right);

            Camera camera = new Camera(200, 100, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 1.5, -5),
                Tuple.Point(0, 1, 0),
                Tuple.Vector(0, 1, 0)
                );

            Canvas canvas = camera.Render(w);

            System.IO.File.WriteAllText(@"C:\Users\prome\Desktop\FancyPatternScene.ppm", canvas.CanvasToPPM());

            Console.WriteLine("File Written!");
        }

        public void PatternScene() {
            Plane floor = new Plane();
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;
            floor.Material.Pattern = new StripedPattern(new Color(0.8, 0.2, 0.3), new Color(1, 1, 1));

            Plane leftWall = new Plane();
            leftWall.Transform =
                Transformation.Translate(0, 0, 5) *
                Transformation.Rotate_Y(-Math.PI / 4) *
                Transformation.Rotate_X(Math.PI / 2);
            leftWall.Material = floor.Material;

            Plane rightWall = new Plane();
            rightWall.Transform =
                Transformation.Translate(0, 0, 5) *
                Transformation.Rotate_Y(Math.PI / 4) *
                Transformation.Rotate_X(Math.PI / 2);
            rightWall.Material = floor.Material;

            Sphere middle = new Sphere();
            middle.Transform = Transformation.Translate(-0.5, 1, 0.5);
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            middle.Material.Pattern = new StripedPattern(new Color(0.5, 1, 0.5), new Color(1, 1, 1));
            middle.Material.Pattern.Transform = Transformation.Rotate_Z(Math.PI / 3) * Transformation.Scale(0.1, 0.1, 0.1);

            Sphere right = new Sphere();
            right.Transform =
                Transformation.Translate(1.5, 0.5, -0.5) *
                Transformation.Scale(0.5, 0.5, 0.5);
            right.Material.Color = new Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;
            right.Material.Pattern = new StripedPattern(new Color(0, 0.4, 0.8), new Color(0, 0, 0));
            right.Material.Pattern.Transform = Transformation.Scale(0.5, 0.5, 0.5) * Transformation.Rotate_Z(-Math.PI / 5);

            Sphere left = new Sphere();
            left.Transform =
                Transformation.Translate(-1.5, 0.33, -0.75) *
                Transformation.Scale(0.33, 0.33, 0.33);
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;

            World w = new World();
            w.Light = Light.PointLight(Tuple.Point(-10, 10, -10), new Color(1, 1, 1));
            w.AddShape(floor);
            w.AddShape(leftWall);
            w.AddShape(rightWall);
            w.AddShape(left);
            w.AddShape(middle);
            w.AddShape(right);

            Camera camera = new Camera(600, 300, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 1.5, -5),
                Tuple.Point(0, 1, 0),
                Tuple.Vector(0, 1, 0)
                );

            Canvas canvas = camera.Render(w);

            System.IO.File.WriteAllText(@"C:\Users\prome\Desktop\PatternScene.ppm", canvas.CanvasToPPM());

            Console.WriteLine("File Written!");
        }

        public void SphereSceneWithPlane() {
            Plane floor = new Plane();
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;

            /*
            Plane leftWall = new Plane();
            leftWall.Transform =
                Transformation.Translate(0, 0, 5) *
                Transformation.Rotate_Y(-Math.PI / 4) *
                Transformation.Rotate_X(Math.PI / 2);
            leftWall.Material = floor.Material;

            Plane rightWall = new Plane();
            rightWall.Transform =
                Transformation.Translate(0, 0, 5) *
                Transformation.Rotate_Y(Math.PI / 4) *
                Transformation.Rotate_X(Math.PI / 2);
            rightWall.Material = floor.Material;
            */

            Sphere middle = new Sphere();
            middle.Transform = Transformation.Translate(-0.5, 1, 0.5);
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;

            Sphere right = new Sphere();
            right.Transform =
                Transformation.Translate(1.5, 0.5, -0.5) *
                Transformation.Scale(0.5, 0.5, 0.5);
            right.Material.Color = new Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;

            Sphere left = new Sphere();
            left.Transform =
                Transformation.Translate(-1.5, 0.33, -0.75) *
                Transformation.Scale(0.33, 0.33, 0.33);
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;

            World w = new World();
            w.Light = Light.PointLight(Tuple.Point(-10, 10, -10), new Color(1, 1, 1));
            w.AddShape(floor);
            //w.AddShape(leftWall);
            //w.AddShape(rightWall);
            w.AddShape(left);
            w.AddShape(middle);
            w.AddShape(right);

            Camera camera = new Camera(400, 200, Math.PI / 2);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 1.5, -5),
                Tuple.Point(0, 1, 0),
                Tuple.Vector(0, 1, 0)
                );

            Canvas canvas = camera.Render(w);

            System.IO.File.WriteAllText(@"C:\Users\prome\Desktop\SphereSceneWithPlane2.ppm", canvas.CanvasToPPM());

            Console.WriteLine("File Written!");
        }

        public void SphereScene() {
            Sphere floor = new Sphere();
            floor.Transform = Transformation.Scale(10, 0.01, 10);
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;
            
            Sphere leftWall = new Sphere();
            leftWall.Transform = 
                Transformation.Translate(0, 0, 5) *
                Transformation.Rotate_Y(-Math.PI / 4) *
                Transformation.Rotate_X(Math.PI / 2) *
                Transformation.Scale(10, 0.01, 10);
            leftWall.Material = floor.Material;
            
            Sphere rightWall = new Sphere();
            rightWall.Transform =
                Transformation.Translate(0, 0, 5) *
                Transformation.Rotate_Y(Math.PI / 4) *
                Transformation.Rotate_X(Math.PI / 2) *
                Transformation.Scale(10, 0.01, 10);
            rightWall.Material = floor.Material;

            Sphere middle = new Sphere();
            middle.Transform = Transformation.Translate(-0.5, 1, 0.5);
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;

            Sphere right = new Sphere();
            right.Transform = 
                Transformation.Translate(1.5, 0.5, -0.5) *
                Transformation.Scale(0.5, 0.5, 0.5);
            right.Material.Color = new Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;
            
            Sphere left = new Sphere();
            left.Transform =
                Transformation.Translate(-1.5, 0.33, -0.75) *
                Transformation.Scale(0.33, 0.33, 0.33);
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;
            
            World w = new World();
            w.Light = Light.PointLight(Tuple.Point(-10, 10, -10), new Color(1, 1, 1));
            w.AddShape(floor);
            w.AddShape(leftWall);
            w.AddShape(rightWall);
            w.AddShape(left);
            w.AddShape(middle);
            w.AddShape(right);

            Camera camera = new Camera(200, 100, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 1.5, -5),
                Tuple.Point(0, 1, 0),
                Tuple.Vector(0, 1, 0)
                );

            Canvas canvas = camera.Render(w);

            System.IO.File.WriteAllText(@"C:\Users\prome\Desktop\SphereScene.ppm", canvas.CanvasToPPM());

            Console.WriteLine("File Written!");
        }

        public void Single3DSphere() {
            Tuple ray_origin = Tuple.Point(0, 0, -5);
            double wall_z = 10;
            double wall_size = 7.0;
            int canvas_pixels = 200;
            double pixel_size = wall_size / canvas_pixels;
            double half = wall_size / 2;

            Canvas canvas = new Canvas(canvas_pixels, canvas_pixels);
            Color color = new Color(1, 0, 0);
            Sphere shape = new Sphere();
            shape.Material.Color = new Color(0.1, 0.2, 0.4);
            shape.Material.Ambient = 0.1;
            shape.Material.Diffuse = 0.5;
            shape.Material.Specular = 0.2;
            shape.Material.Shininess = 100.0;
            //shape.Transform = Transformation.Rotate_Z(Math.PI/3) * Transformation.Scale(1, 0.5, 1);      // Transform

            Tuple lightPosition = Tuple.Point(-10, 10, -10);
            Color lightColor = new Color(1, 1, 1);
            Light light = new Light(lightPosition, lightColor);

            for (int y = 0; y < canvas_pixels; y++) {
                double world_y = half - pixel_size * y;

                for (int x = 0; x < canvas_pixels; x++) {
                    double world_x = -half + pixel_size * x;

                    Tuple position = Tuple.Point(world_x, world_y, wall_z);

                    Ray r = new Ray(ray_origin, Tuple.Normalize(position - ray_origin));
                    List<Intersection> xs = shape.Intersect(r);

                    Intersection hit = Intersection.Hit(xs);
                    if (hit != null) {
                        Tuple point = r.Position(hit.T);
                        Tuple normal = hit.S.NormalAt(point, hit);
                        Tuple eye = -r.Direction;
                        color = hit.S.Material.Lighting(hit.S, light, point, eye, normal, false);

                        canvas.WritePixel(x, y, color);
                    }
                }
            }

            System.IO.File.WriteAllText(@"C:\Users\prome\Desktop\Single3DSphere.ppm", canvas.CanvasToPPM());

            Console.WriteLine("File Written!");
        }

        public void RedSphereSilhouette() {
            Tuple ray_origin = Tuple.Point(0, 0, -5);
            double wall_z = 10;
            double wall_size = 7.0;
            int canvas_pixels = 100;
            double pixel_size = wall_size / canvas_pixels;
            double half = wall_size / 2;

            Canvas canvas = new Canvas(canvas_pixels, canvas_pixels);
            Color color = new Color(1, 0, 0);
            Sphere shape = new Sphere();
            shape.Transform = Transformation.Scale(1, 0.5, 1);      // Transform

            for (int y = 0; y < canvas_pixels; y++) {
                double world_y = half - pixel_size * y;

                for (int x = 0; x < canvas_pixels; x++) {
                    double world_x = -half + pixel_size * x;

                    Tuple position = Tuple.Point(world_x, world_y, wall_z);

                    Ray r = new Ray(ray_origin, Tuple.Normalize(position - ray_origin));
                    List<Intersection> xs = shape.Intersect(r);

                    if (Intersection.Hit(xs) != null) {
                        canvas.WritePixel(x, y, color);
                    }
                }
            }

            System.IO.File.WriteAllText(@"C:\Users\prome\Desktop\RayTracedSilhouette.ppm", canvas.CanvasToPPM());

            Console.WriteLine("File Written!");
        }

        public void TransformationOrderMatters() {
            Tuple p = Tuple.Point(1, 0, 1);
            Matrix a = Transformation.Rotate_X(Math.PI / 2);
            Matrix b = Transformation.Scale(5, 5, 5);
            Matrix c = Transformation.Translate(10, 5, 7);
            Matrix t = a * b * c;
            Tuple p2 = t * p;
            Console.WriteLine(p2.X + " " + p2.Y + " " + p2.Z);
        }

        public void MatrixMultiplicationExample() {
            Matrix m1 = new Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2);
            Matrix m2 = new Matrix(-2, 1, 2, 3, 3, 2, 1, -1, 4, 3, 6, 5, 1, 2, 7, 8);
            Matrix m3 = new Matrix(20, 22, 50, 48, 44, 54, 114, 108, 40, 58, 110, 102, 16, 26, 46, 42);
            Matrix m4 = m1 * m2;
            Console.WriteLine(m4.GetMatrix[0, 0] + " " + m4.GetMatrix[0, 1] + " " + m4.GetMatrix[0, 2] + " " + m4.GetMatrix[0, 3] + " " + Environment.NewLine);
            Console.WriteLine(m4.GetMatrix[1, 0] + " " + m4.GetMatrix[1, 1] + " " + m4.GetMatrix[1, 2] + " " + m4.GetMatrix[1, 3] + " " + Environment.NewLine);
            Console.WriteLine(m4.GetMatrix[2, 0] + " " + m4.GetMatrix[2, 1] + " " + m4.GetMatrix[2, 2] + " " + m4.GetMatrix[2, 3] + " " + Environment.NewLine);
            Console.WriteLine(m4.GetMatrix[3, 0] + " " + m4.GetMatrix[3, 1] + " " + m4.GetMatrix[3, 2] + " " + m4.GetMatrix[3, 3] + " " + Environment.NewLine);
        }
    }
}
