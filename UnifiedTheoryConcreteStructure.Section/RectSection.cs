using System;
using System.Collections.Generic;
using System.Data;
using UnifiedTheoryConcreteStructure.Material;
using UnifiedTheoryConcreteStructure.Public;

namespace UnifiedTheoryConcreteStructure.Section
{
    public class RectSection
    {
        public string Name;
        public double LengthZ, LengthY, ChamferY,ChamferZ;
        public ChamferType ChaType;
        public ConcreteAASHTO thisConcrete;
        public Prestress thisPrestress;
        public Reinforcement RebarY0, RebarY1, RebarZ0, RebarZ1;
        public Stirrup VerticalRebarY,VerticalRebarZ;

        public RectSection(string name, double lengthZ, double lengthY, double cy=0,double cz=0)
        {
            Name = name;
            LengthZ = lengthZ;
            LengthY = lengthY;
            ChamferY = cy;
            ChamferZ = cz;
        }

        public double Pn
        {
            get
            {
                return 0.8*(0.85 * thisConcrete.Fck_cylinder * (A-ASteelTotal)+RebarZ1.Fy*ASteelTotal);
            }
        }

        public double A
        {
            get
            {
                switch (ChaType)
                {                    
                    case ChamferType.Round:
                        return LengthZ * LengthY - (ChamferY * ChamferY - Math.PI * ChamferY * ChamferY);                        
                    case ChamferType.Line:
                        var a1 = LengthZ * LengthY;
                        var a2 = 2*ChamferY * ChamferZ;                        
                        return a1 - a2;
                    default:
                        return LengthZ * LengthY - (ChamferY * ChamferY - Math.PI * ChamferY * ChamferY);
                }
            }
        }
        public double ASteelTotal
        {
            get
            {
                return RebarY0.AsTotal + RebarY1.AsTotal + RebarZ0.AsTotal + RebarZ1.AsTotal;
            }
        }

        public static double GetABalance(ConcreteAASHTO Conc,Reinforcement TensRebar, double DepthT)
        {
            return Conc.Beta1 * DepthT * Conc.Epsu / (Conc.Epsu + TensRebar.Epsy);  
        }
        public static double GetNBalance(double B, double H, ConcreteAASHTO Conc, Reinforcement TensRebar, Reinforcement CompRebar, double DepthT,double DepthC)
        {
            double a_balance = GetABalance(Conc, TensRebar, DepthT);
            var cb = a_balance / Conc.Beta1;
            var dcomp = H - DepthC;
            var eps_comp = Conc.Epsu * (cb - dcomp) / cb;
            double fs = eps_comp * CompRebar.Es;
            fs = fs >= CompRebar.Fy ? CompRebar.Fy : fs;
            double N_balance = 0.85 * Conc.Fck_cylinder * B * a_balance - TensRebar.AsTotal * TensRebar.Fy + CompRebar.AsTotal * fs;
            return N_balance;
        }
        public static double GetEBalance(double B, double H, ConcreteAASHTO Conc, Reinforcement TensRebar, Reinforcement CompRebar, double DepthT, double DepthC)
        {
            double a_balance = GetABalance(Conc, TensRebar, DepthT);
            double N_balance = GetNBalance(B, H, Conc, TensRebar, CompRebar, DepthT, DepthC);
            var cb = a_balance / Conc.Beta1;
            var dcomp = H - DepthC;
            var eps_comp = Conc.Epsu * (cb - dcomp) / cb;
            double fs = eps_comp * CompRebar.Es;
            if (fs>CompRebar.Fy)
            {
                fs = CompRebar.Fy;
            }
            return (0.85 * Conc.Fck_cylinder * B * a_balance * (DepthT - 0.5 * a_balance) +  CompRebar.AsTotal * fs * (DepthT - (H - DepthC))) / N_balance;            
        }










        public double GetMnZ(double Nn)
        {
            return 0.0;
        }









