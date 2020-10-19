using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedTheoryConcreteStructure.Material;

namespace UnifiedTheoryConcreteStructure.Section
{
    public static class PubFuncs
    {

        static List<double> GetErrTensA2(double a0, double a1, double B, double H, double DepthT,double DepthC, ConcreteAASHTO Conc, Reinforcement TensRebar, Reinforcement CompRebar, double e)
        {
            List<double> res = new List<double>();
            List<double> inputs = new List<double>() { a0, a1 };
            double AsC = CompRebar.AsTotal;
            double AsT = TensRebar.AsTotal;
            double a_ret, err, fs = 0.0;
            double c, epsTens;

            foreach (var item in inputs)
            {
                c = item / Conc.Beta1;
                epsTens = Conc.Epsu * (DepthT - c) / c;

                if (epsTens <= -TensRebar.Epsy)
                {
                    fs = -CompRebar.Fy;
                }
                else if (epsTens < TensRebar.Epsy)
                {
                    fs = TensRebar.Es * epsTens;
                }
                else
                {
                    fs = TensRebar.Fy;
                }
                double Nn = ((0.85 * Conc.Fck_cylinder * B * item) * (DepthT - 0.5 * item) + AsC * CompRebar.Fy * (DepthT - (H - DepthC)))/e;
                a_ret = (Nn + AsT * fs - AsC * CompRebar.Fy) / (0.85 * Conc.Fck_cylinder * B);
                err = item - a_ret;
                res.Add(err);

            }
            res.Add(fs);

            return res;
        }



        static List<double>GetErrTensA(double a0, double a1, double B, double H, double DepthT, ConcreteAASHTO Conc, Reinforcement TensRebar, Reinforcement CompRebar, double Nn = 0.0)
        {
            List<double> res = new List<double>();
            List<double> inputs = new List<double>() { a0, a1 };
            double AsC = CompRebar.AsTotal;
            double AsT = TensRebar.AsTotal;
            double a_ret, err, fs = 0.0;
            double c, epsTens;

            foreach (var item in inputs)
            {
                c = item / Conc.Beta1;
                epsTens = Conc.Epsu * (DepthT-c) / c;

                if (epsTens <= -TensRebar.Epsy)
                {
                    fs = -CompRebar.Fy;
                }
                else if (epsTens < TensRebar.Epsy)
                {
                    fs = TensRebar.Es * epsTens;
                }
                else
                {
                    fs = TensRebar.Fy;
                }

                a_ret = (AsT *fs - AsC * CompRebar.Fy - Nn) / (0.85 * Conc.Fck_cylinder * B);
                err = item - a_ret;
                res.Add(err);

            }
            res.Add(fs);

            return res;
        }

        static List<double> GetErrCompA(double a0, double a1,double B,double H,double DepthC,ConcreteAASHTO Conc,Reinforcement TensRebar, Reinforcement CompRebar,  double Nn = 0.0)
        {
            List<double> res = new List<double>();
            List<double> inputs = new List<double>() { a0, a1 };
            double AsC = CompRebar.AsTotal;
            double AsT = TensRebar.AsTotal;
            double a_ret, err, fs = 0.0;
            double c, epsComp;

            foreach (var item in inputs)
            {
                c = item / Conc.Beta1;
                epsComp = -Conc.Epsu * (c - (H - DepthC)) / c;

                if (epsComp <= -CompRebar.Epsy)
                {
                    fs = -CompRebar.Fy;
                }
                else if (epsComp < CompRebar.Epsy)
                {
                    fs = CompRebar.Es * epsComp;
                }
                else
                {
                    fs = CompRebar.Fy;
                }

                a_ret = (AsT * TensRebar.Fy + AsC * fs - Nn) / (0.85 * Conc.Fck_cylinder * B);
                err = item - a_ret;
                res.Add(err);

            }
            res.Add(fs);

            return res;
        }

