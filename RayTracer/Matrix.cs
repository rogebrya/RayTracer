using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Matrix : IEquatable<Matrix> {
        private const double EPSILON = 0.00001;

        private double[,] matrix;
        int size;

        public Matrix() { }

        public Matrix(double m00, double m01, double m10, double m11) {
            matrix = new double[2, 2];
            size = 2;
            matrix[0, 0] = m00;
            matrix[0, 1] = m01;
            matrix[1, 0] = m10;
            matrix[1, 1] = m11;
        }

        public Matrix(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22) {
            matrix = new double[3, 3];
            size = 3;
            matrix[0, 0] = m00;
            matrix[0, 1] = m01;
            matrix[0, 2] = m02;
            matrix[1, 0] = m10;
            matrix[1, 1] = m11;
            matrix[1, 2] = m12;
            matrix[2, 0] = m20;
            matrix[2, 1] = m21;
            matrix[2, 2] = m22;
        }

        public Matrix(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33) {
            matrix = new double[4, 4];
            size = 4;
            matrix[0, 0] = m00;
            matrix[0, 1] = m01;
            matrix[0, 2] = m02;
            matrix[0, 3] = m03;
            matrix[1, 0] = m10;
            matrix[1, 1] = m11;
            matrix[1, 2] = m12;
            matrix[1, 3] = m13;
            matrix[2, 0] = m20;
            matrix[2, 1] = m21;
            matrix[2, 2] = m22;
            matrix[2, 3] = m23;
            matrix[3, 0] = m30;
            matrix[3, 1] = m31;
            matrix[3, 2] = m32;
            matrix[3, 3] = m33;
        }

        public double[,] GetMatrix {
            get { return matrix; }
            //set { matrix = value; }
        }

        public static Matrix operator *(Matrix m1, Matrix m2) {
            Matrix m3 = new Matrix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    m3.matrix[i, j] = 
                        m1.matrix[i, 0] * m2.matrix[0, j] + 
                        m1.matrix[i, 1] * m2.matrix[1, j] + 
                        m1.matrix[i, 2] * m2.matrix[2, j] + 
                        m1.matrix[i, 3] * m2.matrix[3, j];
                }
            }
            return m3;
        }

        public static Tuple operator *(Matrix m1, Tuple t1) {
            double[] t2 = new double[4] { 0, 0, 0, 0 };
            for (int i = 0; i < 4; i++) {
                t2[i] =
                        m1.matrix[i, 0] * t1.GetTuple[0] +
                        m1.matrix[i, 1] * t1.GetTuple[1] +
                        m1.matrix[i, 2] * t1.GetTuple[2] +
                        m1.matrix[i, 3] * t1.GetTuple[3];
            }
            return new Tuple(t2[0], t2[1], t2[2], t2[3]);
        }

        public static Matrix GetIdentityMatrix() {
            return new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        }

        public Matrix TransposeMatrix() {
            return new Matrix(
                matrix[0, 0], matrix[1, 0], matrix[2, 0], matrix[3, 0],
                matrix[0, 1], matrix[1, 1], matrix[2, 1], matrix[3, 1],
                matrix[0, 2], matrix[1, 2], matrix[2, 2], matrix[3, 2],
                matrix[0, 3], matrix[1, 3], matrix[2, 3], matrix[3, 3]);
        }

        public double Determinant() {
            double det = 0;
            if (size == 2) {
                det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            } else {
                for (int i = 0; i < size; i++) {
                    det += matrix[0, i] * Cofactor(0, i);
                }
            }
            return det;
        }

        public Matrix Submatrix(int row, int col) {
            double[] m;
            int k = 0;
            if (size == 3) {
                m = new double[4];
                for (int i = 0; i < size; i++) {
                    for (int j = 0; j < size; j++) {
                        if (!(i == row || j == col)) {
                            m[k] = matrix[i, j];
                            k++;
                        }
                    }
                }
                return new Matrix(
                    m[0], m[1],
                    m[2], m[3]);
            } else if (size == 4) {
                m = new double[9];
                for (int i = 0; i < size; i++) {
                    for (int j = 0; j < size; j++) {
                        if (!(i == row || j == col)) {
                            m[k] = matrix[i, j];
                            k++;
                        }
                    }
                }
                return new Matrix(
                    m[0], m[1], m[2],
                    m[3], m[4], m[5],
                    m[6], m[7], m[8]);
            }
            return this;
        }

        public double Minor(int row, int col) {
            return Submatrix(row, col).Determinant();
        }

        public double Cofactor(int row, int col) {
            if ((row + col) % 2 == 0) {
                return Minor(row, col);
            } else {
                return -Minor(row, col);
            }
        }

        public bool IsInvertable() {
            return (Determinant() != 0);
        }

        public Matrix Inverse() {
            if (IsInvertable()) {
                Matrix m = GetIdentityMatrix();
                for (int i = 0; i < size; i++) {
                    for (int j = 0; j < size; j++) {
                        m.GetMatrix[j, i] = Cofactor(i, j) / Determinant();
                    }
                }
                return m;
            } else {    // Error
                return this;
            }
        }

        // Equality Stuff
        public static bool EqualityOfDouble(double a, double b) {
            return Math.Abs(a - b) < EPSILON;
        }

        public static bool operator ==(Matrix m1, Matrix m2) {
            bool output = true;
            if (m1.size == m2.size) {
                for (int i = 0; i < m1.size; i++) {
                    for (int j = 0; j < m1.size; j++) {
                        if (!EqualityOfDouble(m1.matrix[i, j], m2.matrix[i, j])) {
                            return false;
                        }
                    }
                }
            } else {
                output = false;
            }
            return output;
        }

        public static bool operator !=(Matrix m1, Matrix m2) {
            bool output = false;
            if (m1.size == m2.size) {
                for (int i = 0; i < m1.size; i++) {
                    for (int j = 0; j < m1.size; j++) {
                        if (!EqualityOfDouble(m1.matrix[i, j], m2.matrix[i, j])) {
                            return true;
                        }
                    }
                }
            } else {
                output = true;
            }
            return output;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Matrix)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (int)Math.Pow(((matrix[0,0] * matrix[1, 1]) * 397), size);
            }
        }

        public bool Equals(Matrix other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            bool output = true;
            if (size == other.size) {
                for (int i = 0; i < size; i++) {
                    for (int j = 0; j < size; j++) {
                        if (!EqualityOfDouble(matrix[i, j], other.matrix[i, j])) {
                            return false;
                        }
                    }
                }
            } else {
                output = false;
            }
            return output;
        }
    }
}
