using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class OBJ_File {
        private int linesIgnored = 0;
        private List<Tuple> verticies = new List<Tuple>();
        private Group defaultGroup = new Group();
        private List<Group> groups = new List<Group>();
        private List<Tuple> normals = new List<Tuple>();

        public OBJ_File() {

        }

        public int LinesIgnored {
            get { return linesIgnored; }
            set { linesIgnored = value; }
        }

        public List<Tuple> Verticies {
            get { return verticies; }
            set { verticies = value; }
        }

        public Group DefaultGroup {
            get { return defaultGroup; }
            set { defaultGroup = value; }
        }

        public List<Group> Groups {
            get { return groups; }
            set { groups = value; }
        }

        public List<Tuple> Normals {
            get { return normals; }
            set { normals = value; }
        }

        public static OBJ_File ParseOBJFile(string[] file) {
            int c = 0;
            OBJ_File objFile = new OBJ_File();
            objFile.Verticies.Add(null);
            objFile.Normals.Add(null);
            foreach (string s in file) {
                c++;
                Console.WriteLine("OBJ Processing: Line# " + c + " / " + file.Length);
                string[] currentLine = s.Split(' ');
                if (currentLine[0] == "v") {
                    objFile.Verticies.Add(Tuple.Point(double.Parse(currentLine[1]), double.Parse(currentLine[2]), double.Parse(currentLine[3])));
                } else if (currentLine[0] == "vn") {
                    objFile.Normals.Add(Tuple.Vector(double.Parse(currentLine[1]), double.Parse(currentLine[2]), double.Parse(currentLine[3])));
                } else if (currentLine[0] == "f") {
                    List<Tuple> verts = new List<Tuple>();
                    verts.Add(null);
                    List<Tuple> norms = new List<Tuple>();
                    norms.Add(null);
                    for (int i = 1; i < currentLine.Length; i++) {
                        string[] temp = currentLine[i].Split("/");
                        int.TryParse(temp[0], out int result1);
                        if (temp.Length > 1) {
                            int.TryParse(temp[2], out int result2);
                            norms.Add(objFile.Normals[result2]);
                        }
                        verts.Add(objFile.Verticies[result1]);
                    }
                    if (objFile.Normals.Count > 1) {
                        List<SmoothTriangle> triangles = objFile.FanSmoothTriangulation(verts, norms);
                        foreach (SmoothTriangle t in triangles) {
                            objFile.DefaultGroup.AddShape(t);
                        }
                    } else {
                        List<Triangle> triangles = objFile.FanTriangulation(verts);
                        foreach (Triangle t in triangles) {
                            objFile.DefaultGroup.AddShape(t);
                        }
                    }
                } else if (currentLine[0] == "g") {
                    if (objFile.DefaultGroup.Shapes.Count != 0) {
                        objFile.Groups.Add(objFile.DefaultGroup);
                        objFile.DefaultGroup = new Group();
                    }
                } else {
                    objFile.LinesIgnored++;
                }
            }
            if (objFile.DefaultGroup.Shapes.Count != 0) {
                objFile.Groups.Add(objFile.DefaultGroup);
            }
            return objFile;
        }

        public List<Triangle> FanTriangulation(List<Tuple> verts) {
            List <Triangle> triangles = new List<Triangle>();

            for (int i = 2; i < verts.Count - 1; i++) {
                try { triangles.Add(new Triangle(verts[1], verts[i], verts[i + 1])); } 
                catch { }
            }

            return triangles;
        }

        public List<SmoothTriangle> FanSmoothTriangulation(List<Tuple> verts, List<Tuple> norms) {
            List<SmoothTriangle> triangles = new List<SmoothTriangle>();

            for (int i = 2; i < verts.Count - 1; i++) {
                try { triangles.Add(new SmoothTriangle(verts[1], verts[i], verts[i + 1], norms[1], norms[i], norms[i + 1])); } catch { }
            }

            return triangles;
        }

        public static Group OBJtoGroup(OBJ_File objFile) {
            Group masterGroup = new Group();
            foreach (Group g in objFile.Groups) {
                masterGroup.AddShape(g);
            }
            return masterGroup;
        }
    }
}
