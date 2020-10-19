using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedTheoryConcreteStructure.Material;

namespace UnifiedTheoryConcreteStructure.Section
{
    public class BaseSection
    {
        public double H, B;
        public double Cc;
        internal List<SingleRebar> Rebars;
        internal List<Point2D> OuterPts;
        internal List<Line2D> OuterLns;
        public int NumRebars;
        public double Ds;
        public Reinforcement RebarProperty;
        public ConcreteAASHTO ConcProperty;
        public Point2D PlasticCenter;
        public virtual double Ag { get; }
        public virtual double Astotal { get; }

        public virtual double P0() { return 0.0; }
        public virtual double GetMnX(double Nn) { return 0; }
        public virtual double GetMnY(double Nn) { return 0; }

        public virtual double GetPnX(double e0) { return 0; }
        public virtual double GetPnY(double e0) { return 0; }
    }
}
