using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedTheoryConcreteStructure.Material;

namespace UnifiedTheoryConcreteStructure.Section
{
    public class RoundRecSection:BaseSection
    {

        public double Cr;

        public override double Ag { get { return (B * H - (4 - Math.PI) * Cr * Cr) ; } }
        public override double Astotal { get { return Ds * Ds * 0.25 * Math.PI * NumRebars; } }

        public override double P0()
        {
            return 0.85 * ConcProperty.Fck_cylinder * (Ag - Astotal) + RebarProperty.Fy * Astotal;
        }

        public RoundRecSection(double b, double h, ConcreteAASHTO concProperty, Reinforcement rebarProperty, int numRebars, double ds, double cc = 50, double cr = 100)
        {
            B = b;
            H = h;
            ConcProperty = concProperty;
            RebarProperty = rebarProperty;
            NumRebars = numRebars;
            Ds = ds;
            Cc = cc;
            Cr = cr;           
            GenerateOutline();
            GenerateRebars();
            PlasticCenter = CalPC();
        }

        private void GenerateOutline()
        {
            double x1 = -0.5 * B;
            double y1 = -0.5 * H;
            double x2=x1+Cr;
            double y2=y1+Cr;
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
                new Line2D(Pt2,Pt3),                
                new Line2D(Pt4,Pt5),                
                new Line2D(Pt6,Pt7),                
                new Line2D(Pt8,Pt1),
            };
        }
        private void AddRebars(ref List<SingleRebar> rebars, Point2D st, Point2D ed, int v)
        {
            double dl = ed.DistanceTo(st) / (v + 1);
            Vector2D dir = st.VectorTo(ed).Normalize();
            for (int i = 0; i < v; i++)
            {
                Vector2D ff = dir * dl * (i + 1);
                rebars.Add(new SingleRebar(st + ff, Ds, RebarProperty.Fy, RebarProperty.Es));
            }
        }



        public override double GetPnX(double e0)
        {
            double n0 = -100e3;
            double n1 = -P0();
            double nmid = (n0 + n1) * 0.5;
            double error = 1;
            double err1, err0, err2;
            for (int i = 0; i < 50; i++)
            {
                err0 = Math.Abs(GetMnX(n0) / n0) - e0;
                err1 = Math.Abs(GetMnX(n1) / n1) - e0;
                err2 = Math.Abs(GetMnX(nmid) / nmid )- e0;
                if (err0 * err1 >= 0)
                {
                    break;
                }
                else if (err0 * err2 >= 0)
                {
                    n0 = nmid;
                    nmid = (n0 + n1) * 0.5;
                }
                else
                {
                    n1 = nmid;
                    nmid = (n0 + n1) * 0.5;
                }

                if (Math.Abs(n0 - n1) <= error)
                {
                    break;
                }
            }
            return nmid;
        }
        public override double GetPnY(double e0)
        {
            double n0 = -100e3;
            double n1 = -P0();
            double nmid = (n0 + n1) * 0.5;
            double error = 1;
            double err1, err0, err2;
            for (int i = 0; i < 50; i++)
            {
                err0 = Math.Abs(GetMnY(n0) / n0) - e0;
                err1 = Math.Abs(GetMnY(n1) / n1) - e0;
                err2 = Math.Abs(GetMnY(nmid) / nmid) - e0;
                if (err0 * err1 >= 0)
                {
                    break;
                }
                else if (err0 * err2 >= 0)
                {
                    n0 = nmid;
                    nmid = (n0 + n1) * 0.5;
                }
                else
                {
                    n1 = nmid;
                    nmid = (n0 + n1) * 0.5;
                }

                if (Math.Abs(n0 - n1) <= error)
                {
                    break;
                }
            }
            return nmid;
        }
        public override double GetMnX(double Nn)
        {
            double c0 = 1;
            double c1 = 5 * B;
            double cmid = (c0 + c1) * 0.5;
            double error = 1e-5;
            double err1, err0, err2;
            int div = 1000;
            for (int i = 0; i < 50; i++)
            {
                err0 = CalNX(c0, div) - Nn;
                err1 = CalNX(c1, div) - Nn;
                err2 = CalNX(cmid, div) - Nn;
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

                if (Math.Abs(c0 - c1) <= error)
                {
                    break;
                }
            }
            return CalMX(cmid, div);
        }

        public override double GetMnY(double Nn)
        {
            double c0 = 1;
            double c1 = 5 * H;
            double cmid = (c0 + c1) * 0.5;
            double error = 1e-5;
            double err1, err0, err2;
            int div = 1000;
            for (int i = 0; i < 50; i++)
            {
                err0 = CalNY(c0, div) - Nn;
                err1 = CalNY(c1, div) - Nn;
                err2 = CalNY(cmid, div) - Nn;
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

                if (Math.Abs(c0 - c1) <= error)
                {
                    break;
                }
            }
            return CalMY(cmid, div);
        }

