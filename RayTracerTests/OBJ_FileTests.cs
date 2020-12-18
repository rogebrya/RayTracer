using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class OBJ_FileTests {
        [TestMethod()]
        public void IgnoreUnrecognizedLines() {
            // Test Text:
            /*
            There was a young lady named Bright
            who traveled much faster than light.
            She set out one day
            in a relative way,
            and came back the previous night.
            */
            string[] objInput = System.IO.File.ReadAllLines("C:\\Users\\prome\\source\\repos\\CS 4458\\RayTracer\\RayTracer\\objTests\\objTest1.txt");
            OBJ_File parser = OBJ_File.ParseOBJFile(objInput);
            Assert.AreEqual(parser.LinesIgnored, 5);
        }

        [TestMethod()]
        public void ParseVertexRecords() {
            // Test Text:
            /*
            v -1 1 0
            v -1.0000 0.5000 0.0000
            v 1 0 0
            v 1 1 0
            */
            string[] objInput = System.IO.File.ReadAllLines("C:\\Users\\prome\\source\\repos\\CS 4458\\RayTracer\\RayTracer\\objTests\\objTest2.txt");
            OBJ_File parser = OBJ_File.ParseOBJFile(objInput);
            Assert.AreEqual(parser.LinesIgnored, 0);
            Assert.AreEqual(parser.Verticies[1], Tuple.Point(-1, 1, 0));
            Assert.AreEqual(parser.Verticies[2], Tuple.Point(-1, 0.5, 0));
            Assert.AreEqual(parser.Verticies[3], Tuple.Point(1, 0, 0));
            Assert.AreEqual(parser.Verticies[4], Tuple.Point(1, 1, 0));
        }

        [TestMethod()]
        public void MakeTriangles() {
            // Test Text:
            /*
            v -1 1 0
            v -1 0 0
            v 1 0 0
            v 1 1 0
            f 1 2 3
            f 1 3 4
            */
            string[] objInput = System.IO.File.ReadAllLines("C:\\Users\\prome\\source\\repos\\CS 4458\\RayTracer\\RayTracer\\objTests\\objTest3.txt");
            OBJ_File parser = OBJ_File.ParseOBJFile(objInput);
            Group g = parser.DefaultGroup;
            Triangle t1 = (Triangle)g.Shapes[0];
            Triangle t2 = (Triangle)g.Shapes[1];
            Assert.AreEqual(t1.P1, parser.Verticies[1]);
            Assert.AreEqual(t1.P2, parser.Verticies[2]);
            Assert.AreEqual(t1.P3, parser.Verticies[3]);
            Assert.AreEqual(t2.P1, parser.Verticies[1]);
            Assert.AreEqual(t2.P2, parser.Verticies[3]);
            Assert.AreEqual(t2.P3, parser.Verticies[4]);
        }

        [TestMethod()]
        public void MakePolygons() {
            // Test Text:
            /*
            v -1 1 0 
            v -1 0 0 
            v 1 0 0 
            v 1 1 0 
            v 0 2 0 
            f 1 2 3 4 5
            */
            string[] objInput = System.IO.File.ReadAllLines("C:\\Users\\prome\\source\\repos\\CS 4458\\RayTracer\\RayTracer\\objTests\\objTest4.txt");
            OBJ_File parser = OBJ_File.ParseOBJFile(objInput);
            Group g = parser.DefaultGroup;
            Triangle t1 = (Triangle)g.Shapes[0];
            Triangle t2 = (Triangle)g.Shapes[1];
            Triangle t3 = (Triangle)g.Shapes[2];
            Assert.AreEqual(t1.P1, parser.Verticies[1]);
            Assert.AreEqual(t1.P2, parser.Verticies[2]);
            Assert.AreEqual(t1.P3, parser.Verticies[3]);
            Assert.AreEqual(t2.P1, parser.Verticies[1]);
            Assert.AreEqual(t2.P2, parser.Verticies[3]);
            Assert.AreEqual(t2.P3, parser.Verticies[4]);
            Assert.AreEqual(t3.P1, parser.Verticies[1]);
            Assert.AreEqual(t3.P2, parser.Verticies[4]);
            Assert.AreEqual(t3.P3, parser.Verticies[5]);
        }

        [TestMethod()]
        public void NamedGroups() {
            // Test Text:
            /*
            v -1 1 0
            v -1 0 0
            v 1 0 0
            v 1 1 0
            g FirstGroup
            f 1 2 3
            g SecondGroup
            f 1 3 4
            */
            string[] objInput = System.IO.File.ReadAllLines("C:\\Users\\prome\\source\\repos\\CS 4458\\RayTracer\\RayTracer\\objTests\\objTest5.txt");
            OBJ_File parser = OBJ_File.ParseOBJFile(objInput);
            Group g1 = parser.Groups[0];
            Group g2 = parser.Groups[1];
            Triangle t1 = (Triangle)g1.Shapes[0];
            Triangle t2 = (Triangle)g2.Shapes[0];
            Assert.AreEqual(t1.P1, parser.Verticies[1]);
            Assert.AreEqual(t1.P2, parser.Verticies[2]);
            Assert.AreEqual(t1.P3, parser.Verticies[3]);
            Assert.AreEqual(t2.P1, parser.Verticies[1]);
            Assert.AreEqual(t2.P2, parser.Verticies[3]);
            Assert.AreEqual(t2.P3, parser.Verticies[4]);

        }

        [TestMethod()]
        public void OBJ_ModelToGroup() {
            // Test Text:
            /*
            v -1 1 0
            v -1 0 0
            v 1 0 0
            v 1 1 0
            g FirstGroup
            f 1 2 3
            g SecondGroup
            f 1 3 4
            */
            string[] objInput = System.IO.File.ReadAllLines("C:\\Users\\prome\\source\\repos\\CS 4458\\RayTracer\\RayTracer\\objTests\\objTest5.txt");
            OBJ_File parser = OBJ_File.ParseOBJFile(objInput);
            Group g = OBJ_File.OBJtoGroup(parser);
            Group g1 = (Group)g.Shapes[0];
            Group g2 = (Group)g.Shapes[1];
            Triangle t1 = (Triangle)g1.Shapes[0];
            Triangle t2 = (Triangle)g2.Shapes[0];
            Assert.AreEqual(t1.P1, parser.Verticies[1]);
            Assert.AreEqual(t1.P2, parser.Verticies[2]);
            Assert.AreEqual(t1.P3, parser.Verticies[3]);
            Assert.AreEqual(t2.P1, parser.Verticies[1]);
            Assert.AreEqual(t2.P2, parser.Verticies[3]);
            Assert.AreEqual(t2.P3, parser.Verticies[4]);
        }

        [TestMethod()]
        public void OBJ_VertexNormalRecords() {
            // Test Text:
            /*
            vn 0 0 1
            vn 0.707 0 -0.707
            vn 1 2 3
            */
            string[] objInput = System.IO.File.ReadAllLines("C:\\Users\\prome\\source\\repos\\CS 4458\\RayTracer\\RayTracer\\objTests\\objTest6.txt");
            OBJ_File parser = OBJ_File.ParseOBJFile(objInput);
            Assert.AreEqual(parser.Normals[1], Tuple.Vector(0, 0, 1));
            Assert.AreEqual(parser.Normals[2], Tuple.Vector(0.707, 0, -0.707));
            Assert.AreEqual(parser.Normals[3], Tuple.Vector(1, 2, 3));
        }

        [TestMethod()]
        public void OBJ_FacesWithNormalVectors() {
            // Test Text:
            /*
            v 0 1 0
            v -1 0 0
            v 1 0 0
            vn -1 0 0
            vn 1 0 0
            vn 0 1 0
            f 1//3 2//1 3//2
            f 1/0/3 2/102/1 3/14/2
            */
            string[] objInput = System.IO.File.ReadAllLines("C:\\Users\\prome\\source\\repos\\CS 4458\\RayTracer\\RayTracer\\objTests\\objTest7.txt");
            OBJ_File parser = OBJ_File.ParseOBJFile(objInput);
            Group g = parser.Groups[0];
            SmoothTriangle t1 = (SmoothTriangle)g.Shapes[0];
            SmoothTriangle t2 = (SmoothTriangle)g.Shapes[1];
            Assert.AreEqual(t1.P1, parser.Verticies[1]);
            Assert.AreEqual(t1.P2, parser.Verticies[2]);
            Assert.AreEqual(t1.P3, parser.Verticies[3]);
            Assert.AreEqual(t1.N1, parser.Normals[3]);
            Assert.AreEqual(t1.N2, parser.Normals[1]);
            Assert.AreEqual(t1.N3, parser.Normals[2]);
            Assert.AreEqual(t2, t1);
        }
    }
}
 