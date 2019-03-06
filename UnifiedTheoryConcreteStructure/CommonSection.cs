using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AutoCAD;
using MathNet.Spatial.Euclidean;

namespace UnifiedTheoryConcreteStructure
{
    class CommonSection
    {
        public Polygon2D OuterLine;
        public List<Polygon2D> InnerLines;
        public List<Circle2D> ComRebars;
        public List<Circle2D> PreRebars;

        public Reinforcement MRebar;
        public Reinforcement SRebar;
        public Concrete Conc;

        List<double[]> Xstrip;
        List<double[]> Ystrip;
        List<double> xs, ys, Axs, Ays;
        double dx, dy;

        public double As
        {
            get
            {
                var f = from a in ComRebars select a.Area;
                return f.Sum();
            }
        }

        public double Ac
        {
            get
            {
                return (Ays.Sum() + Axs.Sum()) * 0.5;
            }
        }

        public double Scx
        {
            get
            {
                return xs.Product(ref Axs);
            }
        }
        public double Scy
        {
            get
            {
                return ys.Product(ref Ays);
            }
        }
        public double Ssx
        {
            get
            {
                var f = from a in ComRebars select a.Area * a.Center.X;
                return f.Sum();
            }
        }
        public double Ssy
        {
            get
            {
                var f = from a in ComRebars select a.Area * a.Center.Y;
                return f.Sum();
            }
        }
        public double PlasticCenterX
        {
            get
            {
                return (Scx * Conc.Ec + Ssx * MRebar.Es) / (Ac * Conc.Ec + As * MRebar.Es);
            }
        }

        public double PlasticCenterY
        {
            get
            {
                return (Scy * Conc.Ec + Ssy * MRebar.Es) / (Ac * Conc.Ec + As * MRebar.Es);
            }
        }
        public double DistX
        {
            get
            {
                return OuterLine.Xmax() - OuterLine.Xmin();
            }

        }
        public double DistY
        {
            get
            {
                return OuterLine.Ymax() - OuterLine.Ymin();
            }
        }


        public CommonSection()
        {
            OuterLine = null;
            ComRebars = new List<Circle2D>();
            PreRebars = new List<Circle2D>();
            InnerLines = new List<Polygon2D>();
        }

        public CommonSection(Concrete conc,Reinforcement bar)
        {
            OuterLine = null;
            ComRebars = new List<Circle2D>();
            PreRebars = new List<Circle2D>();
            InnerLines = new List<Polygon2D>();
            MRebar = bar;
            Conc = conc;
            SRebar = bar;

        }
        /// <summary>
        /// 一般截面
        /// </summary>
        /// <param name="fcu"> 混凝土强度标准值</param>
        /// <param name="fsk"> 钢筋强度标准值</param>
        public CommonSection(double fcu, double fsk)
        {
            OuterLine = null;
            ComRebars = new List<Circle2D>();
            PreRebars = new List<Circle2D>();
            InnerLines = new List<Polygon2D>();
            GetSection();
           
            MRebar = new Reinforcement(fsk);
            Conc = new Concrete(fcu);
            SRebar = new Reinforcement();
        }

        public void InitialMaterial(Concrete conc,Reinforcement rein)
        {
            Conc = conc;
            MRebar = rein;

        }




        List<double[]> GetStrip(ref Polygon2D outer, ref List<Polygon2D> inner, bool isVertical, double dv0 = 0.1)
        {
            List<double[]> strip = new List<double[]>();
            var Ys = from Point2D pt in outer.Vertices select pt.Y;
            var Xs = from Point2D pt in outer.Vertices select pt.X;
            double x0 = Xs.Min();
            double x1 = Xs.Max();
            double y0 = Ys.Min();
            double y1 = Ys.Max();
            double vi, dv, v0, v1;
            int steps;
            if (isVertical)
            {
                v0 = x0;
                v1 = x1;
            }
            else
            {
                v0 = y0;
                v1 = y1;
            }
            steps = (int)Math.Round((v1 - v0) / dv0);
            dv = (v1 - v0) / steps;

            for (int i = 0; i < steps; i++)
            {
                double length = 0;
                vi = v0 + i * dv + 0.5 * dv;
                var pts = OuterLine.GetCrossPoints(vi, isVertical);
                foreach (Polygon2D inline in InnerLines)
                {
                    var ptsi = inline.GetCrossPoints(vi, isVertical);
                    pts.AddRange(ptsi);
                }
                if (isVertical)
                {
                    pts.Sort((pt1, pt2) => pt1.Y.CompareTo(pt2.Y));
                }
                else
                {
                    pts.Sort((pt1, pt2) => pt1.X.CompareTo(pt2.X));
                }




                for (int j = 0; j < pts.Count; j++)
                {
                    if (j % 2 == 0)
                    {
                        length += pts[j].DistanceTo(pts[j + 1]);
                    }
                }
                strip.Add(new double[2] { vi, length });
            }
            return strip;
        }





