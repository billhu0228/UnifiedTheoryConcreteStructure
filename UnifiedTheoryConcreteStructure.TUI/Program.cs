using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedTheoryConcreteStructure.Material;
using UnifiedTheoryConcreteStructure.Section;
namespace UnifiedTheoryConcreteStructure.TUI
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ConcreteAASHTO C50 = new ConcreteAASHTO("C50", 50);
            Reinforcement Grade400 = new Reinforcement("", 400, 20, 6, 50 +20, 0);
            RectSection ptm2 = new RectSection("f",1000,1000);
            ptm2.thisConcrete = C50;
            ptm2.RebarY0 = Grade400;
            //    RoundRecSection cpm1 = new RoundRecSection(2500, 1100, C35, Grade500, 64, 32, 50 + 32, 25);
            //    RoundRecSection pme2main = new RoundRecSection(2500, 1000, C35, Grade500, 40, 32, 50 + 32, 25);
            //    RoundRecSection pme2ramp = new RoundRecSection(1400,1000, C35, Grade500, 32, 32, 50 + 32, 250);
            //    RoundRecSection pe14 = new RoundRecSection(1600, 900, C35, Grade500, 28, 32, 50 + 32, 225);

            //    RoundRecSection ptl1 = new RoundRecSection(1000, 1400, C35, Grade500, 24, 32, 50 + 32, 225);


            //    Column PM1 = new Column(25000, cpm1, cpm1);
            //    PM1.SetDesignLoad(15295e3, 1281e6, 9963e6, 15295e3, 1281e6, 9963e6);
            //    PM1.PrintReport();

            ptm2.SectionAnalysis();

            Console.WriteLine("分析完成.");
            Console.ReadKey();
        }
    }
}
