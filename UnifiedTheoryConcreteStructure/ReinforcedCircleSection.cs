using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace UnifiedTheoryConcreteStructure
{
    class ReinforcedCircleSection
    {        
        public double D
        {
            set;
            get;
        }        
        public double Sv
        {
            set;
            get;
        }
        public double CoverT
        {
            set;
            get;
        }        
        
        public int NumRebar
        {
            set;
            get;
        }       
        
        public Reinforcement MRebar;
        public Reinforcement SRebar;
        public Concrete Conc;
      


        // 构造函数
        public ReinforcedCircleSection()
        {
            D = 0;                       
            Sv = 0;
            CoverT = 0.0;           
            NumRebar = 0;
            MRebar = new Reinforcement();
            SRebar = new Reinforcement();            
            Conc = new Concrete();
        }

      

        /// <summary>
        /// 配置圆柱截面
        /// </summary>  
        public ReinforcedCircleSection(
            double dd, double cover, double fcu,
            int nrebar, double dia, double fy,            
            double sv, double diav, double fyv)
        {
            D = dd;
            Sv = sv;
            CoverT = cover;
            NumRebar = nrebar;
            MRebar = new Reinforcement(dia,fy);
            SRebar = new Reinforcement(diav,fyv);
            Conc = new Concrete(fcu);
        }


        


        public Tuple<double,double> CalC(double cc,double div=1)
        {
            double dz,zi;
            double phi = -Conc.epsu / cc;
            double epsi,sigmai;
            double bb, R = 0.5 * D;
            double Fi, Mi;
            double F=0.0, M=0.0;

            for (int i = 0; i*div < D; i++)
            {
                zi = -R + i * div;
                dz = zi-(R-cc);
                
                epsi = dz * phi;
                //sigmai = Conc.sigma01(epsi, -7.9695);
                sigmai = Conc.sigma02(epsi);
                bb = 2.0*Math.Sqrt(R * R - zi * zi);
                Fi = bb * div * sigmai;
                Mi = Fi * zi;
                F += Fi;
                M += Mi;
            }

            return new Tuple<double, double>(F,M);
        }

        public Tuple<double, double> CalS(double cc)
        {
            double F = 0.0, M = 0.0;
            double dreg = Math.PI * 2 / NumRebar;
            double regi;
            double dz,zi;
            double phi =- Conc.epsu / cc;
            double R = 0.5 * D;
            double epsi, sigmai;
            double Fi, Mi;

            for (int i = 0; i < NumRebar; i++)
            {
                regi = i * dreg;
                zi = Math.Sin(regi)*R;
                dz = zi - (R - cc);
                epsi = dz * phi;
                sigmai = MRebar.sigma(epsi);
                Fi = sigmai * MRebar.As;
                Mi = Fi * zi;
                F += Fi;
                M += Mi;
            }
            return new Tuple<double, double>(F, M);
        }



        public Tuple<double,double> TrialFx(double c1,double c2,double Fx)
        {
            double ret1, ret2;
            double c0 = 0.5 * (c1 + c2);
            double dFx0 = CalC(c0).Item1 + CalS(c0).Item1-Fx;
            double dFx1 = CalC(c1).Item1 + CalS(c1).Item1-Fx;            
            double dFx2 = CalC(c2).Item1 + CalS(c2).Item1-Fx;

            if (dFx1 * dFx2 > 0)
            {
                throw new Exception();
            }
            else if(dFx0*dFx1>0)
            {
                ret1 = c0;
                ret2 = c2;
            }
            else
            {
                ret1 = c1;
                ret2 = c0;
            }
            return new Tuple<double, double>(ret1,ret2);
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




        public Tuple<double, double> SectionAnalysis(double Fx = 0, double My = 0, bool isConstantFx = true,double err=1e-6)
        {
            double F=0.0, M=0.0;            
            double c1 = 50 * D;
            double c2 = 1;            
            Tuple<double, double> inp, ret;
            int i;
            double E;
            if (Fx==0)
            {
                E = 0;
            }
            else
            {
                E = Math.Abs(My / Fx);
            }

            if (isConstantFx)
            {
                inp = new Tuple<double, double>(c1, c2);

                i = 0;
                while (Math.Abs(inp.Item1 - inp.Item2) > err)
                {
                    ret = TrialFx(inp.Item1, inp.Item2, Fx);
                    inp = ret;
                    i++;
                    if (i>=1000)
                    {
                        Console.WriteLine("迭代未收敛");
                    }
                }
                double cc = 0.5 * (inp.Item2 + inp.Item1);
                F= CalC(cc).Item1 + CalS(cc).Item1;
                M = CalC(cc).Item2 + CalS(cc).Item2;
            }
            else
            {
                c2 = 0.5*D;
                inp = new Tuple<double, double>(c1, c2);

                i = 0;
                while (Math.Abs(inp.Item1 - inp.Item2) > err)
                {
                    ret = TrialE(inp.Item1, inp.Item2, E);
                    inp = ret;
                    i++;
                    if (i >= 1000)
                    {
                        Console.WriteLine("迭代未收敛");
                    }
                }
                double cc = 0.5 * (inp.Item2 + inp.Item1);
                F = CalC(cc).Item1 + CalS(cc).Item1;
                M = CalC(cc).Item2 + CalS(cc).Item2;
            }
            return new Tuple<double, double>(F, M);
        }


        public void SectionABCD(double Fx = 0, double My = 0, bool isConstantFx = true, double err = 1e-6)
        {
            var ff = Resource1.ABCD;
            Decoder d = Encoding.UTF8.GetDecoder();
            int charSize = d.GetCharCount(ff, 0, ff.Length);
            char[] chs = new char[charSize];
            d.GetChars(ff, 0, ff.Length, chs, 0);
            string s = new string(chs);
            var ss=s.Split('\n');
            double[,] ABCD = new double[ss.Length, 7];
            double[] rcd;
            ArrayList al = new ArrayList();
            double kesi, AA, BB, CC, DD, Mu, Nu;
            double rho = MRebar.As * NumRebar / (0.25 * Math.PI * Math.Pow(D, 2));
            double g = (D - CoverT * 2) / D;

            for (int i = 0; i < ss.Length; i++)
            {
                string item = ss[i];
                item=item.Trim();   
                var items = item.Split(',');
                
                kesi = double.Parse(items[0]);
                AA = double.Parse(items[1]);
                BB = double.Parse(items[2]);
                CC = double.Parse(items[3]);
                DD = double.Parse(items[4]);
                Mu= BB * Math.Pow(0.5 * D, 3) * Conc.Fcd + DD * rho * g * Math.Pow(0.5 * D, 3) * MRebar.Fy;
                Nu= AA * Math.Pow(0.5 * D, 2) * Conc.Fcd + CC * rho * Math.Pow(0.5 * D, 2) * MRebar.Fy;
                rcd = new double[7] { kesi, AA, BB, CC, DD, Mu, Nu };
                al.Add(rcd);
            }

            //for (int i = 0; i < (ABCD.Length/7); i++)
            //{
            //    kesi = ABCD[i, 0];
            //    AA = ABCD[i, 1];
            //    BB = ABCD[i, 2];
            //    CC = ABCD[i, 3];
            //    DD = ABCD[i, 4];

            //    ABCD[i, 5] = BB * Math.Pow(0.5 * D, 3) * Conc.Fcd + DD * rho * g * Math.Pow(0.5 * D, 3) * MRebar.Fy;
            //    ABCD[i, 6] = AA * Math.Pow(0.5 * D, 2) * Conc.Fcd + CC * rho * Math.Pow(0.5 * D, 2) * MRebar.Fy;
            //}



            var ret = from double []a in al select new double[] { a[5],a[6] };
            foreach (var item in ret)
            {
                Console.WriteLine("{0:F1}\t{1:F1}", -item[0] / 1e6,-item[1]/1e3);
            }



        }


    }
}
