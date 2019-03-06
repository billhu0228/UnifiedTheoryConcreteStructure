using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedTheoryConcreteStructure
{



    class Concrete
    {
        public string Cname
        {
            get;

            set;
        }

        public double Fcu
        {
            set;
            get;
        }
        public double Fck
        {
            get
            {
                return Globals.ConcParas[Cname][0];
            }
        }
        public double Ftk
        {
            get
            {
                return Globals.ConcParas[Cname][1];
            }
        }
        public double Fcd
        {
            get
            {
                return Globals.ConcParas[Cname][2];
            }
        }
        public double Ftd
        {
            get
            {
                return Globals.ConcParas[Cname][3];
            }
        }
        public double Ec
        {
            get
            {
                return Globals.ConcParas[Cname][4];
            }
        }


        public double Beta1
        {

            get
            {
                double res = 0.0;
                if (Fcu == 0)
                {
                    res = 0.0;
                }else if (Fcu <= 27.6)
                {
                    res = 0.85;
                }
                else if (Fcu <= 55.2)
                {
                    res = 0.85 - (Fcu - 27.6) / (55.2 - 27.6) * 0.2;
                }
                else
                {
                    res = 0.65;
                }
                return res;
            }
        }
        public double epsu
        {
            get
            {
                return 0.0033;
            }
        }

        public double eps0
        {
            get
            {
                return 0.002;
            }
        }
        public int n
        {
            get
            {
                return 2;
            }
        }


        public Concrete()
        {
            Fcu = 0;
            Cname = "";
        }
        public Concrete(double fcu)
        {
            Fcu = fcu;
            Cname = string.Format("C{0}", (int)fcu);

        }
        public Concrete(string cname)
        {
            Cname = cname;
            Fcu = double.Parse(cname.Substring(1));
        }


        public double sigma01(double eps,double fpcu)
        {   
            if (eps >= 0)
            {
                return 0.0;
            }
            else if (eps>=-eps0)
            {
                double tmp = eps / -eps0;
                return -Fcd * (2.0 * tmp - tmp * tmp);
            }
            else if (eps >= -epsu)
            {
                double kk = (fpcu + Fcd) / (-epsu + eps0);
                return (eps + eps0) *kk - Fcd;
            }
            else
            {
                return fpcu;
            }
            
        }
        public double sigma02(double eps)
        {
            if (eps >= 0)
            {
                return 0.0;
            }
            else if (eps >= -epsu)
            {
                double tmp = eps / -eps0;
                return -Fcd * (2.0 * tmp - tmp * tmp);
            }
            else
            {

                double tmp = -epsu / -eps0;
                return -Fcd * (2.0 * tmp - tmp * tmp);
            }

        }
    }
}
