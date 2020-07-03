using System;
using System.Collections.Generic;

namespace SphereShading.Geometry
{
    class Triangle
    {
        private Vertex p1;
        private Vertex p2;
        private Vertex p3;
        private int y_max;
        private int y_min;

        public Triangle(ref Vertex p1, ref Vertex p2, ref Vertex p3)
        {
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;
            Y_max = (int)Math.Max(Math.Max(p1.P[1], p2.P[1]), p3.P[1]);
            Y_min = (int)Math.Min(Math.Min(p1.P[1], p2.P[1]), p3.P[1]);
        }

        public List<Edge>[] GetEdgeTable()
        {
            Y_max = (int)Math.Max(Math.Max(p1.P[1], p2.P[1]), p3.P[1]);
            Y_min = (int)Math.Min(Math.Min(p1.P[1], p2.P[1]), p3.P[1]);
            var y_range = Y_max - Y_min;
            var result = new List<Edge>[(int)y_range + 1];
            for (int i = 0; i < (int)y_range + 1; ++i)
                result[i] = new List<Edge>();
            result[(int)Y_max - (int)Math.Max(p1.P[1], p2.P[1])].Add(new Edge(ref p1, ref p2));
            result[(int)Y_max - (int)Math.Max(p2.P[1], p3.P[1])].Add(new Edge(ref p2, ref p3));
            result[(int)Y_max - (int)Math.Max(p3.P[1], p1.P[1])].Add(new Edge(ref p3, ref p1));
            return result;
        }

        internal Vertex P1 { get => p1; set => p1 = value; }
        internal Vertex P2 { get => p2; set => p2 = value; }
        internal Vertex P3 { get => p3; set => p3 = value; }
        public int Y_max { get => y_max; set => y_max = value; }
        public int Y_min { get => y_min; set => y_min = value; }
    }
}
