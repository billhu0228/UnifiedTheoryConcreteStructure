using MathNet.Spatial.Euclidean;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedTheoryConcreteStructure.Material;

namespace UnifiedTheoryConcreteStructure.Section
{
    public class OctagonSection
    {
        List<SingleRebar> Rebars;
        List<Point2D> OuterPts;
        List<Line2D> OuterLns;

        public double B, H, Cb, Ch, Cc;
        public int NumRebars;
        public double Ds;
        public Reinforcement RebarProperty;
        public ConcreteAASHTO ConcProperty;
        public Point2D PlasticCenter;

        public OctagonSection(double b, double h, ConcreteAASHTO concProperty, Reinforcement rebarProperty, int numRebars, double ds, double cc=50, double cb=0, double ch=0)
        {
            B = b;
            H = h;
            ConcProperty = concProperty;
            RebarProperty = rebarProperty;
            NumRebars = numRebars;
            Ds = ds;
            Cc = cc;
            Cb = cb;
            Ch = ch;

            GenerateOutline(0.0);
            GenerateRebars();

            PlasticCenter = CalPC();

        }

        private void GenerateRebars()
        {
            Rebars = new List<SingleRebar>();
            double x1 = -0.5 * B + Cc;             
            double y1 = -0.5 * H+Cc;
            double x2;
            double y2;

            if (Cb==0)
            {
                x2 = x1;
                y2 = y1;
            }
            else
            {
                double ang = Math.Atan(Ch / Cb);
                double ang2 = Math.Atan(Cb / Ch);
                x2 = -0.5 * B + Cb + Cc * Math.Tan(ang * 0.5);
                y2 = -0.5 * H + Ch + Cc * Math.Tan(ang2 * 0.5);
            }
            double rCH = y2 - y1;
            double rCB = x2 - x1;
            double ChR = Math.Sqrt(rCH * rCH + rCB * rCB);
            double TotalL = Math.Abs(4 * x2 + 4 * y2) + ChR * 4;
            int CNum = (int)Math.Round(ChR/(TotalL / NumRebars))-1;
            CNum = CNum < 0 ? 0 : CNum;
            int Bnum = (int)Math.Round(2 * Math.Abs(y2) / (TotalL / NumRebars)) - 1;
            Bnum = Bnum < 0 ? 0 : Bnum;
            int Hnum = (NumRebars - 8 - 4 * CNum - 2 * Bnum)/2;

            Point2D Pt1 = new Point2D(-x2, y1);
            Point2D Pt2 = new Point2D(-x1, y2);
            Point2D Pt3 = new Point2D(-x1, -y2);
            Point2D Pt4 = new Point2D(-x2, -y1);
            Point2D Pt5 = new Point2D(x2, -y1);
            Point2D Pt6 = new Point2D(x1, -y2);
            Point2D Pt7 = new Point2D(x1, y2);
            Point2D Pt8 = new Point2D(x2, y1);

            
            
            AddRebars(ref Rebars,Pt8,Pt1,Bnum);
            AddRebars(ref Rebars, Pt5, Pt4, Bnum);            
            AddRebars(ref Rebars, Pt2, Pt3  , Hnum);
            AddRebars(ref Rebars, Pt6,Pt7, Hnum);

            AddRebars(ref Rebars, Pt1, Pt2, CNum);
            AddRebars(ref Rebars, Pt3, Pt4, CNum);
            AddRebars(ref Rebars, Pt5, Pt6, CNum);
            AddRebars(ref Rebars, Pt7, Pt8, CNum);

            Rebars.Add(new SingleRebar(Pt1, Ds, RebarProperty.Fy, RebarProperty.Es));
            Rebars.Add(new SingleRebar(Pt2, Ds, RebarProperty.Fy, RebarProperty.Es));
            Rebars.Add(new SingleRebar(Pt3, Ds, RebarProperty.Fy, RebarProperty.Es));
            Rebars.Add(new SingleRebar(Pt4, Ds, RebarProperty.Fy, RebarProperty.Es));
            Rebars.Add(new SingleRebar(Pt5, Ds, RebarProperty.Fy, RebarProperty.Es));
            Rebars.Add(new SingleRebar(Pt6, Ds, RebarProperty.Fy, RebarProperty.Es));
            Rebars.Add(new SingleRebar(Pt7, Ds, RebarProperty.Fy, RebarProperty.Es));
            Rebars.Add(new SingleRebar(Pt8, Ds, RebarProperty.Fy, RebarProperty.Es));


            Rebars.Sort((x, y) => x.Location.ToVector2D().SignedAngleTo(Vector2D.XAxis).CompareTo(y.Location.ToVector2D().SignedAngleTo(Vector2D.XAxis)));

        }

        public double GetMn(double Nn)
        {
            double c0 = 1;
            double c1 = 5 * H;
            double cmid = (c0 + c1) * 0.5;
            double error = 1e-5;            
            double err1, err0,err2;
            int div = 1000;
            for (int i = 0; i < 50; i++)
            {
                err0 = CalN(c0, div) -Nn;
                err1 = CalN(c1, div) -Nn;
                err2 = CalN(cmid, div) - Nn;
                if (err0 * err1 >= 0)
                {
                    break;
                }
                else if (err0 * err2 >= 0)
                {
                    c0 = cmid;
                    cmid = (c0 + c1) * 0.5;
                }
                else
                {
                    c1 = cmid;
                    cmid = (c0 + c1) * 0.5;
                }

                if (Math.Abs(c0-c1) <= error)
                {
                    break;
                }
            }
            
                                                         
            return CalM(cmid, div);
        }



