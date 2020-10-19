using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedTheoryConcreteStructure.Material
{
    public class SingleRebar
    {
        public Point2D Location;
        public double Dia;
        public double Fy;
        public double Es;

        public SingleRebar(Point2D location, double dia, double fy, double es=200000)
        {
            Location = location;
            Dia = dia;
            Fy = fy;
            Es = es;
        }

        public double As { get { return Math.PI * 0.25 * Dia * Dia; } }
    }
}
