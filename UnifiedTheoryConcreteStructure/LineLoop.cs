using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCAD;

namespace UnifiedTheoryConcreteStructure
{
    class LineLoop
    {
        public List<AcadLine> theLines;
        
        public LineLoop(List<AcadLine> inputs)
        {
            foreach (AcadLine item in inputs)
            {
                theLines.Add(item);
            }
        }

        static public LineLoop SearchLineLoop(AcadSelectionSet acadSelSet)
        {
            LineLoop ret = null ;
            List<AcadLine> allLines=new List<AcadLine>();
            foreach (var item in acadSelSet)
            {
                try
                {
                    AcadLine L = (AcadLine)item;
                    allLines.Add(L);
                }
                catch (Exception)
                {
                    continue;                    
                }
            }



            



            return ret;
        }





    }
}
