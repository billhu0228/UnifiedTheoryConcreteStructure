using AutoCAD;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnifiedTheoryConcreteStructure.Public
{
    public static class Extension
    {



        public static List<double> VecAdd (this List<double> A,double B)
        {
            List<double> C = new List<double>();
            for (int i = 0; i < A.Count; i++)
            {
                C.Add(A[i] + B);
            }
            return C;
        }


        public static List<double> Dot(this List<double> A, List<double> B)
        {
            List<double> C = new List<double>();
            for (int i = 0; i < B.Count; i++)
            {
                C.Add(A[i] * B[i]);
            }
            return C;
        }







        public static Circle2D Circle2D(this AcadCircle acadCircle)
        {
            double[] center = acadCircle.Center;
            Point2D cc = new Point2D(center[0],center[1]);
            double rr = acadCircle.Radius;
            return new Circle2D(cc, rr);
        }
        public static Polygon2D Polygen2D(this AcadLWPolyline acadLW)
        {
            
            double[] ff = acadLW.Coordinates;
            Point2D[] pts = new Point2D[ff.Length / 2];
            for (int i = 0; i < ff.Length; i++)
            {
                if (i%2==0)
                {
                    pts[i / 2] = new Point2D(ff[i], ff[i + 1]);
                }
            }            
            return new Polygon2D(pts); 
        }



        public static double Xmax(this Polygon2D plg)
        {
            var Xs = from Point2D pt in plg.Vertices select pt.X;
            return Xs.Max();
        }
        public static double Xmin(this Polygon2D plg)
        {
            var Xs = from Point2D pt in plg.Vertices select pt.X;
            return Xs.Min();
        }

        public static double Ymax(this Polygon2D plg)
        {
            var Ys = from Point2D pt in plg.Vertices select pt.Y;
            return Ys.Max();
        }
        public static double Ymin(this Polygon2D plg)
        {
            var Ys = from Point2D pt in plg.Vertices select pt.Y;
            return Ys.Min();
        }



        public static bool IsLayOn(this Point2D pt, Line2D line)
        {
            Vector2D vs = pt - line.StartPoint;
            Vector2D ve = pt - line.EndPoint;
            Vector2D v2 = line.EndPoint - line.StartPoint;

            if (vs.IsParallelTo(ve))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsLayIn(this Point2D pt,Line2D line)
        {
            Vector2D vs = pt - line.StartPoint;
            Vector2D ve = pt - line.EndPoint;
            Vector2D v2 = line.EndPoint - line.StartPoint;

           

            if ((pt.DistanceTo(line.StartPoint)<=1e-10)|| (pt.DistanceTo(line.EndPoint) <= 1e-10))
            {
                return false;
            }
            //else if (vs.IsParallelTo(ve,Angle.FromDegrees(1e-20)))
            else if (Math.Abs(vs.AngleTo(ve).Radians - Math.PI) <= 1e-10 || Math.Abs(vs.AngleTo(ve).Radians - 0) <= 1e-10)
            {
               if ((v2.Length > vs.Length)&& (v2.Length > ve.Length) && (vs.Length*ve.Length!=0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Point2D Point2D(this AcadPoint point)
        {
            var cc = point.Coordinates as double[];
            return new Point2D(cc[0], cc[1]);

        }

        public static Line2D Line2D(this LineSegment2D line)
        {
            return new Line2D(line.StartPoint, line.EndPoint);
        }
        public static LineSegment2D LineSegment2D(this Line2D line)
        {
            return new LineSegment2D(line.StartPoint, line.EndPoint);
        }

        

        public static void IntersectWithIn(this Line2D line, Line2D target, out Point2D? pt)
        {
            var ff = line.IntersectWith(target,Angle.FromDegrees(1e-20));
            if (ff.HasValue)
            {
                Point2D aa = (Point2D)ff;
                if (aa.IsLayIn(line)&&(aa.IsLayIn(target)))
                {
                    pt = aa;
                }
                else
                {
                    pt = null;
                }
                
            }
            else
            {
                pt = null;
            }
        }


        public static List<Point2D> GetCrossPoints(this Polygon2D plg, double value,bool isVertical=false)
        {
            List<Point2D> ret = new List<Point2D>();
            Point2D? cr;
            Line2D target;                       
            
            if (isVertical)
            {
                target = new Line2D(new Point2D(value, plg.Ymin()-100), new Point2D(value, plg.Ymax()+100));
            }
            else
            {
                target = new Line2D(new Point2D(plg.Xmin()-100,value), new Point2D(plg.Xmax()+100, value));
            }
            
            foreach (var item in plg.Edges)
            {
                item.Line2D().IntersectWithIn(target,out cr);
                if (cr.HasValue)
                {
                    ret.Add((Point2D)cr);
                }
            }

            foreach (Point2D item in plg.Vertices)
            {
                if (item.IsLayIn(target))
                {
                    ret.Add(item);                    
                }
            }
            if (ret.Count % 2 != 0)
            {
                ;
            }
            

            if (isVertical)
            {
                ret.Sort((pt1, pt2) => pt1.Y.CompareTo(pt2.Y));
            }
            else
            {
                ret.Sort((pt1, pt2) => pt1.X.CompareTo(pt2.X));
            }

            ret=ret.Distinct(new Point2DCompare()).ToList();
            return ret;
        }


        public static double Product(this List<double>A,ref List<double> B)
        {
            double ret = 0;
            Debug.Assert((A.Count == B.Count));
            for (int i = 0; i < A.Count; i++)
            {
                ret += A[i] * B[i];
            }
            return ret;
        }


    }

    public class Point2DCompare : IEqualityComparer<Point2D>
    {
        public bool Equals(Point2D x, Point2D y)
        {
            return x.DistanceTo(y) < 1e-10;
        }
        public int GetHashCode(Point2D obj)
        {
            return 0;
        }
    }
}
