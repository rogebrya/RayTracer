using System;
using System.Collections.Generic;
using System.Threading;

namespace RayTracer {
    public class Camera {
        private int hsize;
        private int vsize;
        private double fieldOfView;
        private Matrix transform = Matrix.GetIdentityMatrix();
        private double halfWidth;
        private double halfHeight;
        private double pixelSize;

        public Camera(int hsize, int vsize, double fieldOfView) {
            this.hsize = hsize;
            this.vsize = vsize;
            this.fieldOfView = fieldOfView;
            CalculatePixelSize();
        }

        public int Hsize {
            get { return hsize; }
            set { hsize = value; }
        }

        public int Vsize {
            get { return vsize; }
            set { vsize = value; }
        }

        public double FieldOfView {
            get { return fieldOfView; }
            set { fieldOfView = value; }
        }

        public Matrix Transform {
            get { return transform; }
            set { transform = value; }
        }

        public double HalfWidth {
            get { return halfWidth; }
            set { halfWidth = value; }
        }

        public double HalfHeight {
            get { return halfHeight; }
            set { halfHeight = value; }
        }

        public double PixelSize {
            get { return pixelSize; }
            set { pixelSize = value; }
        }

        private void CalculatePixelSize() {
            double halfView = Math.Tan(fieldOfView / 2);
            double aspect = (double)hsize / vsize;
            if (aspect >= 1) {
                halfWidth = halfView;
                halfHeight = halfView / aspect;
            } else {
                halfWidth = halfView * aspect;
                halfHeight = halfView;
            }
            pixelSize = (halfWidth * 2) / hsize;
        }

        public Ray RayForPixel(int px, int py) {
            double xoffset = (px + 0.5) * pixelSize;
            double yoffset = (py + 0.5) * pixelSize;

            double worldX = halfWidth - xoffset;
            double worldY = halfHeight - yoffset;

            Tuple pixel = transform.Inverse() * Tuple.Point(worldX, worldY, -1);
            Tuple origin = transform.Inverse() * Tuple.Point(0, 0, 0);
            Tuple direction = Tuple.Normalize(pixel - origin);

            return new Ray(origin, direction);
        }

        public Canvas Render(World w) {
            Canvas image = new Canvas(hsize, vsize);
            for(int y = 0; y < vsize; y++) {
                for(int x = 0; x < hsize; x++) {
                    //Console.WriteLine("Pixels Rendered: " + (y * hsize + x + 1) + " / " + (hsize * vsize));
                    Ray ray = RayForPixel(x, y);
                    Color color = w.ColorAt(ray, 5);
                    image.WritePixel(x, y, color);
                }
            }
            return image;
        }

        public Canvas RenderMultithread(World w, int numThreads = 0) {
            if (numThreads == 0) {
                return Render(w);
            } else if (vsize % numThreads != 0) {
                Console.WriteLine("Invalid thread count specified; proceeding with single threaded algorithm.");
                return Render(w);
            } else {
                Canvas image = new Canvas(hsize, vsize);
                List<Thread> threads = new List<Thread>();
                for (int i = 0; i < numThreads; i++) {
                    int ymin = (int)(vsize * ((double)i / numThreads));
                    int ymax = (int)(vsize * (((double)i + 1) / numThreads));
                    Thread t = new Thread(new ThreadStart(() => ThreadJob(w, image, ymin, ymax, 0, hsize)));
                    t.Start();
                    threads.Add(t);
                }
                foreach(var thread in threads) {
                    thread.Join();
                }
                return image;
            }
        }

        public void ThreadJob(World w, Canvas image, int ymin, int ymax, int xmin, int xmax) {
            for (int y = ymin; y < ymax; y++) {
                for (int x = xmin; x < xmax; x++) {
                    //Console.WriteLine("Pixels Rendered: " + (y * hsize + x + 1) + " / " + (hsize * vsize));
                    Ray ray = RayForPixel(x, y);
                    Color color = w.ColorAt(ray, 5);
                    image.WritePixel(x, y, color);
                }
            }
        }
    }
}
