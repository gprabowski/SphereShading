using System;

namespace SphereShading.Geometry
{
    class Edge : IComparable
    {
        private Vertex p0;
        private Vertex p1;
        private Vertex p_temp;
        private double xmin;
        private double m;
        private int ymax;
        private int ymin;
        public Edge(ref Vertex p0, ref Vertex p1)
        {
            if (p0.P[1] > p1.P[1])
            {
                this.P0 = p0;
                this.P1 = p1;
            }
            else
            {
                this.P1 = p0;
                this.P0 = p1;
            }
            Xmin = this.P0.P[0];
            Ymax = (int)Math.Max(p0.P[1], p1.P[1]);
            Ymin = (int)Math.Min(p0.P[1], p1.P[1]);
            M = (P0.P[1] - P1.P[1]) / (P1.P[0] - P0.P[0]);

        }

        public void reset()
        {
            P_temp = new Vertex(P0);
        }

        public double Xmin { get => xmin; set => xmin = value; }
        public double M { get => m; set => m = value; }
        internal Vertex P0 { get => p0; set => p0 = value; }
        internal Vertex P1 { get => p1; set => p1 = value; }
        public int Ymax { get => ymax; set => ymax = value; }
        public int Ymin { get => ymin; set => ymin = value; }
        internal Vertex P_temp { get => p_temp; set => p_temp = value; }

        public static Boolean operator <(Edge a, Edge b)
        {
            return a.Xmin < b.Xmin;
        }

        public static Boolean operator >(Edge a, Edge b)
        {
            return a.Xmin > b.Xmin;
        }
        public void makeStep()
        {
            Xmin += 1 / M;
        }

        public Vertex getMedP(double t)
        {
            return (P1 - P0) * t + P0;
        }

        public int CompareTo(Object edge)
        {
            if (edge == null)
                return 1;
            Edge edg = edge as Edge;
            return xmin.CompareTo(edg.Xmin);
        }
    }
}
