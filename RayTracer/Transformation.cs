using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer {
    public class Transformation : Matrix {
        public static Matrix Translate(double x, double y, double z) {
            Matrix m = Matrix.GetIdentityMatrix();
            m.GetMatrix[0, 3] = x;
            m.GetMatrix[1, 3] = y;
            m.GetMatrix[2, 3] = z;
            return m;
        }

        public static Matrix Scale(double x, double y, double z) {
            Matrix m = Matrix.GetIdentityMatrix();
            m.GetMatrix[0, 0] = x;
            m.GetMatrix[1, 1] = y;
            m.GetMatrix[2, 2] = z;
            return m;
        }
        
        public static Matrix Rotate_X(double theta) {
            Matrix m = Matrix.GetIdentityMatrix();
            m.GetMatrix[1, 1] = Math.Cos(theta);
            m.GetMatrix[1, 2] = -Math.Sin(theta);
            m.GetMatrix[2, 1] = Math.Sin(theta);
            m.GetMatrix[2, 2] = Math.Cos(theta);
            return m;
        }

        public static Matrix Rotate_Y(double theta) {
            Matrix m = Matrix.GetIdentityMatrix();
            m.GetMatrix[0, 0] = Math.Cos(theta);
            m.GetMatrix[0, 2] = Math.Sin(theta);
            m.GetMatrix[2, 0] = -Math.Sin(theta);
            m.GetMatrix[2, 2] = Math.Cos(theta);
            return m;
        }

        public static Matrix Rotate_Z(double theta) {
            Matrix m = Matrix.GetIdentityMatrix();
            m.GetMatrix[0, 0] = Math.Cos(theta);
            m.GetMatrix[0, 1] = -Math.Sin(theta);
            m.GetMatrix[1, 0] = Math.Sin(theta);
            m.GetMatrix[1, 1] = Math.Cos(theta);
            return m;
        }

        public static Matrix Shear(double x_y, double x_z, double y_x, double y_z, double z_x, double z_y) {
            Matrix m = Matrix.GetIdentityMatrix();
            m.GetMatrix[0, 1] = x_y;
            m.GetMatrix[0, 2] = x_z;
            m.GetMatrix[1, 0] = y_x;
            m.GetMatrix[1, 2] = y_z;
            m.GetMatrix[2, 0] = z_x;
            m.GetMatrix[2, 1] = z_y;
            return m;
        }

        public static Matrix ViewTransform(Tuple from, Tuple to, Tuple up) {
            Tuple forward = Tuple.Normalize(to - from);
            Tuple upn = Tuple.Normalize(up);
            Tuple left = Tuple.Cross(forward, upn);
            Tuple trueUp = Tuple.Cross(left, forward);

            Matrix orientation = new Matrix( 
                left.X,     left.Y,     left.Z,     0,
                trueUp.X,   trueUp.Y,   trueUp.Z,   0,
                -forward.X, -forward.Y, -forward.Z, 0,
                0,          0,          0,          1);

            return orientation * Translate(-from.X, -from.Y, -from.Z);
        }
    }
}
