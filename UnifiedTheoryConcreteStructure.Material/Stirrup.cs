using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedTheoryConcreteStructure.Public;

namespace UnifiedTheoryConcreteStructure.Material
{
    public class Stirrup
    {
        public string Name;
        public SpecName Spec;
        public double Fy;
        public double Sv;
        public int Num;
        public double ds;

        public Stirrup(string name, double fy, double ds, int ns, double ss, SpecName spec = SpecName.D60V2004)
        {
            Fy = fy;
            Name = name;
            Num = ns;
            this.ds = ds;
            Sv = ss;            
            Spec = spec;
        }
    }
}
