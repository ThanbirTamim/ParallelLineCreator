using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLineCreator
{
    public static class ParallelPointClass
    {
        public static List<double> newX = new List<double>();

        public static List<double> newY = new List<double>();

        public static Tuple<double, double> findIntesection(double p1x, double p1y, double p2x, double p2y, double p3x, double p3y, double p4x, double p4y)
        {
            var dx12 = p2x - p1x;
            var dy12 = p2y - p1y;
            var dx34 = p4x - p3x;
            var dy34 = p4y - p3y;
            var denominator = dy12 * dx34 - dx12 * dy34;
            var t1 = ((p1x - p3x) * dy34 + (p3y - p1y) * dx34) / denominator;
            var t2 = ((p3x - p1x) * dy12 + (p1y - p3y) * dx12) / -denominator;
            var intersectX = p1x + dx12 * t1;
            var intersectY = p1y + dy12 * t1;
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }
            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }
            return new Tuple<double, double>(intersectX, intersectY);
        }

        public static Tuple<double, double> normalizeVec(double x, double y)
        {
            var distance = Math.Sqrt(x * x + y * y);
            return new Tuple<double, double>(x / distance, y / distance);
        }

        public static void getEnlarged(List<double> oldX, List<double> oldY, double offset)
        {
            var num_points = oldX.Count;
            foreach (var j in Enumerable.Range(0, num_points))
            {
                var i = j - 1;
                if (i < 0)
                {
                    i += num_points;
                }
                var k = (j + 1) % num_points;
                var vec1X = oldX[j] - oldX[i];
                var vec1Y = oldY[j] - oldY[i];
                var _tup_1 = normalizeVec(vec1X, vec1Y);
                var v1normX = _tup_1.Item1;
                var v1normY = _tup_1.Item2;
                v1normX *= offset;
                v1normY *= offset;
                var n1X = -v1normY;
                var n1Y = v1normX;
                var pij1X = oldX[i] + n1X;
                var pij1Y = oldY[i] + n1Y;
                var pij2X = oldX[j] + n1X;
                var pij2Y = oldY[j] + n1Y;
                var vec2X = oldX[k] - oldX[j];
                var vec2Y = oldY[k] - oldY[j];
                var _tup_2 = normalizeVec(vec2X, vec2Y);
                var v2normX = _tup_2.Item1;
                var v2normY = _tup_2.Item2;
                v2normX *= offset;
                v2normY *= offset;
                var n2X = -v2normY;
                var n2Y = v2normX;
                var pjk1X = oldX[j] + n2X;
                var pjk1Y = oldY[j] + n2Y;
                var pjk2X = oldX[k] + n2X;
                var pjk2Y = oldY[k] + n2Y;
                var _tup_3 = findIntesection(pij1X, pij1Y, pij2X, pij2Y, pjk1X, pjk1Y, pjk2X, pjk2Y);
                var intersectX = _tup_3.Item1;
                var intersetY = _tup_3.Item2;
                //print(intersectX,intersetY)
                newX.Add(intersectX);
                newY.Add(intersetY);
            }
        }
    }
}