        public void GetSection()
        {
            AcadApplication AcadApp = null;
            AcadDocument doc = null;
            AcadSelectionSets SelSets = null;
            AcadSelectionSet curSelection = null;
            InnerLines.Clear();
            try
            {


#if R22
                Console.WriteLine("正在连接CAD2018...成功");
                AcadApp = (AcadApplication)Marshal.GetActiveObject("AutoCAD.Application.22");
#elif R20
                Console.WriteLine("正在连接CAD2016...成功");
                AcadApp = (AcadApplication)Marshal.GetActiveObject("AutoCAD.Application.20");
#elif R19
                Console.WriteLine("正在连接CAD2014...成功");
                AcadApp = (AcadApplication)Marshal.GetActiveObject("AutoCAD.Application.19");
#elif R18
                Console.WriteLine("正在连接CAD2010...成功");
                AcadApp = (AcadApplication)Marshal.GetActiveObject("AutoCAD.Application.18");
#endif


                doc = AcadApp.ActiveDocument;
                SelSets = doc.SelectionSets;
                Console.WriteLine("选择截面...");
            }
            catch (Exception)
            {
                Console.WriteLine("无法连接AutoCAD...");
                return;
            }


            try
            {
                curSelection = SelSets.Add("GetPLs");
            }
            catch (Exception)
            {
                var item = from AcadSelectionSet a in SelSets where a.Name == "GetPLs" select a;
                curSelection = item.First();
            }
            finally
            {
                curSelection.Clear();
            }


            curSelection.SelectOnScreen();


            Console.WriteLine("共选择{0}个对象。", curSelection.Count);            
            Globals.LogBox.AppendText(string.Format("共选择{0}个对象。", curSelection.Count));

            var PLs = from AcadEntity a in curSelection where a.EntityType == 24 select a as AcadLWPolyline;
            var Acs = from AcadLWPolyline a in PLs select a.Area;
            double AcMax = Acs.Max();

            foreach (AcadLWPolyline item in PLs)
            {
                if (item.Area == AcMax)
                {
                    OuterLine = item.Polygen2D();
                }
                else
                {
                    InnerLines.Add(item.Polygen2D());
                }
            }

            var bars = from AcadEntity a in curSelection where a.EntityType == 8 select a as AcadCircle;
            foreach (AcadCircle item in bars)
            {
                ComRebars.Add(item.Circle2D());

            }


            Console.WriteLine("共拾取1个外边界，{0}个内边界，{1}根钢筋...", InnerLines.Count, ComRebars.Count);

            Xstrip = GetStrip(ref OuterLine, ref InnerLines, true);
            Ystrip = GetStrip(ref OuterLine, ref InnerLines, false);
            xs = (from a in Xstrip select a[0]).ToList();
            ys = (from a in Ystrip select a[0]).ToList();
            dx = xs[1] - xs[0];
            dy = ys.ElementAt(1) - ys.ElementAt(0);
            Axs = (from a in Xstrip select a[1] * dx).ToList();
            Ays = (from a in Ystrip select a[1] * dy).ToList();
        }





        public Tuple<double, double> SectionAnalysis(double Nd = 0, bool isVert = true, double err = 1e-6)
        {
            double F = 0.0, M = 0.0;
            double Dist = isVert ? DistY : DistX;
            double center = isVert ? PlasticCenterY : PlasticCenterX;
            double top = isVert ? OuterLine.Ymax() : OuterLine.Xmax();

            double c1 = -20 * Dist;
            double c2 = top - center - 1;

            Tuple<double, double> inp, ret;
            int i;
            inp = new Tuple<double, double>(c1, c2);

            i = 0;
            while (Math.Abs(inp.Item1 - inp.Item2) > err)
            {
                ret = TrialFx(inp.Item1, inp.Item2, Nd, isVert);
                inp = ret;
                i++;
                if (i >= 1000)
                {
                    Console.WriteLine("迭代未收敛");
                }
            }
            double cc = 0.5 * (inp.Item2 + inp.Item1);
            F = CalC(cc, isVert).Item1 + CalS(cc, isVert).Item1;
            M = CalC(cc, isVert).Item2 + CalS(cc, isVert).Item2;

            Globals.LogBox.AppendText( string.Format("Fu={0:F1}kN,Mu={1:F1}kNm",F/1e3,M/1e6));
            return new Tuple<double, double>(F, M);
        }



