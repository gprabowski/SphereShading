using SphereShading.Geometry;
using SphereShading.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Vector = SphereShading.Geometry.Vector;

namespace SphereShading
{
    public partial class MainWindow : Window
    {
        private double lastX;
        private double lastY;
        private readonly int height = 800;
        private readonly int width = 1000;
        private readonly Sphere sphere;
        private Geometry.Vector light = new Geometry.Vector(0, 0, 0, 0);
        private Geometry.Matrix translation;
        private readonly Vertex[] projectedShape;
        private readonly double lightI;
        private readonly Geometry.Vector lightKa;
        private readonly Geometry.Vector lightKd;
        private readonly Geometry.Vector lightKs;
        private Geometry.Vector lightCam;
        private readonly WriteableBitmap bitmap;
        private bool fixedLight = true;
        private readonly int lightM;
        byte[] pixels;
        public MainWindow()
        {
            InitializeComponent();
            bitmap = new WriteableBitmap(
                1000, 800, 96, 96, PixelFormats.Bgra32, null);
            lightI = 120;
            lightKa = new Geometry.Vector(0.2, 0.2, 0.2, 0);
            lightKd = new Geometry.Vector(0.4, 0.6, 0.5, 0);
            lightKs = new Geometry.Vector(0.4, 0.6, 0.5, 0);
            lightCam = new Geometry.Vector(0, 0, mainSlider.Value, 0);
            lightM = 2;
            sphere = new Sphere(10, 20, 20);
            projectedShape = new Geometry.Vertex[sphere.getVertices().Length];
            sphere.getVertices().CopyTo(projectedShape, 0);
            var t = new Geometry.Vector(0, 0, mainSlider.Value, 1);
            translation = GeometryUtils.getTranslationMatrix(t);
            displayScene(0, 0);
        }

        private void displayScene(double v1, double v2)
        {
            pixels = new byte[height * width * 4];
            for (int i = 0; i < projectedShape.Length; ++i)
            {
                sphere.getVertices()[i].P = GeometryUtils.getProjectionMatrix(1000, 800)
                .multiply(translation)
                .multiply(GeometryUtils.getXRotationMatrix(v1))
                .multiply(GeometryUtils.getYRotationMatrix(v2))
                .multiply(sphere.getVertices()[i].Pg)
                .scalarProduct(1.0 / (sphere.getVertices()[i].Pg[2] + mainSlider.Value));

                if (v1 == 0 && v2 == 0)
                    continue;
                sphere.getVertices()[i].Pg = GeometryUtils.getXRotationMatrix(v1)
                    .multiply(GeometryUtils.getYRotationMatrix(v2))
                    .multiply(sphere.getVertices()[i].Pg);
            }
            if (v1 != 0 || v2 != 0)
            {
                if (fixedLight)
                    light = GeometryUtils.getXRotationMatrix(v1)
                    .multiply(GeometryUtils.getYRotationMatrix(v2))
                     .multiply(light);
            }


            var mesh = sphere.generateMesh(sphere.getVertices());
            foreach (Triangle tri in mesh)
            {
                var temp = new Geometry.Vector(tri.P2.P[0] - tri.P1.P[0], tri.P2.P[1] - tri.P1.P[1], 0, 0).crossProduct(new Geometry.Vector(tri.P3.P[0] - tri.P1.P[0], tri.P3.P[1] - tri.P1.P[1], 0, 0));
                if (temp[2] > 0)
                {
                    drawTriangle(tri);
                }
            }

            Int32Rect rect = new Int32Rect(0, 0, width, height);
            int stride = 4 * width;
            bitmap.WritePixels(rect, pixels, stride, 0);
            mainImage.Source = bitmap;
        }
        private void mainSlider_dragCompleted(object sender, DragCompletedEventArgs e)
        {
            var t = new Geometry.Vector(0, 0, mainSlider.Value, 1);
            translation = GeometryUtils.getTranslationMatrix(t);
            sphere.getVertices().CopyTo(projectedShape, 0);
            lightCam[2] = -mainSlider.Value;
            displayScene(0, 0);
        }

        private void calculatePhong(Triangle tri)
        {
            tri.P1.Phong = PhongValue(tri.P1);
            tri.P2.Phong = PhongValue(tri.P2);
            tri.P3.Phong = PhongValue(tri.P3);

        }

