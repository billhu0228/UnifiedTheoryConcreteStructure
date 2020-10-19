using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedTheoryConcreteStructure.Public
{
    public enum SpecName {None,JTG,BS5400,LRFD2007,EN1992, D60V2004 }
    public enum ChamferType { Round,Line}

    public class Specification
    {
        SpecName name;

        public SpecName Name { get => name; set => name = value; }
    }

    public static class GlobalDef
    {

        public static void Gen(this DataTable dt, int rows, string[] columns)
        {
            foreach (string item in columns)
            {
                dt.Columns.Add(item, Type.GetType("System.Double"));
            }

            for (int i = 0; i < rows; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }

        public static void WriteLine(this DataTable dt, int rowidx, double[] content)
        {
            int i = 0;
            foreach (double item in content)
            {
                dt.Rows[rowidx][i] = item;
                i++;
            }

        }
    }
}
