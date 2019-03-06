using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedTheoryConcreteStructure
{
    class Reinforcement
    {
        public string Cname
        {
            set;
            get;
        }
        public double Dia
        {
            set;
            get;
        }
        public double Fy
        {
            get
            {
                return Globals.RebarParas[Cname][1];
            }
        }
        public double As
        {
            get
            {
                return Math.Pow(Dia,2.0) * 0.25 * Math.PI;
            }
        }

        public double Es
        {
            get
            {
                return Globals.RebarParas[Cname][2];
            }
        }
        public double epsy
        {
            get
            {
                return Fy / Es;
            }
        }
        public double Fsk
        {
            get
            {
                return Globals.RebarParas[Cname][0];
            }
        }

        public Reinforcement()
        {
            Cname = "";
            Dia = 0;
                  
        }

        /// <summary>
        /// 一根钢筋
        /// </summary>
        /// <param name="d">直径</param>        
        public Reinforcement(string name,double d=25.0)
        {
            Cname = name;
            Dia = d;            
        }
        public Reinforcement(double fsk,double d=25.0)
        {
            Cname = string.Format("HRB{0}", (int)fsk);
            Dia = d;
        }


        public double sigma(double eps)
        {
            double epsy = Fy / Es;
            
            if (Math.Abs(eps)<epsy)
            {
                return eps*Es;
            }
            else if(eps>0)
            {
                return Fy;
            }
            else
            {
                return -Fy;
            }
        }


    }
}