        private double CalM(double c,int div = 1000)
        {
            double dy = H / (div - 1);
            double phi = -ConcProperty.Epsu / c;
            double oc = H * 0.5 - c;
            double MC = 0;
            double MS = 0.0;

            for (int i = 0; i < div; i++)
            {
                double y0 = -0.5 * H + dy * i;
                double l0 = CutLengY(y0);
                double eps = phi * (y0 - oc);
                double Nconc = ConcProperty.GetSigma(eps) * dy * l0;
                MC += Nconc*(y0-PlasticCenter.Y);
            }

            foreach (SingleRebar item in Rebars)
            {
                double eps = phi * (item.Location.Y - oc);
                double Nsteel = RebarProperty.GetSigma(eps);
                MS += Nsteel*(item.Location.Y-PlasticCenter.Y);
            }
            return MS + MC;
        }

        private double CalN(double c,int div=1000)
        {
            double dy = H / (div - 1);
            double phi = -ConcProperty.Epsu / c;
            double oc = H * 0.5 - c;
            double NC = 0;
            double NS = 0.0;
            for (int i = 0; i < div; i++)
            {
                double y0 = -0.5 * H + dy*i;
                double l0 = CutLengY(y0);
                double eps = phi * (y0 - oc);
                double Nconc = ConcProperty.GetSigma(eps) * dy * l0;
                NC += Nconc;
            }

            foreach (SingleRebar item in Rebars)
            {
                double eps = phi * (item.Location.Y - oc);
                double Nsteel = RebarProperty.GetSigma(eps);
                NS += Nsteel;
            }
            return NS + NC;

        }
        private double CutLengX(double x0)
        {
            if (Math.Abs(x0) <= -OuterPts[7].X)
            {
                return H;
            }
            else
            {
                Point2D st = new Point2D(x0,0);
                Point2D ed = new Point2D(x0,1);
                Point2D inter = new Point2D();

                if (x0 > 0)
                {
                    inter = (Point2D)OuterLns[0].IntersectWith(new Line2D(st, ed));
                }
                else
                {
                    inter = (Point2D)OuterLns[4].IntersectWith(new Line2D(st, ed));
                }

                return Math.Abs(inter.Y * 2);
            }
        }
        private double CutLengY(double y0)
        {
            if (Math.Abs(y0)<=-OuterPts[1].Y)
            {
                return B;
            }
            else
            {
                Point2D st = new Point2D(0, y0);
                Point2D ed = new Point2D(1, y0);
                Point2D inter = new Point2D();

                if (y0<0)
                {
                    inter = (Point2D)OuterLns[0].IntersectWith(new Line2D(st, ed));
                }
                else
                {
                    inter = (Point2D)OuterLns[2].IntersectWith(new Line2D(st, ed));
                }
                
                return Math.Abs(inter.X * 2);
            }
        }

        private void AddRebars(ref List<SingleRebar> rebars, Point2D st, Point2D ed, int v)
        {
            double dl = ed.DistanceTo(st)/(v+1);
            Vector2D dir = st.VectorTo(ed).Normalize();
            for (int i = 0; i < v; i++)
            {
                Vector2D ff= dir * dl*(i+1);
                rebars.Add(new SingleRebar(st+ff,Ds,RebarProperty.Fy,RebarProperty.Es));
            }
        }

        private void GenerateOutline(double offset)
        {
            double x1 = -0.5 *B;
            double x2 = x1 + Cb;
            double y1 = -0.5 * H;
            double y2 = y1 + Ch;
            Point2D Pt1 = new Point2D(-x2, y1);
            Point2D Pt2 = new Point2D(-x1, y2);
            Point2D Pt3 = new Point2D(-x1, -y2);
            Point2D Pt4 = new Point2D(-x2, -y1);
            Point2D Pt5 = new Point2D(x2, -y1);
            Point2D Pt6 = new Point2D(x1, -y2);
            Point2D Pt7 = new Point2D(x1, y2);
            Point2D Pt8 = new Point2D(x2, y1);
            OuterPts = new List<Point2D>() { Pt1, Pt2, Pt3, Pt4, Pt5, Pt6, Pt7, Pt8 };
            OuterLns = new List<Line2D>()
            {
                new Line2D(Pt1,Pt2),
                new Line2D(Pt2,Pt3),
                new Line2D(Pt3,Pt4),
                new Line2D(Pt4,Pt5),
                new Line2D(Pt5,Pt6),
                new Line2D(Pt6,Pt7),
                new Line2D(Pt7,Pt8),
                new Line2D(Pt8,Pt1),
            };

        }



        public double DistRebar
        {
            get
            {
                return Rebars[0].Location.DistanceTo(Rebars[1].Location);
            }
        }

        Point2D CalPC()
        {
            int div = 2000;
            double dy = H / (div - 1);
            double dx = B / (div - 1);
            double AC = 0.0;
            double EACY = 0;
            double EASX = 0.0;
            double EACX = 0;
            double EASY = 0.0;
            double EAS, EAC;
            EAS = 0;
            EAC = (B * H - 2 * Ch * Cb) * ConcProperty.Ec;


            for (int i = 0; i < div; i++)
            {
                double y0 = -0.5 * H + dy * i;
                double l0 = CutLengY(y0);
                EACY += ConcProperty.Ec * l0 * dy * y0;

                double x0 = -0.5 * B + dx * i;
                double lx0 = CutLengX(x0);
                EACX += ConcProperty.Ec * lx0 * dx * x0;
            }

            foreach (SingleRebar item in Rebars)
            {
                EASX += RebarProperty.Es * item.As * item.Location.X;
                EASY += RebarProperty.Es * item.As * item.Location.Y;
                EAS += RebarProperty.Es * item.As;
            }
            return new Point2D((EACX + EASX) / (EAS + EAC),(EACY + EASY) / (EAS + EAC));

        }
    }
}
