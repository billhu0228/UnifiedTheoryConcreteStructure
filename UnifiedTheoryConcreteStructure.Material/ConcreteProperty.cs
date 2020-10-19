using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedTheoryConcreteStructure.Material
{

    public class ConcreteProperty
    {
        #region 构造函数
        public ConcreteProperty()
        {
        }
        public ConcreteProperty(string v)
        {
            _name = v;
        }
        #endregion

        #region 字段
        string _name;
        //double _fck;
        //double _ftk;
        //double _fcd;
        //double _ftd;
        //double _ec;
        //private string v;
        #endregion

        #region 静态属性
        static Dictionary<string, double> EcDic 
        {
            get
            {
                Dictionary<string, double> Dic = new Dictionary<string, double>();
                Dic.Add("JTG-C30", 3.00e4);
                Dic.Add("JTG-C35", 3.15e4);
                Dic.Add("JTG-C40", 3.25e4);
                Dic.Add("JTG-C50", 3.45e4);
                return Dic;
            }
        }
        static Dictionary<string, double> FckDic
        {
            get
            {
                Dictionary<string, double> Dic = new Dictionary<string, double>();
                Dic.Add("JTG-C30", 20.1);
                Dic.Add("JTG-C35", 23.4);
                Dic.Add("JTG-C40", 26.8);
                Dic.Add("JTG-C50", 32.4);
                return Dic;
            }

        }
        static Dictionary<string, double> Fck_cylinderDic
        {
            get
            {
                Dictionary<string, double> Dic = new Dictionary<string, double>();
                Dic.Add("AASHTO-Fc45", 45);
                Dic.Add("AASHTO-Fc35", 35);
                Dic.Add("AASHTO-Fc30", 30);
                return Dic;
            }

        }


        static Dictionary<string, double> FcdDic 
        {
            get
            {
                Dictionary<string, double> Dic = new Dictionary<string, double>();
                Dic.Add("JTG-C30", 13.8);
                Dic.Add("JTG-C35", 16.1);
                Dic.Add("JTG-C40", 18.4);
                Dic.Add("JTG-C50", 22.4);
                return Dic;
            }
        }
        static Dictionary<string, double> FtkDic 
        {
            get
            {
                Dictionary<string, double> Dic = new Dictionary<string, double>();
                Dic.Add("JTG-C30", 2.01);
                Dic.Add("JTG-C35", 2.20);
                Dic.Add("JTG-C40", 2.40);
                Dic.Add("JTG-C50", 2.65);
                return Dic;
            }
        }
        static Dictionary<string, double> FtdDic 
        {
            get
            {
                Dictionary<string, double> Dic = new Dictionary<string, double>();
                Dic.Add("JTG-C30", 1.39);
                Dic.Add("JTG-C35", 1.52);
                Dic.Add("JTG-C40", 1.65);
                Dic.Add("JTG-C50", 1.83);
                return Dic;
            }

        }
        //public override List<string> UsedConcreteList
        //{
        //    get
        //    {
        //        return (from a in EcDic select a.Key).ToList();
        //    }
        //}
        #endregion

        #region 属性
        public string Name { get => _name; set => _name = value; }

        public double Fck_cylinder
        {
            get
            {
                try
                {
                    return Fck_cylinderDic[_name];
                }
                catch (Exception)
                {
                    return double.NaN;
                }

            }
        }

        public double Fck
        {
            get
            {
                try
                {
                    return FckDic[_name];
                }
                catch (Exception)
                {
                    return double.NaN;
                }

            }
        }
        //   public double Ftk { get => FtkDic[_name]; }
        double Fcd 
        {
            get
            {
                try
                {
                    return FcdDic[_name];
                }
                catch (Exception)
                {

                    return double.NaN;
                }
             
            }
        }
     //   public double Ftd { get => FtdDic[_name]; }
        public  double Ec 
        {
            get 
            {
                try
                {
                    return EcDic[_name]; 
                }
                catch (Exception)
                {

                    return 4800 * Math.Sqrt(Fck_cylinder);
                }
               
            }
        }




        public double Epsu
        {
            get
            {
                if (_name.StartsWith("JTG"))
                {
                    return Math.Min(0.0033-(Fck - 50)*1e-5,0.0033);
                }
                else
                {
                    return 0.003;
                }
                
            }
        }
        double Eps0
        {
            get
            {
                return Math.Max(0.002 + 0.5 * (Fck - 50) * 1e-5,0.002);
            }
        }
        double PowerN
        {
            get
            {
                return Math.Min(2 - (Fck - 50) / 60.0,2);
            }
        }

        


      //  public static List<string> UsedConcretPropertyList => (from a in EcDic select a.Key).ToList();


        #endregion

        #region 方法
        public double GetSigma(double eps)
        {
            if (_name.StartsWith("JTG"))
            {
               return GetSigmaJTG(eps);
            }
            else
            {
               return GetSigmaAASHTO(eps);
            }
           
        }

        double GetSigmaJTG(double eps)
        {
            if (eps >= 0)
            {
                return 0.0;
            }
            else if (eps >= -Eps0)
            {
                return -1.0 * Fcd * (1 - Math.Pow(1 - Math.Abs(eps) / Eps0, PowerN));
            }
            else if (eps >= -Epsu)
            {
                return -Fcd;
            }
            else
            {
                return 0.0;
            }
        }

        double GetSigmaAASHTO(double eps)
        {
            if (eps >= 0)
            {
                return 0.0;
            }
            else if (eps >= -Epsu)
            {
                return -(2 * (eps / -0.002) - (eps / -0.002) * (eps / -0.002)) * Fck_cylinder;
            }
            else
            {
                return 0.0;
            }
        }

        #endregion

    }
}