        public double GetMnY(double Nn)
        {

            double Mn = 0;
            double h = LengthY;
            double b = LengthZ;
            Reinforcement TensRebar = RebarY0;
            Reinforcement CompRebar = RebarY1;
            double DepthT = h - TensRebar.locY;
            double DepthC = Math.Abs(CompRebar.locY);
            double N_blance = GetNBalance(b, h, thisConcrete, TensRebar, CompRebar, DepthT, DepthC);
            double a_blance = GetABalance(thisConcrete, TensRebar, DepthT);
            double[] FFA = PubFuncs.GetFFA(-Math.Abs(Nn), a_blance, b, h,thisConcrete , TensRebar, CompRebar,DepthT,DepthC);
            double ftens = FFA[0];
            double fcomp = FFA[1];
            double a = FFA[2];
            double FailureMode = FFA[3];

            double dp = (0.85 * thisConcrete.Fck_cylinder * b * h * (DepthT - 0.5 * h) + CompRebar.AsTotal * CompRebar.Fy * (DepthT - (h - DepthC))) / (0.85 * thisConcrete.Fck_cylinder * b * h + CompRebar.AsTotal * CompRebar.Fy + TensRebar.AsTotal * TensRebar.Fy);

            if (FailureMode>3)
            {
                return 0;//C>H的破坏
            }
            else if (FailureMode==3)
            {
                //C<H受压
                double epp = (0.85 * thisConcrete.Fck_cylinder * b * a * (DepthT - 0.5 * a) - CompRebar.AsTotal * fcomp * (DepthT + DepthC - h)) /Math.Abs(Nn);

                Mn = (epp - dp) * Math.Abs(Nn);
            }
            else if (FailureMode <= 2)
            {
                // 受拉破坏
                double epp=  (0.85 * thisConcrete.Fck_cylinder * b * a * (DepthT - 0.5 * a) - CompRebar.AsTotal * fcomp * (DepthT + DepthC - h))/ Math.Abs(Nn);

                Mn = (epp - dp) * Math.Abs(Nn);
            }

            
            return Mn;
        }



















        public void GetMNCurveY(out List<Tuple<double, double>> MN_Y,  int npts = 20)
        {
            DataTable Summary = new DataTable("Y方向MN曲线");
            Summary.Gen(120, new string[] { "M", "N", "a", "e","fst","fsc" ,"Ncal"});

            MN_Y = new List<Tuple<double, double>>();
            double h = LengthY;
            double b = LengthZ;
            double Nn = 0.0;
            double Mu = 0.0;
            Reinforcement CompRebar = RebarY1;
            Reinforcement TensRebar = RebarY0;
            ConcreteAASHTO Conc = thisConcrete;
            double DepthT = h - TensRebar.locY;
            double DepthC = Math.Abs(CompRebar.locY);

            double a_balance = GetABalance(Conc, TensRebar, DepthT);
            double e_balance = GetEBalance(b, h, Conc, TensRebar, CompRebar, DepthT, DepthC);
            double N_balance = GetNBalance(b, h, Conc, TensRebar, CompRebar, DepthT, DepthC);



            double Nstep = N_balance / (npts - 1);
            Tuple<double, double> fsa = new Tuple<double, double>(0, 0);
            for (int i = 0; i < npts; i++)
            {
                Nn = N_balance - Nstep * i;
                fsa = PubFuncs.GetCompRebarFs(1, LengthY, LengthZ, LengthY, DepthC, thisConcrete, RebarY0, RebarY1, -Nn);
                Mu = 0.85 * thisConcrete.Fck_cylinder * LengthZ * fsa.Item2 * (DepthT - 0.5 * fsa.Item2) - RebarY1.AsTotal * fsa.Item1 * (DepthT - (LengthY - DepthC));
                MN_Y.Add(new Tuple<double, double>(Mu, Nn));
                double Ncal = 0.85 * Conc.Fck_cylinder * b * fsa.Item2 - RebarY0.AsTotal * RebarY0.Fy - RebarY1.AsTotal * fsa.Item1;
                Summary.WriteLine(i, new double[] {Mu,Nn,fsa.Item2,Mu/Nn,RebarY0.Fy,fsa.Item1 , Ncal });
            }

            double Estep = e_balance / (5*npts);
            double e = 0;
            for (int i = 0; i < 5*npts; i++)
            {
                e = e_balance - Estep * (i+1);
                fsa = PubFuncs.GetTensRebarFs(a_balance, Conc.Beta1 * h, b, h, DepthT,DepthC, Conc, TensRebar, CompRebar,e);
                Mu= 0.85 * thisConcrete.Fck_cylinder * b * fsa.Item2*(DepthT-0.5*fsa.Item2) + CompRebar.AsTotal * CompRebar.Fy* (DepthT - (LengthY - DepthC));
                //Nn = 0.85 * thisConcrete.Fck_cylinder * b * fsa.Item2 - TensRebar.AsTotal * fsa.Item1 + CompRebar.AsTotal * CompRebar.Fy;
                Summary.WriteLine(npts+i, new double[] { Mu, Mu/e, fsa.Item2,e,fsa.Item1,-RebarY1.Fy });

            }







        }
        
