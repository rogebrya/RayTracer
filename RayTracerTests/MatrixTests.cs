using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Tests {
    [TestClass()]
    public class MatrixTests {
        [TestMethod()]
        public void MatrixTest44() {
            Matrix m = new Matrix(1, 2, 3, 4, 5.5, 6.5, 7.5, 8.5, 9, 10, 11, 12, 13.5, 14.5, 15.5, 16.5);
            Assert.AreEqual(1, m.GetMatrix[0, 0]);
            Assert.AreEqual(4, m.GetMatrix[0, 3]);
            Assert.AreEqual(5.5, m.GetMatrix[1, 0]);
            Assert.AreEqual(7.5, m.GetMatrix[1, 2]);
            Assert.AreEqual(11, m.GetMatrix[2, 2]);
            Assert.AreEqual(13.5, m.GetMatrix[3, 0]);
            Assert.AreEqual(15.5, m.GetMatrix[3, 2]);
        }

        [TestMethod()]
        public void MatrixTest22() {
            Matrix m = new Matrix(-3, 5, 1, -2);
            Assert.AreEqual(-3, m.GetMatrix[0, 0]);
            Assert.AreEqual(5, m.GetMatrix[0, 1]);
            Assert.AreEqual(1, m.GetMatrix[1, 0]);
            Assert.AreEqual(-2, m.GetMatrix[1, 1]);
        }

        [TestMethod()]
        public void MatrixTest33() {
            Matrix m = new Matrix(-3, 5, 0, 1, -2, -7, 0, 1, 1);
            Assert.AreEqual(-3, m.GetMatrix[0, 0]);
            Assert.AreEqual(-2, m.GetMatrix[1, 1]);
            Assert.AreEqual(1, m.GetMatrix[2, 2]);
        }

        [TestMethod()]
        public void MatrixEquality() {
            Matrix m1 = new Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2);
            Matrix m2 = new Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2);
            Assert.AreEqual(m1, m2);
            Assert.IsTrue(m1 == m2);
        }

        [TestMethod()]
        public void MatrixInequality() {
            Matrix m1 = new Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2);
            Matrix m2 = new Matrix(2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2, 1);
            Assert.AreNotEqual(m1, m2);
            Assert.IsTrue(m1 != m2);
        }

        [TestMethod()]
        public void MatrixMultiplication() {
            Matrix m1 = new Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2);
            Matrix m2 = new Matrix(-2, 1, 2, 3, 3, 2, 1, -1, 4, 3, 6, 5, 1, 2, 7, 8);
            Matrix m3 = new Matrix(20, 22, 50, 48, 44, 54, 114, 108, 40, 58, 110, 102, 16, 26, 46, 42);
            Assert.AreEqual(m1 * m2, m3);
        }

        [TestMethod()]
        public void MatrixMultiplyTuple() {
            Matrix m1 = new Matrix(1, 2, 3, 4, 2, 4, 4, 2, 8, 6, 4, 1, 0, 0, 0, 1);
            Tuple t1 = new Tuple(1, 2, 3, 1);
            Tuple t2 = new Tuple(18, 24, 33, 1);
            Assert.AreEqual(m1 * t1, t2);
        }

        [TestMethod()]
        public void MatrixIdentityMultiplication() {
            Matrix m1 = new Matrix(1, 2, 3, 4, 1, 2, 4, 8, 2, 4, 8, 16, 4, 8, 16, 32);
            Assert.AreEqual(m1 * Matrix.GetIdentityMatrix(), m1);
            Assert.AreEqual(Matrix.GetIdentityMatrix() * m1, m1);

            Tuple t1 = new Tuple(1, 2, 3, 4);
            Assert.AreEqual(Matrix.GetIdentityMatrix() * t1, t1);
        }

        [TestMethod()]
        public void MatrixTranspose() {
            Matrix m1 = new Matrix(0, 9, 3, 0, 9, 8, 0, 8, 1, 8, 5, 3, 0, 0, 5, 8);
            Matrix m2 = new Matrix(0, 9, 1, 0, 9, 8, 8, 0, 3, 0, 5, 5, 0, 8, 3, 8);
            Assert.AreEqual(m1.TransposeMatrix(), m2);

            Assert.AreEqual(Matrix.GetIdentityMatrix().TransposeMatrix(), Matrix.GetIdentityMatrix());
        }

        [TestMethod()]
        public void MatrixDeterminant22() {
            Matrix m1 = new Matrix(1, 5, -3, 2);
            Assert.AreEqual(m1.Determinant(), 17);
        }

        [TestMethod()]
        public void MatrixSubmatrix() {
            Matrix m1 = new Matrix(1, 5, 0, -3, 2, 7, 0, 6, -3);
            Matrix m2 = new Matrix(-3, 2, 0, 6);
            Assert.AreEqual(m1.Submatrix(0, 2), m2);

            Matrix m3 = new Matrix(-6, 1, 1, 6, -8, 5, 8, 6, -1, 0, 8, 2, -7, 1, -1, 1);
            Matrix m4 = new Matrix(-6, 1, 6, -8, 8, 6, -7, -1, 1);
            Assert.AreEqual(m3.Submatrix(2, 1), m4);
        }

        [TestMethod()]
        public void MatrixMinor33() {
            Matrix m1 = new Matrix(3, 5, 0, 2, -1, -7, 6, -1, 5);
            Matrix m2 = m1.Submatrix(1, 0);
            Assert.AreEqual(m2.Determinant(), 25);
            Assert.AreEqual(m1.Minor(1, 0), 25);
        }

        [TestMethod()]
        public void MatrixCofactor33() {
            Matrix m1 = new Matrix(3, 5, 0, 2, -1, -7, 6, -1, 5);
            Assert.AreEqual(m1.Minor(0, 0), -12);
            Assert.AreEqual(m1.Cofactor(0, 0), -12);
            Assert.AreEqual(m1.Minor(1, 0), 25);
            Assert.AreEqual(m1.Cofactor(1, 0), -25);
        }

        [TestMethod()]
        public void MatrixDeterminant3344() {
            Matrix m1 = new Matrix(1, 2, 6, -5, 8, -4, 2, 6, 4);
            Assert.AreEqual(m1.Cofactor(0, 0), 56);
            Assert.AreEqual(m1.Cofactor(0, 1), 12);
            Assert.AreEqual(m1.Cofactor(0, 2), -46);
            Assert.AreEqual(m1.Determinant(), -196);

            Matrix m2 = new Matrix(-2, -8, 3, 5, -3, 1, 7, 3, 1, 2, -9, 6, -6, 7, 7, -9);
            Assert.AreEqual(m2.Cofactor(0, 0), 690);
            Assert.AreEqual(m2.Cofactor(0, 1), 447);
            Assert.AreEqual(m2.Cofactor(0, 2), 210);
            Assert.AreEqual(m2.Cofactor(0, 3), 51);
            Assert.AreEqual(m2.Determinant(), -4071);
        }

        [TestMethod()]
        public void MatrixIsInvertable() {
            Matrix m1 = new Matrix(6, 4, 4, 4, 5, 5, 7, 6, 4, -9, 3, -7, 9, 1, 7, -6);
            Assert.AreEqual(m1.Determinant(), -2120);
            Assert.IsTrue(m1.IsInvertable());

            Matrix m2 = new Matrix(-4, 2, -2, -3, 9, 6, 2, 6, 0, -5, 1, -5, 0, 0, 0, 0);
            Assert.AreEqual(m2.Determinant(), 0);
            Assert.IsFalse(m2.IsInvertable());
        }

        [TestMethod()]
        public void MatrixInverse() {
            Matrix m1 = new Matrix(-5, 2, 6, -8, 1, -5, 1, 8, 7, 7, -6, -7, 1, -3, 7, 4);
            Matrix m2 = m1.Inverse();
            Assert.AreEqual(m1.Determinant(), 532);
            Assert.AreEqual(m1.Cofactor(2, 3), -160);
            Assert.AreEqual(m2.GetMatrix[3,2], -160/532.0);
            Assert.AreEqual(m1.Cofactor(3, 2), 105);
            Assert.AreEqual(m2.GetMatrix[2, 3], 105/532.0);

            Matrix m3 = new Matrix(0.21805, 0.45113, 0.24060, -0.04511, -0.80827, -1.45677, -0.44361, 0.52068, -0.07895, -0.22368, -0.05263, 0.19737, -0.52256, -0.81391, -0.30075, 0.30639);
            Assert.AreEqual(m2, m3);
        }

        [TestMethod()]
        public void MatrixMoreInverses() {
            Matrix m1 = new Matrix(8, -5, 9, 2, 7, 5, 6, 1, -6, 0, 9, 6, -3, 0, -9, -4);
            Matrix m2 = new Matrix(-0.15385, -0.15385, -0.28205, -0.53846, -0.07692, 0.12308, 0.02564, 0.03077, 0.35897, 0.35897, 0.43590, 0.92308, -0.69231, -0.69231, -0.76923, -1.92308);
            Assert.AreEqual(m1.Inverse(), m2);

            Matrix m3 = new Matrix(9, 3, 0, 9, -5, -2, -6, -3, -4, 9, 6, 4, -7, 6, 6, 2);
            Matrix m4 = new Matrix(-0.04074, -0.07778, 0.14444, -0.22222, -0.07778, 0.03333, 0.36667, -0.33333, -0.02901, -0.14630, -0.10926, 0.12963, 0.17778, 0.06667, -0.26667, 0.33333);
            Assert.AreEqual(m3.Inverse(), m4);
        }

        [TestMethod()]
        public void MatrixMultiplyInverses() {
            Matrix m1 = new Matrix(3, -9, 7, 3, 3, -8, 2, -9, -4, 4, 4, 1, -6, 5, -1, 1);
            Matrix m2 = new Matrix(8, 2, 2, 2, 3, -1, 7, 0, 7, 0, 5, 4, 6, -2, 0, 5);
            Matrix m3 = m1 * m2;
            Assert.AreEqual(m3 * m2.Inverse(), m1);
        }
    }
}