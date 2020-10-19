using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedTheoryConcreteStructure.Public;

namespace UnifiedTheoryConcreteStructure.Material
{
    public class Prestress
    {
        public Prestress(string n,double fpu)
        {
            Name = n;
            Fpu = fpu;
            Diameter =11.2;
        }
        public string Name;
        public Specification Spec;
        public double Fpu;
        public double Diameter;

        public double Area { get { return Math.PI * Diameter * Diameter * 0.25; } }

        public double Fy { get { return 1674; } }

        public double Ep { get { return 197000; } }
        public double Epsy { get { return Fy / Ep; } }

        public double GetSigma(double eps)
        {
            if (Math.Abs(eps) <= Epsy)
            {
                return eps * Ep;
            }
            else
            {
                return Fy;
            }
        }

    }
}