        public Tuple<double, double> CalC(double NeutralDist, bool isVert = true)
        {

            var strip = isVert ? Ystrip : Xstrip;
            double center = isVert ? PlasticCenterY : PlasticCenterX;
            double top = isVert ? OuterLine.Ymax() : OuterLine.Xmax();
            double div = isVert ? dy : dx;
            double na = center + NeutralDist;
            int steps = strip.Count;
            double dz, zi, bb;
            double phi = -Conc.epsu / (top - na);
            double epsi, sigmai;
            double Fi, Mi;
            double F = 0.0, M = 0.0;

            for (int i = 0; i < steps; i++)
            {
                zi = strip[i][0];
                dz = zi - na;

                epsi = dz * phi;
                //sigmai = Conc.sigma01(epsi, -7.9695);
                sigmai = Conc.sigma02(epsi);
                bb = strip[i][1];
                Fi = bb * div * sigmai;
                Mi = Fi * (zi - center);
                F += Fi;
                M += Mi;
            }

            return new Tuple<double, double>(F, M);
        }

        public Tuple<double, double> CalS(double NeutralDist, bool isVert = true)
        {
            double center = isVert ? PlasticCenterY : PlasticCenterX;
            double top = isVert ? OuterLine.Ymax() : OuterLine.Xmax();
            double na = center + NeutralDist;
            int steps = ComRebars.Count;
            double F = 0.0, M = 0.0;
            double dz, zi;
            double phi = -Conc.epsu / (top - na);
            double epsi, sigmai;
            double Fi, Mi;

            for (int i = 0; i < steps; i++)
            {
                zi = isVert ? ComRebars[i].Center.Y : ComRebars[i].Center.X;
                dz = zi - na;
                epsi = dz * phi;
                sigmai = MRebar.sigma(epsi);
                Fi = sigmai * ComRebars[i].Area;
                Mi = Fi * (zi - center);
                F += Fi;
                M += Mi;
            }
            return new Tuple<double, double>(F, M);
        }

        Tuple<double, double> TrialFx(double Loc1, double Loc2, double Fx, bool isVert = true)
        {
            double ret1, ret2;
            double Loc0 = 0.5 * (Loc1 + Loc2);
            double dFx0 = CalC(Loc0, isVert).Item1 + CalS(Loc0, isVert).Item1 - Fx;
            double dFx1 = CalC(Loc1, isVert).Item1 + CalS(Loc1, isVert).Item1 - Fx;
            double dFx2 = CalC(Loc2, isVert).Item1 + CalS(Loc2, isVert).Item1 - Fx;

            if (dFx1 * dFx2 > 0)
            {
                throw new Exception();
            }
            else if (dFx0 * dFx1 > 0)
            {
                ret1 = Loc0;
                ret2 = Loc2;
            }
            else
            {
                ret1 = Loc1;
                ret2 = Loc0;
            }
            return new Tuple<double, double>(ret1, ret2);
        }
        public Tuple<double, double> TrialE(double c1, double c2, double E)
        {
            double ret1, ret2;
            double c0 = 0.5 * (c1 + c2);
            var resC0 = CalC(c0);
            var resS0 = CalS(c0);
            var resC1 = CalC(c1);
            var resS1 = CalS(c1);
            var resC2 = CalC(c2);
            var resS2 = CalS(c2);

            var M0 = (resC0.Item2 + resS0.Item2);
            var M1 = (resC1.Item2 + resS1.Item2);
            var M2 = (resC2.Item2 + resS2.Item2);

            var F0 = (resC0.Item1 + resS0.Item1);
            var F1 = (resC1.Item1 + resS1.Item1);
            var F2 = (resC2.Item1 + resS2.Item1);

            var E0 = M0 / F0;
            var E1 = M1 / F1;
            var E2 = M2 / F2;

            double dE0 = E0 - E;
            double dE1 = E1 - E;
            double dE2 = E2 - E;

            if (dE1 * dE2 > 0)
            {
                throw new Exception();
            }
            else if (dE0 * dE1 > 0)
            {
                ret1 = c0;
                ret2 = c2;
            }
            else
            {
                ret1 = c1;
                ret2 = c0;
            }
            return new Tuple<double, double>(ret1, ret2);
        }
    }
}
