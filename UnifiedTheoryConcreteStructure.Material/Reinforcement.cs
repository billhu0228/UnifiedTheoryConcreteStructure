using System;
using UnifiedTheoryConcreteStructure.Public;

namespace UnifiedTheoryConcreteStructure.Material
{
    public class Reinforcement
    {

        public string Name;
        public SpecName Spec;
        public double Fy;
        public double locY, locZ;
        public int Num;
        public double ds;
        private string v;

        public Reinforcement(string v)
        {
            Name = v;
            Fy = double.Parse(v.Substring(v.Length - 3, 3));
            Num = 1;
            this.ds = 32;
            this.locY = 0;
            this.locZ = 0;
            Spec = SpecName.D60V2004;
        }

        public Reinforcement(string name, double fy,double ds,int ns, double locY, double locZ, SpecName spec = SpecName.D60V2004)
        {
            Fy = fy;
            Name = name;
            Num = ns;
            this.ds = ds;
            this.locY = locY;
            this.locZ = locZ;
            Spec = spec;
        }

        public double AsTotal { get { return Math.PI * ds * ds * 0.25 * Num; } }
        public double Es { get { return 200000; } }
        public double Epsy { get { return Fy / Es; } }

        public double GetSigma(double eps)
        {
            if (Math.Abs(eps)<=Epsy)
            {
                return eps * Es;
            }
            else
            {
                return Fy;
            }
        }
    }
}
