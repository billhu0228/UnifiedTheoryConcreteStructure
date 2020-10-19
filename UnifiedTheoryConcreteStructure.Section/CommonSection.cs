using AutoCAD;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using UnifiedTheoryConcreteStructure.Material;
using UnifiedTheoryConcreteStructure.Public;

namespace UnifiedTheoryConcreteStructure.Section
{
    public class CommonSection
    {

        public CommonSection()
        {
            OuterLine = null;
            Rebars = new List<Circle2D>();
            PreRebars = new List<Circle2D>();
            InnerLines = new List<Polygon2D>();
            Xstrip = new List<double[]>();
            Ystrip = new List<double[]>();
            xs = new List<double>();
            ys = new List<double>();
            Axs = new List<double>();
            Ays = new List<double>();

            Conc = new ConcreteProperty();
            MRebar = new Reinforcement("HRB400",400,1,1,0,0);
            SRebar = new Reinforcement("HRB400", 400, 1, 1, 0, 0);
            PrestressProperty = new Prestress("G270",1860);
        }

        // 通过选择初始化一般截面
        public CommonSection( AcadSelectionSet curSelection)
        {
            OuterLine = null;
            Rebars = new List<Circle2D>();
            PreRebars = new List<Circle2D>();
            InnerLines = new List<Polygon2D>();
            Xstrip = new List<double[]>();
            Ystrip = new List<double[]>();
            xs = new List<double>();
            ys = new List<double>();
            Axs = new List<double>();
            Ays = new List<double>();
                            
            var bars = from AcadEntity a in curSelection where a.EntityType == 8 select a as AcadCircle;            
            foreach (AcadCircle item in bars)
            {
                if ((int)item.color==6)
                {
                    PreRebars.Add(item.Circle2D());
                }
                else
                {
                    Rebars.Add(item.Circle2D());
                }
               
            }
            var tmp = from AcadEntity a in curSelection where a.EntityType == 24 select a as AcadLWPolyline;
            List<AcadLWPolyline> pl2ds=tmp.ToList();
            pl2ds = pl2ds.OrderByDescending(u => u.Area).ToList();
            OuterLine = pl2ds[0].Polygen2D();
            pl2ds.Remove(pl2ds[0]);
            InnerLines = (from AcadLWPolyline a in pl2ds select a.Polygen2D()).ToList();
            //Xstrip = GetStrip(ref OuterLine, ref InnerLines, true);
            //Ystrip = GetStrip(ref OuterLine, ref InnerLines, false);
            //Axs = (from a in Xstrip select a[1]).ToList();
            //Ays = (from a in Ystrip select a[1]).ToList();
            //xs = (from a in Xstrip select a[0]).ToList();
            //ys = (from a in Ystrip select a[0]).ToList();
            GenStrip();
        }


        #region 字段
        public Polygon2D OuterLine;
        public List<Polygon2D> InnerLines;
        public List<Circle2D> Rebars;
        public List<Circle2D> PreRebars;

        public Reinforcement MRebar;
        public Reinforcement SRebar;
        public ConcreteProperty Conc;
        public Prestress PrestressProperty;

        List<double[]> Xstrip;
        List<double[]> Ystrip;
        List<double> xs, ys, Axs, Ays;
        double dx, dy;
        #endregion

        #region 属性
        public double As
        {
            get
            {
                var f = from a in Rebars select a.Area;
                return f.Sum();
            }
        }


