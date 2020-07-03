using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SphereShading.Geometry
{
    class Matrix
    {
        private Vector[] vectors;

        public Matrix(Vector v1 = null, Vector v2 = null, Vector v3 = null, Vector v4 = null)
        {
            if (v1 == v2 && v2 == v3 && v3 == v4 && v4 == null) {
                vectors = new Vector[4] { new Vector(0,0,0,0), new Vector(0, 0, 0, 0), new Vector(0, 0, 0, 0), new Vector(0, 0, 0, 0) };
            }
            else
                vectors = new Vector[4] { v1, v2, v3, v4 };
        }

        public Vector this[int key]
        {
            get => vectors[key];
            set => vectors[key] = value;
        }

        public Vector multiply(Vector vector)
        {
            Vector result = new Vector(0,0,0,0);

            for (int i = 0; i < 4; i++)
            {
                var temp = new Vector(vectors[0][i],
                                vectors[1][i],
                                vectors[2][i],
                                 vectors[3][i]);

                result[i] = temp.dotProduct(vector);
            }

            return result;
        }

        public Matrix multiply(Matrix m)
        {
            var result = new Matrix();

            for (int i = 0; i < 4; i++)
            {
                var temp = new Vector(vectors[0][i],
                        vectors[1][i],
                        vectors[2][i],
                        vectors[3][i]);

                for (int j = 0; j < 4; j++)
                {
                    result.vectors[j][i] = temp.dotProduct(m.vectors[j]);
                }
            }
            return result;
        }
    }
}
