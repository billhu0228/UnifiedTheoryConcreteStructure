using UnifiedTheoryConcreteStructure.Public;
using UnifiedTheoryConcreteStructure.Material;
using UnifiedTheoryConcreteStructure.Section;
using UnifiedTheoryConcreteStructure.Member;
using System;

namespace UnifiedTheoryConcreteStructure.TUI
{
    public static class RunAASHTO
    {

        public static void Sub3()
        {

        }

        //public static void Sub1()
        //{
        //    Specification thisSpec = Specification.LRFD2007;
        //    Concrete C35 = new Concrete("C35", 35, thisSpec);           

        //    RectSection CPTM2Bot = new RectSection("CPTM2Bot", 1200, 1500, 25)
        //    {
        //        thisConcrete = C35,
        //        ChaType = ChamferType.Round,
        //        RebarY0 = new Reinforcement("", 500, 32, 2, 50 + 32, 0, thisSpec),
        //        RebarY1 = new Reinforcement("", 500, 32, 2, 1500 - 50 - 32, 0, thisSpec),
        //        RebarZ0 = new Reinforcement("", 500, 32, 10, 750, -600 + 50 + 32, thisSpec),
        //        RebarZ1 = new Reinforcement("", 500, 32, 10, 750, 600 - 50 - 32, thisSpec),
        //        VerticalRebarY = new Stirrup("", 500, 12, 6, 100, thisSpec),
        //        VerticalRebarZ = new Stirrup("", 500, 12, 6, 100, thisSpec),
        //    };





        //    //Column CPTM2 = new Column(25000, CPTM2Bot, CPTM2Bot);

        //    CPTM2.SetDesignLoad(7432e3, 636e6, 6921e6, 8689e3, 2106e6, 986e6);

        //    var resBuff = CPTM2.PrintReport();











        //    foreach (var item in resBuff)
        //    {
        //        Console.WriteLine(item);
        //    }


        //}

        //internal static void Sub2()
        //{
        //    Specification thisSpec = Specification.LRFD2007;
        //    ConcreteAASHTO C35 = new ConcreteAASHTO("C35", 35, thisSpec);
        //    Reinforcement Grade500 = new Reinforcement("", 500, 32, 46, 50 + 32, 0, thisSpec);
        //    RoundRecSection ptm2 = new RoundRecSection(1200, 1500, C35, Grade500, 46, 32, 50+32, 200);
        //    RoundRecSection cpm1 = new RoundRecSection(2500, 1100, C35, Grade500, 64, 32, 50 + 32, 25);
        //    RoundRecSection pme2main = new RoundRecSection(2500, 1000, C35, Grade500, 40, 32, 50 + 32, 25);
        //    RoundRecSection pme2ramp = new RoundRecSection(1400,1000, C35, Grade500, 32, 32, 50 + 32, 250);
        //    RoundRecSection pe14 = new RoundRecSection(1600, 900, C35, Grade500, 28, 32, 50 + 32, 225);

        //    RoundRecSection ptl1 = new RoundRecSection(1000, 1400, C35, Grade500, 24, 32, 50 + 32, 225);


        //    Column PM1 = new Column(25000, cpm1, cpm1);
        //    PM1.SetDesignLoad(15295e3, 1281e6, 9963e6, 15295e3, 1281e6, 9963e6);
        //    PM1.PrintReport();


        //    Column PM2 = new Column(25000, cpm1, cpm1);
        //    PM2.SetDesignLoad(17396e3, 462e6, 11842e6, 17396e3, 462e6, 11842e6);
        //    PM2.PrintReport();

        //    Column PTM2 = new Column(25000, ptm2, ptm2);
        //    PTM2.SetDesignLoad(8689e3, 2106e6, 2502e6, 8689e3, 2106e6, 2502e6);
        //    PTM2.PrintReport();

        //    Column PME2Main = new Column(10000, pme2main, pme2main);
        //    PME2Main.SetDesignLoad(11555e3, 541e6, 903e6, 11555e3, 541e6, 903e6);
        //    PME2Main.PrintReport();


        //    Column PME2Ramp = new Column(10000, pme2ramp, pme2ramp);
        //    PME2Ramp.SetDesignLoad(7623e3, 391e6, 1850e6, 7623e3, 391e6, 1850e6);
        //    PME2Ramp.PrintReport();

        //    Column PE14 = new Column(18000, pe14, pe14);
        //    PE14.SetDesignLoad(7493e3, 375e6, 2619e6, 7493e3, 375e6, 2619e6);
        //    PE14.PrintReport();

        //    Column PTL1C1 = new Column(15000, ptl1, ptl1);
        //    PTL1C1.SetDesignLoad(5907e3, 447e6, 1209e6, 5907e3, 447e6, 1209e6);
        //    PTL1C1.PrintReport();
        //    Column PTL1C2 = new Column(15000, ptl1, ptl1);
        //    PTL1C2.SetDesignLoad(9017e3, 443e6, 1200e6, 9017e3, 443e6, 1200e6);
        //    PTL1C2.PrintReport();


        //    Column PTL2 = new Column(15000, ptl1, ptl1);
        //    PTL2.SetDesignLoad(6786e3, 257e6, 1187e6, 6786e3, 257e6, 1187e6);
        //    PTL2.PrintReport();


        //}
    }
}