        public double Ap
        {
            get
            {
                var f = from a in PreRebars select a.Area;
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

        /// <summary>
        /// 换算截面面积
        /// </summary>
        public double A0
        {
            get
            {
                return (Ac-As-Ap) + (As * MRebar.Es+Ap*PrestressProperty.Ep) / Conc.Ec;
            }
        }
        public double Icx
        {
            get
            {
                return (ys.VecAdd(-PlasticCenterY).Dot(ys.VecAdd(-PlasticCenterY)).Product(ref Ays));
            }
        }

        public double Icy
        {
            get
            {
                return (xs.VecAdd(-PlasticCenterX).Dot(xs.VecAdd(-PlasticCenterX)).Product(ref Axs));
            }
        }

        //钢筋绕x轴惯性矩
        double Isx
        {
            get
            {
                double res = 0;
                foreach (var item in Rebars)
                {
                    res += item.Area * (MRebar.Es / Conc.Ec) * (item.Center.Y - PlasticCenterY) * (item.Center.Y - PlasticCenterY);
                }
                return res;
            }
        }
        //钢筋绕y轴惯性矩
        double Isy
        {
            get
            {
                double res = 0;
                foreach (var item in Rebars)
                {
                    res += item.Area * (MRebar.Es / Conc.Ec) * (item.Center.X - PlasticCenterX) * (item.Center.X - PlasticCenterX);
                }
                return res;
            }
        }



        //预应力筋绕x轴惯性矩
        double Ipx
        {
            get
            {
                double res = 0;
                foreach (var item in PreRebars)
                {
                    res += item.Area * (PrestressProperty.Ep / Conc.Ec) * (item.Center.Y - PlasticCenterY) * (item.Center.Y - PlasticCenterY);
                }
                return res;
            }
        }
        //预应力筋绕y轴惯性矩
        double Ipy
        {
            get
            {
                double res = 0;
                foreach (var item in PreRebars)
                {
                    res += item.Area * (PrestressProperty.Ep / Conc.Ec) * (item.Center.X - PlasticCenterX) * (item.Center.X - PlasticCenterX);
                }
                return res;
            }
        }



        double Scy
        {
            get
            {
                return xs.Product(ref Axs);
            }
        }
        double Scx
        {
            get
            {
                return ys.Product(ref Ays);
            }
        }
        double Ssy
        {
            get
            {
                if (Rebars==null)
                {
                    return 0;
                }
                var f = from a in Rebars select a.Area * a.Center.X;
                return f.Sum();
            }
        }
        double Ssx
        {
            get
            {
                if (Rebars == null)
                {
                    return 0;
                }
                var f = from a in Rebars select a.Area * a.Center.Y;
                return f.Sum();
            }
        }



        double Spy
        {
            get
            {
                if (Rebars == null)
                {
                    return 0;
                }
                var f = from a in PreRebars select a.Area * a.Center.X;
                return f.Sum();
            }
        }
        double Spx
        {
            get
            {
                if (Rebars == null)
                {
                    return 0;
                }
                var f = from a in PreRebars select a.Area * a.Center.Y;
                return f.Sum();
            }
        }


        public double PlasticCenterX
        {
            get
            {
                return (Scy * Conc.Ec + Ssy * MRebar.Es+Spy*PrestressProperty.Ep) /(A0*Conc.Ec);
            }
        }
        public double PlasticCenterY
        {
            get
            {
                return (Scx * Conc.Ec + Ssx * MRebar.Es+Spx*PrestressProperty.Ep) /(A0*Conc.Ec);
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

        public double Ix0 { get { return Icx + Isx + Ipx; } }
        public double Iy0 { get { return Icy + Isy + Ipy; } }

        #endregion

        #region 方法

        void GenStrip(double dv0 = 0.05)
        {
            bool isVertical;
            List<double[]> strip;
            foreach (var item in new bool[]{ true,false})
            {
                strip = new List<double[]>();
                isVertical = item;
                var Ys = from Point2D pt in OuterLine.Vertices select pt.Y;
                var Xs = from Point2D pt in OuterLine.Vertices select pt.X;
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
                if (isVertical)
                {
                    dx = dv;
                    Xstrip = strip;
                    Axs = (from a in Xstrip select dv * a[1]).ToList();
                    xs = (from a in Xstrip select a[0]).ToList();

                }
                else
                {
                    dy = dv;
                    Ystrip = strip;
                    Ays = (from a in Ystrip select dv * a[1]).ToList();
                    ys = (from a in Ystrip select a[0]).ToList();
                }
            }
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
               
        public Tuple<double, double> SectionAnalysis(out double CC,double Nd = 0, bool isVert = true ,double err = 1e-6)
        {
            double F, M;
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
                    CC = double.NaN;
                    return new Tuple<double, double>(-1, -1);
                    //Console.WriteLine("迭代未收敛");
                }
            }
            double cc = 0.5 * (inp.Item2 + inp.Item1);
            F = CalC(cc, isVert).Item1 + CalS(cc, isVert).Item1 + CalP(cc, isVert).Item1;
            M = CalC(cc, isVert).Item2 + CalS(cc, isVert).Item2 + CalP(cc, isVert).Item2;

            //Globals.LogBox.AppendText( string.Format("Fu={0:F1}kN,Mu={1:F1}kNm",F/1e3,M/1e6));
            CC = cc;
            return new Tuple<double, double>(F, M);
        }

        Tuple<double, double> CalC(double NeutralDist, bool isVert = true)
        {

            var strip = isVert ? Ystrip : Xstrip;
            double center = isVert ? PlasticCenterY : PlasticCenterX;
            double top = isVert ? OuterLine.Ymax() : OuterLine.Xmax();
            double div = isVert ? dy : dx;
            double na = center + NeutralDist;
            int steps = strip.Count;
            double dz, zi, bb;
            double phi = -Conc.Epsu / (top - na);
            double epsi, sigmai;
            double Fi, Mi;
            double F = 0.0, M = 0.0;

            for (int i = 0; i < steps; i++)
            {
                zi = strip[i][0];
                dz = zi - na;

                epsi = dz * phi;
                //sigmai = Conc.sigma01(epsi, -7.9695);
                sigmai = Conc.GetSigma(epsi);
                bb = strip[i][1];
                Fi = bb * div * sigmai;
                Mi = Fi * (zi - center);
                F += Fi;
                M += Mi;
            }

            return new Tuple<double, double>(F, M);
        }

        Tuple<double, double> CalS(double NeutralDist, bool isVert = true)
        {
            double center = isVert ? PlasticCenterY : PlasticCenterX;
            double top = isVert ? OuterLine.Ymax() : OuterLine.Xmax();
            double na = center + NeutralDist;
            int steps = Rebars.Count;
            double F = 0.0, M = 0.0;
            double dz, zi;
            double phi = -Conc.Epsu / (top - na);
            double epsi, sigmai;
            double Fi, Mi;

            for (int i = 0; i < steps; i++)
            {
                zi = isVert ? Rebars[i].Center.Y : Rebars[i].Center.X;
                dz = zi - na;
                epsi = dz * phi;
                sigmai = MRebar.GetSigma(epsi);
                Fi = sigmai * Rebars[i].Area;
                Mi = Fi * (zi - center);
                F += Fi;
                M += Mi;
            }
            return new Tuple<double, double>(F, M);
        }

        Tuple<double, double> CalP(double NeutralDist, bool isVert = true)
        {
            double center = isVert ? PlasticCenterY : PlasticCenterX;
            double top = isVert ? OuterLine.Ymax() : OuterLine.Xmax();
            double na = center + NeutralDist;
            int steps = PreRebars.Count;
            double F = 0.0, M = 0.0;
            double dz, zi;
            double phi = -Conc.Epsu / (top - na);
            double epsi, sigmai;
            double Fi, Mi;

            for (int i = 0; i < steps; i++)
            {
                zi = isVert ? PreRebars[i].Center.Y : PreRebars[i].Center.X;
                dz = zi - na;
                epsi = dz * phi;
                sigmai = PrestressProperty.GetSigma(epsi);
                Fi = sigmai * PreRebars[i].Area;
                Mi = Fi * (zi - center);
                F += Fi;
                M += Mi;
            }
            return new Tuple<double, double>(F, M);
        }


        Tuple<double, double> TrialFx(double Loc1, double Loc2, double Fx, bool isVert = true)
        {
            double ret1=0, ret2=0;
            double Loc0 = 0.5 * (Loc1 + Loc2);
            double dFx0 = CalC(Loc0, isVert).Item1 + CalS(Loc0, isVert).Item1 + CalP(Loc0, isVert).Item1 - Fx;
            double dFx1 = CalC(Loc1, isVert).Item1 + CalS(Loc1, isVert).Item1 + CalP(Loc1, isVert).Item1 - Fx;
            double dFx2 = CalC(Loc2, isVert).Item1 + CalS(Loc2, isVert).Item1 + CalP(Loc2, isVert).Item1 - Fx;

            if (dFx1 * dFx2 > 0)
            {
                ;
                return new Tuple<double, double>(0, 0);
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

        //public Tuple<double, double> TrialE(double c1, double c2, double E)
        //{
        //    double ret1, ret2;
        //    double c0 = 0.5 * (c1 + c2);
        //    var resC0 = CalC(c0);
        //    var resS0 = CalS(c0);
        //    var resC1 = CalC(c1);
        //    var resS1 = CalS(c1);
        //    var resC2 = CalC(c2);
        //    var resS2 = CalS(c2);

        //    var M0 = (resC0.Item2 + resS0.Item2);
        //    var M1 = (resC1.Item2 + resS1.Item2);
        //    var M2 = (resC2.Item2 + resS2.Item2);

        //    var F0 = (resC0.Item1 + resS0.Item1);
        //    var F1 = (resC1.Item1 + resS1.Item1);
        //    var F2 = (resC2.Item1 + resS2.Item1);

        //    var E0 = M0 / F0;
        //    var E1 = M1 / F1;
        //    var E2 = M2 / F2;

        //    double dE0 = E0 - E;
        //    double dE1 = E1 - E;
        //    double dE2 = E2 - E;

        //    if (dE1 * dE2 > 0)
        //    {
        //        throw new Exception();
        //    }
        //    else if (dE0 * dE1 > 0)
        //    {
        //        ret1 = c0;
        //        ret2 = c2;
        //    }
        //    else
        //    {
        //        ret1 = c1;
        //        ret2 = c0;
        //    }
        //    return new Tuple<double, double>(ret1, ret2);
        //}


        #endregion







        #region 事件
        #endregion
        












    }
}