        private double CalMX(double c, int div = 1000)
        {
            double dx = B / (div - 1);
            double phi = -ConcProperty.Epsu / c;
            double oc = B * 0.5 - c;
            double MC = 0;
            double MS = 0.0;

            for (int i = 0; i < div; i++)
            {
                double x0 = -0.5 * B + dx * i;
                double l0 = CutLengX(x0);
                double eps = phi * (x0 - oc);
                double Nconc = ConcProperty.GetSigma(eps) * dx * l0;
                MC += Nconc * (x0 - PlasticCenter.Y);
            }

            foreach (SingleRebar item in Rebars)
            {
                double eps = phi * (item.Location.X - oc);
                double Nsteel = RebarProperty.GetSigma(eps);
                MS += Nsteel * (item.Location.X - PlasticCenter.X);
            }
            return MS + MC;
        }

        private double CalMY(double c, int div = 1000)
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
                MC += Nconc * (y0 - PlasticCenter.Y);
            }

            foreach (SingleRebar item in Rebars)
            {
                double eps = phi * (item.Location.Y - oc);
                double Nsteel = RebarProperty.GetSigma(eps);
                MS += Nsteel * (item.Location.Y - PlasticCenter.Y);
            }
            return MS + MC;
        }

        private double CalNX(double c, int div = 1000)
        {
            double dx = B / (div - 1);
            double phi = -ConcProperty.Epsu / c;
            double oc = 0.5*B - c;
            double NC = 0;
            double NS = 0.0;
            for (int i = 0; i < div; i++)
            {
                double x0 = -0.5 * B + dx * i;
                double l0 = CutLengX(x0);
                double eps = phi * (x0 - oc);
                double Nconc = ConcProperty.GetSigma(eps) * dx * l0;
                NC += Nconc;
            }

            foreach (SingleRebar item in Rebars)
            {
                double eps = phi * (item.Location.X - oc);
                double Nsteel = RebarProperty.GetSigma(eps);
                NS += Nsteel;
            }
            return NS + NC;

        }

        private double CalNY(double c, int div = 1000)
        {
            double dy = H / (div - 1);
            double phi = -ConcProperty.Epsu / c;
            double oc = H * 0.5 - c;
            double NC = 0;
            double NS = 0.0;
            for (int i = 0; i < div; i++)
            {
                double y0 = -0.5 * H + dy * i;
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

        private void GenerateRebars()
        {
            Rebars = new List<SingleRebar>();
            double x1 = -0.5 * B + Cc;
            double y1 = -0.5 * H + Cc;
            double x2;
            double y2;
            x2 = -0.5 * B + Cr;
            y2 = -0.5 * H + Cr;


            
            double ChR = Cr*Math.PI*0.5;
            double TotalL = Math.Abs(4 * x2 + 4 * y2) + ChR * 4;
            int CNum = (int)Math.Round(ChR / (TotalL / NumRebars)) - 1;
            CNum = CNum < 0 ? 0 : CNum;
            int Bnum = (int)Math.Round(2 * Math.Abs(y2) / (TotalL / NumRebars)) - 1;
            Bnum = Bnum < 0 ? 0 : Bnum;
            int Hnum = (NumRebars - 8 - 4 * CNum - 2 * Bnum) / 2;

            Point2D Pt1 = new Point2D(-x2, y1);
            Point2D Pt2 = new Point2D(-x1, y2);
            Point2D Pt3 = new Point2D(-x1, -y2);
            Point2D Pt4 = new Point2D(-x2, -y1);
            Point2D Pt5 = new Point2D(x2, -y1);
            Point2D Pt6 = new Point2D(x1, -y2);
            Point2D Pt7 = new Point2D(x1, y2);
            Point2D Pt8 = new Point2D(x2, y1);



            AddRebars(ref Rebars, Pt8, Pt1, Bnum);
            AddRebars(ref Rebars, Pt5, Pt4, Bnum);
            AddRebars(ref Rebars, Pt2, Pt3, Hnum);
            AddRebars(ref Rebars, Pt6, Pt7, Hnum);

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
            EAC = Ag*ConcProperty.Ec;


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
            return new Point2D((EACX + EASX) / (EAS + EAC), (EACY + EASY) / (EAS + EAC));

        }


        private double CutLengX(double x0)
        {
            if (Math.Abs(x0) > 0.5 *B)
            {
                x0 = 0.5 * B;
            }
            if (Math.Abs(x0) <= OuterPts[0].X)
            {
                return H;
            }
            else
            {
                double n = Math.Abs(x0) - OuterPts[0].X;
                double v = 2 * (Cr - Math.Sqrt(Cr * Cr - n * n));
                return H - v;
            }
        }
        private double CutLengY(double y0)
        {
            if (Math.Abs(y0) > 0.5*H)
            {
                y0 = 0.5 * H;
            }
            if (Math.Abs(y0) <= -OuterPts[1].Y)
            {
                return B;
            }
            else
            {
                double n = Math.Abs(y0) + OuterPts[1].Y;
                double v = 2 * (Cr - Math.Sqrt(Cr * Cr - n * n));
                return B - v;
            }
        }




    }
}
