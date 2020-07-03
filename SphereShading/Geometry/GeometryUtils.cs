using SphereShading.Geometry;
using System;

namespace SphereShading
{
    class GeometryUtils
    {
        public static Matrix getXRotationMatrix(double alpha)
        {
            return new Matrix(
                new Vector(1, 0, 0, 0),
                new Vector(0, Math.Cos(alpha), -1 * Math.Sin(alpha), 0),
                new Vector(0, Math.Sin(alpha), Math.Cos(alpha), 0),
                new Vector(0, 0, 0, 1)
                );
        }

        public static Matrix getYRotationMatrix(double alpha)
        {
            return new Matrix(
                new Vector(Math.Cos(alpha), 0, -1 * Math.Sin(alpha), 0),
                new Vector(0, 1, 0, 0),
                new Vector(Math.Sin(alpha), 0, Math.Cos(alpha), 0),
                new Vector(0, 0, 0, 1)
                );
        }

        public static Matrix getZRotationZMatrix(double alpha)
        {
            return new Matrix(
                new Vector(Math.Cos(alpha), Math.Sin(alpha), 0, 0),
                new Vector(-1 * Math.Sin(alpha), Math.Cos(alpha), 0, 0),
                new Vector(0, 0, 1, 0),
                new Vector(0, 0, 0, 1));
        }

        public static Matrix getTranslationMatrix(Vector translate)
        {
            var m = getIdentityMatrix();
            m[3] = translate;
            return m;
        }

        public static Matrix getIdentityMatrix()
        {
            return new Matrix(
                new Vector(1, 0, 0, 0),
                new Vector(0, 1, 0, 0),
                new Vector(0, 0, 1, 0),
                new Vector(0, 0, 0, 1));
        }

        public static Matrix getScaleMatrix(Vector scale)
        {
            return new Matrix(
                new Vector(scale[0], 0, 0, 0),
                new Vector(0, scale[1], 0, 0),
                new Vector(0, 0, scale[2], 0),
                new Vector(0, 0, 0, 1));
        }

        public static Matrix getProjectionMatrix(double w, double h)
        {
            var d = (w / 2.0) * 1.0 / Math.Tan(Math.PI / 4.0);
            return new Matrix(
                new Vector(d, 0, 0, 0),
                new Vector(0, -d, 0, 0),
                new Vector(w / 2.0, h / 2.0, 0, 1),
                new Vector(0, 0, 1, 0));
        }
    }
}
