namespace SphereShading.Geometry
{
    class Vertex
    {
        private Vector pg;
        private Vector ng;
        private Vector p;
        private Vector phong;

        public Vertex(Vector pg, Vector ng, Vector p)
        {
            this.Pg = pg;
            this.P = p;
        }
        public Vertex(Vertex v)
        {
            this.Pg = new Vector(v.Pg);
            this.P = new Vector(v.P);
        }

        internal Vector Pg { get => pg; set => pg = value; }
        internal Vector Ng { get => new Vector(Pg[0] / 10, Pg[1] / 10, Pg[2] / 10, 0); }
        internal Vector P { get => p; set => p = value; }
        internal Vector Phong { get => phong; set => phong = value; }

        public static Vertex operator +(Vertex v1, Vertex v2)
        {
            return new Vertex(v1.Pg + v2.Pg, v1.Ng + v2.Ng, v1.P + v2.P);
        }

        public static Vertex operator -(Vertex v1, Vertex v2)
        {
            return new Vertex(v1.Pg - v2.Pg, v1.Ng - v2.Ng, v1.P - v2.P);
        }
        public static Vertex operator *(Vertex v1, double t)
        {
            return new Vertex(v1.Pg.scalarProduct(t), v1.Ng.scalarProduct(t), v1.P.scalarProduct(t));
        }
    }
}