        private void drawTriangle(Triangle tri)
        {
            //bgra
            calculatePhong(tri);
            var ET = tri.GetEdgeTable();
            var AET = new List<Edge>();
            for (int i = 0; i < tri.Y_max - tri.Y_min + 1; ++i)
            {
                AET.AddRange(ET[i]);
                AET.RemoveAll(s => s.Ymin == tri.Y_max - i || s.Ymax == s.Ymin);
                if (AET.Count == 0)
                    continue;
                AET.Sort();
                //light magic
                Geometry.Vector color;
                for (int j = (int)AET[0].Xmin; j <= (int)AET[1].Xmin + 1; ++j)
                {
                    var res = cartesian2barycentric(j, tri.Y_max - i, tri.P1.P, tri.P2.P, tri.P3.P);
                    color = tri.P1.Phong.scalarProduct(res[0]) + tri.P2.Phong.scalarProduct(res[1]) + tri.P3.Phong.scalarProduct(res[2]);
                    setPixel(tri.Y_max - i, j, 0, color);
                }
                foreach (Edge edg in AET)
                    edg.makeStep();
            }
        }

        private void setPixel(int y, int x, int c, Geometry.Vector col)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                pixels[y * width * 4 + x * 4 + 0] = (byte)col[2];
                pixels[y * width * 4 + x * 4 + 1] = (byte)col[1];
                pixels[y * width * 4 + x * 4 + 2] = (byte)col[0];
                pixels[y * width * 4 + x * 4 + 3] = 255;
            }
        }

        private Vector cartesian2barycentric(double x, double y, Geometry.Vector p1, Geometry.Vector p2, Geometry.Vector p3)
        {
            //TODO do not calculate everything every time, huge waste
            double y2y3 = p2[1] - p3[1],
                x3x2 = p3[0] - p2[0],
                x1x3 = p1[0] - p3[0],
                y1y3 = p1[1] - p3[1],
                y3y1 = p3[1] - p1[1],
                xx3 = x - p3[0],
                yy3 = y - p3[1];

            double d = y2y3 * x1x3 + x3x2 * y1y3,
                  lambda1 = (y2y3 * xx3 + x3x2 * yy3) / d,
                  lambda2 = (y3y1 * xx3 + x1x3 * yy3) / d;

            return new Geometry.Vector(
              lambda1,
              lambda2,
              1 - lambda1 - lambda2, 0);
        }
        private Geometry.Vector PhongValue(Vertex P)
        {
            //light - point light position
            //P.Pg - global coords of point
            //
            var lcoeff = (light - P.Pg).scalarProduct(1 / (light - P.Pg).length());
            var first = lightKa.scalarProduct(150);
            var second = lightKd.scalarProduct(lightI).scalarProduct(Math.Max(P.Ng.dotProduct(lcoeff), 0));
            var v = (lightCam - P.Pg).scalarProduct(1 / (lightCam - P.Pg).length());
            var r = P.Ng.scalarProduct(P.Ng.dotProduct(lcoeff)) - lcoeff;
            var third = lightKs.scalarProduct(lightI).scalarProduct(Math.Pow(Math.Max(v.dotProduct(r), 0), lightM));
            if ((P.Pg - light).dotProduct(P.Ng) >= 0)
                third = new Geometry.Vector(0, 0, 0, 0);
            return first + second + third;
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                light[1] += 100;
                displayScene(0, 0);
            }
            else if (e.Key == Key.S)
            {
                light[1] -= 100;
                displayScene(0, 0);
            }
            else if (e.Key == Key.A)
            {
                light[0] -= 100;
                displayScene(0, 0);
            }
            else if (e.Key == Key.D)
            {
                light[0] += 100;
                displayScene(0, 0);
            }
            else if (e.Key == Key.F)
            {
                light[2] -= 100;
                displayScene(0, 0);
            }
            else if (e.Key == Key.G)
            {
                light[2] += 100;
                displayScene(0, 0);
            }
            else if (e.Key == Key.R)
            {
                light = new Geometry.Vector(0, 100, 0, 0);
                displayScene(0, 0);
            }
            else if (e.Key == Key.M)
            {
                fixedLight = !fixedLight;
            }

        }
        private void mainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var dx = lastX - e.GetPosition(this).X;
                var dy = lastY - e.GetPosition(this).Y;
                displayScene(-dy * Math.PI / 800.0, dx * Math.PI / 800.0);
                lastX = e.GetPosition(this).X;
                lastY = e.GetPosition(this).Y;
            }
        }

        private void mainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastX = e.GetPosition(this).X;
            lastY = e.GetPosition(this).Y;
        }
    }
}
