using SphereShading.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SphereShading.Models
{
    class Sphere
    {
        private Vertex[] vertices;
        private Triangle[] mesh;
        private int radius, m, n;

        public Vertex[] getVertices() {
            return vertices;
        }
        public Sphere(int radius, int m, int n)
        {
            this.radius = radius;
            this.m = m;
            this.n = n;
            this.vertices = new Vertex[m * n + 2];
            //generate points
            vertices[0] = new Vertex((new Vector(0, radius, 0, 1)), new Vector(0,0,0,0), new Vector(0,0,0,0));
            for (int i = 0; i < n; ++i) {
                for (int j = 0; j < m; ++j) {
                    vertices[i*m + j + 1] = new Vertex (new Vector(
                        radius * Math.Cos((2 * Math.PI * j) / m)*Math.Sin((i+1)*Math.PI/(n + 1)),
                        radius*Math.Cos((i+1)* Math.PI / (n + 1)),
                        radius*Math.Sin(2* Math.PI * j/m)*Math.Sin(Math.PI * (i+1)/(n+1)),
                        1
                        ), new Vector(0,0,0,0), new Vector(0,0,0,0));
                }
            }
            vertices[m * n  + 1] = new Vertex(new Vector(0, -radius, 0, 1), new Vector(0,0,0,0), new Vector(0,0,0,0));
        }

        public Triangle[] generateMesh(Vertex[] points) {
            this.mesh = new Triangle[2 * m * n];
            for (int i = 0; i < m - 1; ++i)
            {
                mesh[i] = new Triangle(ref points[0], ref points[i + 2], ref points[i + 1]);
                mesh[(2 * n - 1) * m + i] = new Triangle(ref points[m * n + 1], ref points[(n - 1) * m + i + 1], ref points[(n - 1) * m + i + 2]);

            }
            mesh[m - 1] = new Triangle(ref points[0], ref points[1], ref points[m]);
            mesh[(2 * n - 1) * m + m - 1] = new Triangle(ref points[m * n + 1], ref points[m * n], ref points[(n - 1) * m + 1]);

            for (int i = 0; i < n - 1; ++i)
            {
                for (int j = 1; j < m; ++j)
                {
                    mesh[(2 * i + 1) * m + j - 1] = new Triangle(ref points[i * m + j], ref points[i * m + j + 1], ref points[(i + 1) * m + j + 1]);
                    mesh[(2 * i + 2) * m + j - 1] = new Triangle(ref points[i * m + j], ref points[(i + 1) * m + j + 1], ref points[(i + 1) * m + j]);
                }
                mesh[(2 * i + 1) * m + m - 1] = new Triangle(ref points[(i + 1) * m], ref points[i * m + 1], ref points[(i + 1) * m + 1]);
                mesh[(2 * i + 2) * m + m - 1] = new Triangle(ref points[(i + 1) * m], ref points[(i + 1) * m + 1], ref points[(i + 2) * m]);
            }

            return mesh;
        }
    }
}
