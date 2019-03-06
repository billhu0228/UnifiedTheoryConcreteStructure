using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UnifiedTheoryConcreteStructure
{
    class DoubleReinforcedRectSection
    {        
        public double B
        {
            set;
            get;
        }
        public double H
        {
            set;
            get;
        }
        public double Sv
        {
            set;
            get;
        }
        public double DepthT
        {
            set;
            get;
        }
        public double DepthC
        {
            set;
            get;
        }

        public double DepthPC
        {
            get
            {
                var fz = 0.85 * Conc.Fcu * B * H * (DepthT - 0.5 * H) + NumRebarC * CompRebar.As * CompRebar.Fy * (DepthT - (H - DepthC));
                var fm = 0.85 * Conc.Fcu * B * H + NumRebarC * CompRebar.As * CompRebar.Fy + NumRebarT * TensRebar.As * TensRebar.Fy;
                return fz / fm;
            }

        }

        public double a_balance
        {
            get
            {
                return Conc.Beta1 * DepthT * Conc.epsu / (Conc.epsu + TensRebar.epsy);
            }
        }
        public double N_balance
        {
            get
            {
                var cb = a_balance / Conc.Beta1;
                var dcomp = H - DepthC;
                var eps_comp = Conc.epsu * (cb - dcomp) / cb;
                return 0.85 * Conc.Fcu * B * a_balance - NumRebarT * TensRebar.As * TensRebar.Fy + NumRebarC * CompRebar.As * eps_comp * CompRebar.Es;
            }
        }
        public double e_balance
        {
            get
            {
                var cb = a_balance / Conc.Beta1;
                var dcomp = H - DepthC;
                var eps_comp = Conc.epsu * (cb - dcomp) / cb;
                return (0.85 * Conc.Fcu * B * a_balance * (DepthT - 0.5 * a_balance) + NumRebarC * CompRebar.As * eps_comp * (DepthT - (H - DepthC)))/N_balance;
            }
        }
        public double NumRebarT
        {
            set;
            get;
        }
        public double NumRebarC
        {
            set;
            get;
        }
        public double NumRebarV
        {
            set;
            get;
        }
        public Reinforcement CompRebar;
        public Reinforcement TensRebar;
        public Reinforcement VertRebar;
        public Concrete Conc;
        public double RhoT
        {
            get
            {
                return NumRebarT * TensRebar.As / (B * DepthT);
            }
        }
        public double RhoC
        {
            get
            {
                return NumRebarC * CompRebar.As / (B * DepthC);
            }
        }
        public double RhoSv
        {
            get
            {
                return NumRebarV * VertRebar.As / (Sv*B);
            }
        }


        // 构造函数
        public DoubleReinforcedRectSection()
        {
            B = 0;
            H = 0;            
            Sv = 0;
            DepthT = 0.0;
            DepthC = 0.0;
            NumRebarT = 0;
            NumRebarC = 0;
            NumRebarV = 0;
            CompRebar = new Reinforcement();
            TensRebar = new Reinforcement();
            VertRebar = new Reinforcement();
            Conc = new Concrete();
        }

        /// <summary>
        /// 配置双筋截面：一般
        /// </summary>
        /// <param name="b"></param>
        /// <param name="h"></param>
        /// <param name="fcu">混凝土标号</param>
        /// <param name="ftd">混凝土设计拉应力</param>
        /// <param name="dt">受拉钢筋型心力臂</param>
        /// <param name="nst">受拉钢筋根数</param>
        /// <param name="diat">受拉钢筋直径</param>
        /// <param name="fyt">受拉钢筋屈服强度</param>
        /// <param name="dc">受压钢筋型心力臂</param>
        /// <param name="nsc">受压钢筋根数</param>
        /// <param name="diac">受压钢筋直径</param>
        /// <param name="fyc">受压钢筋屈服强度</param>
        /// <param name="sv">箍筋间距</param>
        /// <param name="nsv">箍筋根数</param>
        /// <param name="diav">箍筋直径</param>
        /// <param name="fyv">箍筋屈服强度</param>
        public DoubleReinforcedRectSection(
            double b,double h, double fcu,
            double dt, double nst, double diat, double fskt,
            double dc, double nsc,double diac, double fskc,
            double sv,  double nsv, double diav, double fskv)
        {
            B = b;
            H = h;            
            Sv = sv;
            DepthT = dt;
            DepthC = dc;
            NumRebarT = nst;
            NumRebarC = nsc;
            NumRebarV = nsv;
            CompRebar = new Reinforcement(fskc,diac);
            TensRebar = new Reinforcement(fskt,diat);
            VertRebar = new Reinforcement(fskv,diav);
            Conc = new Concrete(fcu);
        }


        /// <summary>
        /// 配置双筋截面：对称
        /// </summary>
        /// <param name="b"></param>
        /// <param name="h"></param>
        /// <param name="fcu">混凝土标号</param>
        /// <param name="ftd">混凝土设计拉应力</param>
        /// <param name="dt">受拉钢筋型心力臂</param>
        /// <param name="nst">受拉钢筋根数</param>
        /// <param name="diat">受拉钢筋直径</param>
        /// <param name="fyt">受拉钢筋屈服强度</param>
        /// <param name="sv">箍筋间距</param>
        /// <param name="nsv">箍筋根数</param>
        /// <param name="diav">箍筋直径</param>
        /// <param name="fyv">箍筋屈服强度</param>
        public DoubleReinforcedRectSection(
            double b, double h, double fcu,
            double dt, double nst, double diat, double fsk,            
            double sv, double nsv, double diav, double fskv)
        {
            B = b;
            H = h;
            Sv = sv;
            DepthT = dt;
            DepthC = dt;
            NumRebarT = nst;
            NumRebarC = nst;
            NumRebarV = nsv;
            CompRebar = new Reinforcement(fsk,diat);
            TensRebar = new Reinforcement(fsk,diat);
            VertRebar = new Reinforcement(fskv,diav);
            Conc = new Concrete(fcu);
        }



        List<double> getErr(double a0,double a1,double Nn=0.0)
        {
            List<double> res = new List<double>();
            List<double> inputs = new List<double>() { a0, a1 };
            double AsC = NumRebarC * CompRebar.As;
            double AsT = NumRebarT * TensRebar.As;
            double a_ret, err, fs=0.0;
            double c, epsComp;

            foreach (var item in inputs)
            {
                c = item / Conc.Beta1;
                epsComp = -Conc.epsu * (c - (H - DepthC)) / c;

                if (epsComp <= -CompRebar.epsy)
                {
                    fs = -CompRebar.Fy;
                }
                else if (epsComp < CompRebar.epsy)
                {
                    fs = CompRebar.Es * epsComp;
                }
                else
                {
                    fs = CompRebar.Fy;
                }

                a_ret = (AsT * TensRebar.Fy + AsC * fs-Nn) / (0.85 * Conc.Fcu * B);
                err = item - a_ret;
                res.Add(err);

            }
            res.Add(fs);

            return res;
        }

        Tuple<double,double> GetFs(double a0,double a1,double nn=0, double error = 1e-6)
        {
            List<double> testA = new List<double>();
            List<double> testB = new List<double>();
            double amid;
            double fs = 0.0;            
            amid = (a0 + a1) * 0.5;
            for (int i = 0; i < 50; i++)
            {
                testA = getErr(a0, a1,nn);
                testB = getErr(a0, amid,nn);
                if (testA[0] * testA[1] >= 0)
                {
                    break;
                }
                else if (testB[0] * testB[1] >= 0)
                {
                    a0 = amid;
                    amid = (a0 + a1) * 0.5;
                }
                else
                {
                    a1 = amid;
                    amid = (a0 + a1) * 0.5;
                }

                if (Math.Abs(testA[0]) <= error)
                {
                    fs = (testA[2] + testB[2]) * 0.5;
                    Console.WriteLine(string.Format("#  {0}次迭代，计算收敛。", i));
                    break;
                }
            }
            return new Tuple<double, double>(fs,amid);

        }




        /// <summary>
        /// 计算抗弯承载力：纯弯曲
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public double CalculateMu(double error=1e-6)
        {
            Console.WriteLine(string.Format("#--------------------------------------------------------#"));

            List<double> testA = new List<double>();
            List<double> testB = new List<double>();
            double a0=1, a1=H, amid,dd;
            double fs=0.0;
            double Mu = 0.0;
            amid = (a0 + a1) * 0.5;
            for (int i = 0; i < 50; i++)
            {
                testA = getErr(a0,a1);
                testB = getErr(a0, amid);
                if (testA[0] * testA[1] >= 0)
                {
                    break;
                }
                else if(testB[0]*testB[1]>=0)
                {
                    a0 = amid;
                    amid = (a0 + a1) * 0.5;
                }
                else
                {
                    a1 = amid;
                    amid= (a0 + a1) * 0.5;
                }

                if (Math.Abs(testA[0]) <= error)
                {
                    fs = (testA[2] + testB[2]) * 0.5;
                    Console.WriteLine(string.Format("#  {0}次迭代，计算收敛。", i));
                    break;
                }
            }

            dd = (H - DepthC);
            Mu = 0.85 * Conc.Fcu * B * amid * (DepthT - 0.5 * amid) - CompRebar.As * fs * (DepthT - dd);

            Console.WriteLine(string.Format("#  a={0:F3}mm", amid));
            Console.WriteLine(string.Format("#  fs={0:F3}MPa", fs));
            Console.WriteLine(string.Format("#  Mu={0:F3}kNm", Mu/1000000));


            Console.WriteLine(string.Format("#--------------------------------------------------------#"));

            return Mu;
        }



        public void ShearRef()
        {            

            Console.WriteLine(string.Format("#  截面参考剪力Vu={0}kN",0.5*1.0*Conc.Ftd*B*DepthT*1.0e-3));
        }

        /// <summary>
        /// 截面分析
        /// </summary>
        /// <param name="Nn">初始轴压，拉力为正值</param>
        /// <param name="isConstantNn">是否轴压恒定</param>
        public void SectionAnalysis(double Nn=0,double Mn=0,bool isConstantNn=true)
        {
            Console.WriteLine(string.Format("#--------------------------------------------------------#"));

            Console.WriteLine(string.Format("#  截面平衡条件:(a={0:F1}mm,N=-{1:F0}kN,e={2:F1}mm)", a_balance, N_balance / 1000.0, e_balance));
            var ss = "偏心距";
            if (isConstantNn)
            {
                ss = "轴力";
            }            
            Console.WriteLine(string.Format("#  初始轴力Nn={0:F0}kN,初始弯矩Mn={1:F0}kNm,{2}恒定。", Nn/1.0e3,Mn/1.0e6,ss));
            
            var AsTens = TensRebar.As * NumRebarT;
            var AsComp = CompRebar.As * NumRebarC;

            if (isConstantNn)
            {//常轴力问题
                if( Nn >= -N_balance)
                {
                    //受拉破坏
                    Console.WriteLine(string.Format("#  截面为<常轴力弯曲破坏>"));
                    Tuple<double,double> fsa=GetFs(1,H,Nn);
                    var Mu = 0.85 * Conc.Fcu * B * fsa.Item2 * (DepthT - 0.5 * fsa.Item2) - CompRebar.As * fsa.Item1 * (DepthT - (H-DepthC));
                    Console.WriteLine(string.Format("#  a={0:F3}mm", fsa.Item2));
                    Console.WriteLine(string.Format("#  fs={0:F3}MPa", fsa.Item1));
                    Console.WriteLine(string.Format("#  极限承载力Mu={1:F1}kNm",fsa.Item1,Mu/1e6));



                }
                else
                {
                    //受压破坏
                    Console.WriteLine(string.Format("#  截面为<常轴力受压破坏>"));
                }


            }
            else
            {//常偏心问题
                if ((Nn == 0) || (Mn / Nn > -e_balance))
                {
                    //受拉破坏
                    Console.WriteLine(string.Format("#  截面为<大偏心受拉破坏>"));
                }
                else
                {
                    //受压破坏
                    Console.WriteLine(string.Format("#  截面为<小偏心受拉破坏>"));
                }
            }


            Console.WriteLine(string.Format("#--------------------------------------------------------#"));

            return;
        }


        public void ShearAnalysis(double a1=0.9,double a2=1.0)
        {
            Console.WriteLine(string.Format("#--------------------------------------------------------#"));
            double P = 100 * RhoT;
            if (P > 2.5) { P = 2.5; }
            Console.WriteLine(string.Format("#  斜截面配筋率Ps={0:F6}%,斜截面配箍率Psv={1:F6}%",RhoT,RhoSv));
            
            double Vcs = a1 * a2 * 1.1 * 0.45e-3 * B * DepthT * Math.Sqrt((2 + 0.6 * P) * Math.Sqrt(Conc.Fcu) * RhoSv * VertRebar.Fy);

            Console.WriteLine(string.Format("#  斜面参考剪力Vu={0:F3}kN,斜截面设计剪力Vcs={1:F3}kN", 0.5 * 1.0 * Conc.Ftd * B * DepthT * 1.0e-3,Vcs));
            Console.WriteLine(string.Format("#--------------------------------------------------------#"));
        }






    }
}
