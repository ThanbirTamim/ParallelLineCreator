using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParallelLineCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //List<Vector2> points_for_triangulation = new List<Vector2>();

            List<PointF> line1 = new List<PointF>
            {
                new PointF(50,50),
                new PointF(500,50),
                new PointF(500, 500),
                new PointF(50, 500),
                new PointF(50, 250),
                new PointF(250, 250),
                new PointF(250, 100)
            };

            #region Way 1 (Most Accurate)
            List<double> oldX = new List<double>();
            List<double> oldY = new List<double>();
            for (int i = 0; i < line1.Count; i++)
            {
                oldX.Add(line1[i].X);
                oldY.Add(line1[i].Y);
            }
            double offset = 15;
            ParallelPointClass.getEnlarged(oldX, oldY, offset);
            var newX = ParallelPointClass.newX;
            var newY = ParallelPointClass.newY;
            List<PointF> line2 = new List<PointF>();
            for (int i = 0; i < newX.Count; i++)
            {
                line2.Add(new PointF((float)newX[i], (float)newY[i]));
            }
            #endregion

            #region Way 2 (100% is not Accurate)
            //List<PointF> line2 = new List<PointF>();
            //List<PointF> temp_line2 = new List<PointF>();
            //int round_trck = 0;
            //for (int i = 0; i < line1.Count - 1; i++)
            //{
            //    round_trck++;

            //    #region Technique1
            //    double x1 = line1[i].X, x2 = line1[i + 1].X, y1 = line1[i].Y, y2 = line1[i + 1].Y; // The original line
            //    var L = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            //    var offsetPixels = 40.0;

            //    double x1p = x1 + offsetPixels * (y2 - y1) / L;
            //    double x2p = x2 + offsetPixels * (y2 - y1) / L;
            //    double y1p = y1 + offsetPixels * (x1 - x2) / L;
            //    double y2p = y2 + offsetPixels * (x1 - x2) / L;
            //    #endregion

            //    #region Technique2
            //    //var distance = 10;
            //    //double x1 = line1[i].X, x2 = line1[i + 1].X, y1 = line1[i].Y, y2 = line1[i + 1].Y; // The original line
            //    //var dx = x2 - x1;
            //    //var dy = y2 - y1;
            //    //var len = Math.Sqrt(dx * dx + dy * dy);
            //    //var perpx = dy * distance / len;
            //    //var perpy = dx * distance / len;
            //    #endregion


            //    Console.WriteLine($"i: {i} ==>> x1 = {x1}, y1 = {y1}, x2 = {x2}, y2 = {y2} --->>>>> px1 = {x1p}, py1 = {y1p}, px2 = {x2p}, py2 = {y2p}");

            //    temp_line2.Add(new PointF((float)x1p, (float)y1p));
            //    temp_line2.Add(new PointF((float)x2p, (float)y2p));


            //    line2.Add(new PointF((float)x1p, (float)y1p));
            //    line2.Add(new PointF((float)x2p, (float)y2p));

            //    if (round_trck == 2)
            //    {
            //        var intesect_point = isIntersect(temp_line2);

            //        if (intesect_point.x != -1 && intesect_point.y != -1 && intesect_point.x != 0 && intesect_point.y != 0)
            //        {
            //            line2.Add(new PointF((float)intesect_point.x, (float)intesect_point.y));

            //            line2.Remove(temp_line2[1]);
            //            line2.Remove(temp_line2[2]);
            //            var tmp_p = temp_line2[3];
            //            line2.Remove(temp_line2[3]);

            //            line2.Add(tmp_p);

            //        }

            //        temp_line2.Clear();
            //        round_trck = 0;
            //    }

            //}
            #endregion

            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 3);
            e.Graphics.DrawLines(pen, line1.ToArray());

            Pen pen1 = new Pen(Color.FromArgb(255, 255, 0, 0), 3);
            e.Graphics.DrawLines(pen1, line2.ToArray());
        }




        public Point isIntersect(List<PointF> pointFs)
        {
            Line line_intersect1 = new Line();
            line_intersect1.x1 = pointFs[0].X;
            line_intersect1.y1 = pointFs[0].Y;
            line_intersect1.x2 = pointFs[1].X;
            line_intersect1.y2 = pointFs[1].Y;

            Line line_intersect2 = new Line();
            line_intersect2.x1 = pointFs[2].X;
            line_intersect2.y1 = pointFs[2].Y;
            line_intersect2.x2 = pointFs[3].X;
            line_intersect2.y2 = pointFs[3].Y;

            return LineIntersection.FindIntersection(line_intersect1, line_intersect2);
        }
    }
}
