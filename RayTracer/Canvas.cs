using System;
using System.Text;

namespace RayTracer {
    public class Canvas {
        private int width;
        private int height;
        private Color[,] pixels;

        public Canvas(int width, int height) {
            this.width = width;
            this.height = height;
            pixels = new Color[width,height];

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    pixels[i,j] = new Color(0, 0, 0);
                }
            }
        }

        public int Width {
            get { return width; }
        }

        public int Height {
            get { return height; }
        }

        public Color[,] Pixels {
            get { return pixels; }
        }

        public void WritePixel(int w, int h, Color color) {
            pixels[w, h] = color;
        }

        public Color PixelAt(int w, int h) {
            return pixels[w, h];
        }

        public string CanvasToPPM() {
            StringBuilder sb = new StringBuilder();
            PPM_Header(sb);
            PPM_Body_Split_Lines(sb);
            return sb.ToString(); 
        }

        public void PPM_Header(StringBuilder sb) {
            sb.Append(
                "P3" + Environment.NewLine +
                width + " " + height + Environment.NewLine +
                "255" + Environment.NewLine);
        }

        public string PPM_Body() {
            string output = "";

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    output += 
                        PPM_BodyScaledColor(pixels[j, i].Red) + " " +
                        PPM_BodyScaledColor(pixels[j, i].Green) + " " +
                        PPM_BodyScaledColor(pixels[j, i].Blue);
                    if (j != width - 1) {
                        output += " ";
                    }
                }
                output += Environment.NewLine;
            }
            return output;
        }

        private int PPM_BodyScaledColor(double c) {
            int output = (int)Math.Round(c * 255);
            if (output > 255) {
                output = 255;
            } else if (output < 0) {
                output = 0;
            }
            return output;
        }

        public void PPM_Body_Split_Lines(StringBuilder sb) {
            int count = 0;
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    count++;
                    sb.Append(PPM_BodyScaledColor(pixels[j, i].Red) + " ");
                    sb.Append(PPM_BodyScaledColor(pixels[j, i].Green) + " ");
                    sb.Append(PPM_BodyScaledColor(pixels[j, i].Blue));
                    if (count % 5 == 0) {
                        sb.Append(Environment.NewLine);
                    } else {
                        sb.Append(" ");
                    }
                }
            }
                    /*
                    string output = "";

                    for (int i = 0; i < height; i++) {
                        int count = 0;
                        for (int j = 0; j < width; j++) {
                            output += PPM_BodyScaledColor(pixels[j, i].Red);
                            count += 4;
                            if (count >= 66) { output += Environment.NewLine; count = 0; }
                            else { output += " "; }
                            output += PPM_BodyScaledColor(pixels[j, i].Green);
                            count += 4;
                            if (count >= 66) { output += Environment.NewLine; count = 0; }
                            else { output += " "; }
                            output += PPM_BodyScaledColor(pixels[j, i].Blue);
                            count += 3;
                            if (count >= 66) { output += Environment.NewLine; count = 0; } 
                            if (j != width - 1 ) {
                                output += " ";
                                count += 1;
                            }
                        }
                        output += Environment.NewLine;
                    }
                    return output;
                    */
        }
    }
}
