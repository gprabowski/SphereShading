using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SphereShading.Geometry
{
    class Vector
    {
        private double[] vals;

        public Vector(double x, double y, double z, double a)
        {
            vals = new double[4] { x, y, z, a };
        }

        public Vector(Vector v)
        {
            vals = new double[4] { v[0], v[1], v[2], v[3] };
        }

        public double this[int key] {
            get => vals[key];
            set => vals[key] = value;
        }
        public double dotProduct(Vector v)
        {
            double result = 0;

            for (int i = 0; i < 4; i++)
            {
                result += vals[i] * v[i];
            }
            return result;
        }

        public Vector crossProduct(Vector v)
        {
            double x = vals[1] * v.vals[2] - vals[2] * v.vals[1];
            double y = vals[0] * v.vals[2] - vals[2] * v.vals[0];
            double z = vals[0] * v.vals[1] - vals[1] * v.vals[0];

            return new Vector(x, y, z, 1);
        }

        public Vector scalarProduct(double n)
        {
            var result = new Vector(0,0,0,0);

            for (int i = 0; i < 4; i++)
            {
                result[i] = vals[i] * n;
            }
            return result;
        }

        public static Vector operator- (Vector v1, Vector v2) {
            return new Vector(v1[0] - v2[0], v1[1] - v2[1], v1[2] - v2[2], v1[3] - v2[3]);
        }
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1[0] + v2[0], v1[1] + v2[1], v1[2] + v2[2], v1[3] + v2[3]);
        }

        public double length()
        {
            return Math.Sqrt(Math.Pow(vals[0], 2) + Math.Pow(vals[1], 2) + Math.Pow(vals[2], 2));
        }
    }
}