        public static Tuple<double, double> GetCompRebarFs(double a0, double a1, double B, double H, double DepthC, ConcreteAASHTO Conc, Reinforcement TensRebar, Reinforcement CompRebar,  double Nn = 0, double error = 1e-6)
        {
            List<double> testA = new List<double>();
            List<double> testB = new List<double>();
            double amid;
            double fs = 0.0;
            amid = (a0 + a1) * 0.5;
            for (int i = 0; i < 50; i++)
            {
                testA = GetErrCompA(a0, a1, B, H, DepthC, Conc, TensRebar, CompRebar, Nn);
                testB = GetErrCompA(a0, amid, B, H, DepthC, Conc, TensRebar, CompRebar, Nn);
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
                    //Console.WriteLine(string.Format("#  {0}次迭代，计算收敛。", i));
                    break;
                }
            }
            return new Tuple<double, double>(fs, amid);

        }
        /// <summary>
        /// 返回ft,fc,a,failmode;
        /// </summary>
        /// <param name="Nn"></param>
        /// <param name="a_blance"></param>
        /// <param name="B"></param>
        /// <param name="H"></param>
        /// <param name="Conc"></param>
        /// <param name="TensRebar"></param>
        /// <param name="CompRebar"></param>
        /// <param name="depthT"></param>
        /// <param name="DepthC"></param>
        /// <param name="error"></param>
        /// <returns>0-纯弯曲；1-受拉破坏；2-平衡破坏；3-受压且c<h4-受压且c>h;5-纯压</returns>
        internal static double[] GetFFA(double Nn, double a_blance,double B, double H,ConcreteAASHTO Conc, Reinforcement TensRebar, Reinforcement CompRebar, double depthT, double DepthC, double error=1e-6)
        {
            double St = 0;
            double Sc = 0;
            double a = 0;
            double Fail=-1;//0-纯弯曲；1-受拉破坏；2-平衡破坏；3-受压且c<h;4-受压且c>h;5-纯压
            bool isConv = false;

            // 按受拉破坏            
            double a0 = 1;
            double a1 = a_blance;
            List<double> testA = new List<double>();
            List<double> testB = new List<double>();
            double amid=(a0 +a1) *0.5;
            double fs = 0.0;
            
            for (int i = 0; i < 50; i++)
            {
                testA = GetErrCompA(a0, a1, B, H, DepthC, Conc, TensRebar, CompRebar, Nn);
                testB = GetErrCompA(a0, amid, B, H, DepthC, Conc, TensRebar, CompRebar, Nn);
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
                    isConv = true;
                    //Console.WriteLine(string.Format("#  {0}次迭代，计算收敛。", i));
                    break;
                }
            }
            if (isConv)
            {
                St = TensRebar.Fy;
                Sc = fs;
                a = amid;
                Fail = 1;
                return new double[] { St, Sc, a, Fail };
            }


            // 按受压破坏
            a0 = a_blance;
            a1 = H*Conc.Beta1;
            testA = new List<double>();
            testB = new List<double>();
            amid = (a0 + a1) * 0.5;
            fs = 0.0;
            for (int i = 0; i < 50; i++)
            {
                testA = GetErrTensA(a0, a1, B, H, DepthC, Conc, TensRebar, CompRebar, Nn);
                testB = GetErrTensA(a0, amid, B, H, DepthC, Conc, TensRebar, CompRebar, Nn);
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
                    isConv = true;
                    //Console.WriteLine(string.Format("#  {0}次迭代，计算收敛。", i));
                    break;
                }
            }
            if (isConv)
            {
                St = fs;
                Sc = -CompRebar.Fy;
                a = amid;
                Fail = 3;
                return new double[] { St, Sc, a, Fail };
            }



            return new double[] { St, Sc, a, Fail };
        }

        public static Tuple<double, double> GetTensRebarFs(double a0, double a1, double B, double H, double DepthT, double DepthC,ConcreteAASHTO Conc, Reinforcement TensRebar, Reinforcement CompRebar,double e, double error = 1e-6)
        {
            List<double> testA = new List<double>();
            List<double> testB = new List<double>();
            double amid;
            double fs = 0.0;
            amid = (a0 + a1) * 0.5;
            for (int i = 0; i < 50; i++)
            {
                testA = GetErrTensA2(a0, a1, B, H, DepthT, DepthC, Conc, TensRebar, CompRebar, e);
                testB = GetErrTensA2(a0, amid, B, H, DepthT, DepthC, Conc, TensRebar, CompRebar, e);
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
                    //Console.WriteLine(string.Format("#  {0}次迭代，计算收敛。", i));
                    break;
                }
            }
            return new Tuple<double, double>(fs, amid);

        }

    }
}