        /// <summary>
        /// 截面分析
        /// </summary>
        public void SectionAnalysis(int npts=20)
        {
            List<Tuple<double, double>> MN_Y = new List<Tuple<double, double>>();
            List<Tuple<double, double>> MN_Z = new List<Tuple<double, double>>();

            double N_balance = GetNBalance(LengthZ, LengthY, thisConcrete, RebarY0, RebarY1, LengthY - RebarY0.locY, Math.Abs(RebarY1.locY));
            double a_balance = GetABalance(thisConcrete, RebarY0, LengthY - RebarY0.locY);
                                                                              
            double Nstep = N_balance / (npts - 1);
            double Nn = 0.0;
            double Mu = 0.0;
            double DepthC = RebarY1.locY;
            double DepthT = LengthY - RebarY0.locY;
            Tuple<double, double> fsa=new Tuple<double, double>(0,0) ;
            for (int i = 0; i < npts; i++)
            {
                Nn = N_balance - Nstep * i;
                if (Nn<=N_balance)
                {
                    fsa = PubFuncs.GetCompRebarFs(1, LengthY, LengthZ, LengthY, DepthC, thisConcrete, RebarY0, RebarY1, -Nn);
                }
                
                Mu = 0.85 * thisConcrete.Fck_cylinder * LengthZ * fsa.Item2 * (DepthT - 0.5 * fsa.Item2) - RebarY1.AsTotal * fsa.Item1 * (DepthT - (LengthY - DepthC));
                MN_Y.Add(new Tuple<double, double>(Mu, Nn));
            }


            MN_Y.Sort((x, y) => x.Item2.CompareTo(y.Item2));


            ;


                    ////受拉破坏
                    //Console.WriteLine(string.Format("#  截面为<常轴力弯曲破坏>"));
                    //Tuple<double, double> fsa = GetFs(1, H, Nn);
                    //var Mu = 0.85 * Conc.Fcu * B * fsa.Item2 * (DepthT - 0.5 * fsa.Item2) - CompRebar.As * fsa.Item1 * (DepthT - (H - DepthC));
                    //Console.WriteLine(string.Format("#  a={0:F3}mm", fsa.Item2));
                    //Console.WriteLine(string.Format("#  fs={0:F3}MPa", fsa.Item1));
                    //Console.WriteLine(string.Format("#  极限承载力Mu={1:F0}kNm", fsa.Item1, Mu / 1e6));
                    //Console.WriteLine(string.Format("#  极限承载力φMu={1:F0}kNm", fsa.Item1, Mu / 1e6 * 0.75));





            return;
        }


















    }
}
