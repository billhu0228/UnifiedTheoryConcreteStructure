using System;
using UnifiedTheoryConcreteStructure.Public;

namespace UnifiedTheoryConcreteStructure.Material
{


    public class ConcreteAASHTO
    {

        public string Name;
        public SpecName Spec;
        public double Fck_cube, Fck_cylinder;
        


        public ConcreteAASHTO(string name)
        {
            Name = name;
            double value = double.Parse(name.Substring(1, 2));
            Fck_cube = value;
            Fck_cylinder = 0;         
            
        }

        /// <summary>
        /// 创建混凝土材料
        /// </summary>
        /// <param name="value">立方体抗压强度（国标）或圆柱体抗压强度（其他）(MPa)</param>        
        /// <param name="name">材料名称</param>
        /// <param name="spec">规范体系</param>
        public ConcreteAASHTO(string name, double value, SpecName spec = SpecName.D60V2004)
        {
            Name = name;
            if (spec== SpecName.D60V2004)
            {
                Fck_cube = value;
                Fck_cylinder = 0;
            }
            else
            {
                Fck_cylinder = value;
                Fck_cube = 0;
            }            
            Spec = spec;
        }
        public double Ec
        {
            get
            {
                return 4800 * Math.Sqrt(Fck_cylinder);
            }
            set
            {
                Ec = value;
            }
        }
        public double Epsu
        {
            get
            {
                return 0.003;
            }
            set
            {
                Epsu = value;   
            }
        }

        public double Beta1
        {
            get
            {
                double res = 0.0;
                if (Fck_cylinder == 0)
                {
                    res = 0.0;
                }
                else if (Fck_cylinder <= 28)
                {
                    res = 0.85;
                }               
                else
                {
                    res = 0.85-0.05*(Fck_cylinder-28)/7.0;
                }
                if (res<0.65)
                {
                    res = 0.65;
                }
                return res;
            }
        }

        public double GetSigma(double eps)
        {
            if (eps>=0)
            {
                return 0.0;
            }
            else if (eps>=-Epsu)
            {
                return -(2 * (eps / -0.002) - (eps / -0.002) * (eps / -0.002)) * Fck_cylinder;
            }
            else
            {
                return 0.0;
            }
        }
    }
}
