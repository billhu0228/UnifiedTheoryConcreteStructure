using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedTheoryConcreteStructure
{
    class CPolyLine
    {
        public List<Point> Verts;
        public double Area;

        public CPolyLine(List<Point> pts)
        {
            Verts = new List<Point>();
            foreach (Point item in pts)
            {
                Verts.Add(item);
            }           
        }

        public void Add(Point pt)
        {
            Verts.Add(pt);
        }



    }
}
