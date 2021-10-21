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



            //for (int i = line1.Count - 1; i >= 0; i--)
            //{
            //    points_for_triangulation.Add(new Vector2(line1[i].X, line1[i].Y));
            //}


            List<PointF> line2 = new List<PointF>();
            List<PointF> temp_line2 = new List<PointF>();
            int round_trck = 0;
            for (int i = 0; i < line1.Count - 1; i++)
            {
                round_trck++;
                double x1 = line1[i].X, x2 = line1[i + 1].X, y1 = line1[i].Y, y2 = line1[i + 1].Y; // The original line
                var L = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

                var offsetPixels = 40.0;

                double x1p = x1 + offsetPixels * (y2 - y1) / L;
                double x2p = x2 + offsetPixels * (y2 - y1) / L;
                double y1p = y1 + offsetPixels * (x1 - x2) / L;
                double y2p = y2 + offsetPixels * (x1 - x2) / L;



                Console.WriteLine($"i: {i} ==>> x1 = {x1}, y1 = {y1}, x2 = {x2}, y2 = {y2} --->>>>> px1 = {x1p}, py1 = {y1p}, px2 = {x2p}, py2 = {y2p}");
                
                temp_line2.Add(new PointF((float)x1p, (float)y1p));
                temp_line2.Add(new PointF((float)x2p, (float)y2p));


                line2.Add(new PointF((float)x1p, (float)y1p));
                line2.Add(new PointF((float)x2p, (float)y2p));

                if (round_trck == 2)
                {
                    var intesect_point = isIntersect(temp_line2);

                    if (intesect_point.x != -1 && intesect_point.y != -1 && intesect_point.x != 0 && intesect_point.y != 0)
                    {
                        line2.Add(new PointF((float)intesect_point.x, (float)intesect_point.y));

                        line2.Remove(temp_line2[1]);
                        line2.Remove(temp_line2[2]);
                        var tmp_p = temp_line2[3];
                        line2.Remove(temp_line2[3]);

                        line2.Add(tmp_p);

                    }

                    temp_line2.Clear();
                    round_trck = 0;
                }

            }


            //for (int i = 0; i < line2.Count - 1; i++)
            //{
            //    points_for_triangulation.Add(new Vector2(line2[i].X, line2[i].Y));
            //}


            #region Road Object

            //var new_vertecs = Triangulator.EnsureWindingOrder(points_for_triangulation.Distinct().ToArray(), WindingOrder.Clockwise);
            //Triangulator.Triangulate(new_vertecs, WindingOrder.Clockwise, out Vector2[] result, out int[] indices);

            //List<Vector2> resultVertices = new List<Vector2>(result);
            //List<int> ResultIndices = new List<int>(indices);

            ////vertex
            //StringBuilder vertex_Builder = new StringBuilder();
            //foreach (var point in resultVertices)
            //{
            //    vertex_Builder.Append($"v {point.X} {0} {point.Y} \n");
            //}

            ////face
            //StringBuilder face_Builder = new StringBuilder();
            //for (int i = 0; i < ResultIndices.Count; i += 3)
            //{
            //    face_Builder.Append($"f {ResultIndices[i] + 1} {ResultIndices[i + 1] + 1} {ResultIndices[i + 2] + 1} \n");
            //}

            //File.WriteAllText(@"D:\3DTerrainData\RoadData\Road.obj", vertex_Builder.ToString() + face_Builder.ToString());

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

    public struct Line
    {
        public double x1 { get; set; }
        public double y1 { get; set; }

        public double x2 { get; set; }
        public double y2 { get; set; }
    }

    public struct Point
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public static class LineIntersection
    {
        //  Returns Point of intersection if do intersect otherwise default Point (null)
        public static Point FindIntersection(Line lineA, Line lineB, double tolerance = 0.001)
        {
            try
            {
                double x1 = lineA.x1, y1 = lineA.y1;
                double x2 = lineA.x2, y2 = lineA.y2;

                double x3 = lineB.x1, y3 = lineB.y1;
                double x4 = lineB.x2, y4 = lineB.y2;

                // equations of the form x = c (two vertical lines)
                if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance && Math.Abs(x1 - x3) < tolerance)
                {
                    throw new Exception("Both lines overlap vertically, ambiguous intersection points.");
                }

                //equations of the form y=c (two horizontal lines)
                if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance && Math.Abs(y1 - y3) < tolerance)
                {
                    throw new Exception("Both lines overlap horizontally, ambiguous intersection points.");
                }

                //equations of the form x=c (two vertical parallel lines)
                if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance)
                {
                    //return default (no intersection)
                    return default(Point);
                }

                //equations of the form y=c (two horizontal parallel lines)
                if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance)
                {
                    //return default (no intersection)
                    return default(Point);
                }

                //general equation of line is y = mx + c where m is the slope
                //assume equation of line 1 as y1 = m1x1 + c1 
                //=> -m1x1 + y1 = c1 ----(1)
                //assume equation of line 2 as y2 = m2x2 + c2
                //=> -m2x2 + y2 = c2 -----(2)
                //if line 1 and 2 intersect then x1=x2=x & y1=y2=y where (x,y) is the intersection point
                //so we will get below two equations 
                //-m1x + y = c1 --------(3)
                //-m2x + y = c2 --------(4)

                double x, y;

                //lineA is vertical x1 = x2
                //slope will be infinity
                //so lets derive another solution
                if (Math.Abs(x1 - x2) < tolerance)
                {
                    //compute slope of line 2 (m2) and c2
                    double m2 = (y4 - y3) / (x4 - x3);
                    double c2 = -m2 * x3 + y3;

                    //equation of vertical line is x = c
                    //if line 1 and 2 intersect then x1=c1=x
                    //subsitute x=x1 in (4) => -m2x1 + y = c2
                    // => y = c2 + m2x1 
                    x = x1;
                    y = c2 + m2 * x1;
                }
                //lineB is vertical x3 = x4
                //slope will be infinity
                //so lets derive another solution
                else if (Math.Abs(x3 - x4) < tolerance)
                {
                    //compute slope of line 1 (m1) and c2
                    double m1 = (y2 - y1) / (x2 - x1);
                    double c1 = -m1 * x1 + y1;

                    //equation of vertical line is x = c
                    //if line 1 and 2 intersect then x3=c3=x
                    //subsitute x=x3 in (3) => -m1x3 + y = c1
                    // => y = c1 + m1x3 
                    x = x3;
                    y = c1 + m1 * x3;
                }
                //lineA & lineB are not vertical 
                //(could be horizontal we can handle it with slope = 0)
                else
                {
                    //compute slope of line 1 (m1) and c2
                    double m1 = (y2 - y1) / (x2 - x1);
                    double c1 = -m1 * x1 + y1;

                    //compute slope of line 2 (m2) and c2
                    double m2 = (y4 - y3) / (x4 - x3);
                    double c2 = -m2 * x3 + y3;

                    //solving equations (3) & (4) => x = (c1-c2)/(m2-m1)
                    //plugging x value in equation (4) => y = c2 + m2 * x
                    x = (c1 - c2) / (m2 - m1);
                    y = c2 + m2 * x;

                    //verify by plugging intersection point (x, y)
                    //in orginal equations (1) & (2) to see if they intersect
                    //otherwise x,y values will not be finite and will fail this check
                    if (!(Math.Abs(-m1 * x + y - c1) < tolerance
                        && Math.Abs(-m2 * x + y - c2) < tolerance))
                    {
                        //return default (no intersection)
                        return default(Point);
                    }
                }

                //x,y can intersect outside the line segment since line is infinitely long
                //so finally check if x, y is within both the line segments
                if (IsInsideLine(lineA, x, y) &&
                    IsInsideLine(lineB, x, y))
                {
                    return new Point { x = x, y = y };
                }

                //return default (no intersection)
                return default(Point);
            }
            catch (Exception e)
            {
                var fail = new Point();
                fail.x = -1;
                fail.y = -1;
                return fail;
            }


        }

        // Returns true if given point(x,y) is inside the given line segment
        private static bool IsInsideLine(Line line, double x, double y)
        {
            return (x >= line.x1 && x <= line.x2
                        || x >= line.x2 && x <= line.x1)
                   && (y >= line.y1 && y <= line.y2
                        || y >= line.y2 && y <= line.y1);
        }
    }
}
