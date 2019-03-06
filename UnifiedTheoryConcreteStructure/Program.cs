using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoCAD;


namespace UnifiedTheoryConcreteStructure
{
    class Program
    {
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //MainWinForm App = new MainWinForm();
            //Application.Run(App);


            //CommonSection bill = new CommonSection(30, 330);
            //Console.WriteLine("毛截面面积Ac={0:F3}", bill.Ac);
            //Console.WriteLine("截面型心X={0:F3},Y={1:F3}", bill.PlasticCenterX, bill.PlasticCenterY);
            //var RES = bill.SectionAnalysis();
            //Console.WriteLine("抗弯承载力Mu={0:F1}kNm", RES.Item2 * 1e-6);



            //DoubleReinforcedRectSection theSection = new DoubleReinforcedRectSection()
            //{
            //    B = 1000,
            //    H = height,
            //    Sv = 100,
            //    DepthT = height - 50 - 13 - (numeq) * 0.5 * D,
            //    DepthC = height - 50 - 13 - 0.5 * D,
            //    Conc = new Concrete(30.0, 1.39),
            //    CompRebar = new Reinforcement(D, 330),
            //    TensRebar = new Reinforcement(Dequal, 330),
            //    VertRebar = new Reinforcement(12, 280),
            //    NumRebarT = 1000.0 / 150.0,
            //    NumRebarC = 1000.0 / 150.0,
            //    NumRebarV = 1000.0 / 600.0,
            //};
            //var f = theSection.CalculateMu();
            //theSection.SectionAnalysis(-1647e3);
            //theSection.ShearAnalysis();



            Console.Write("输入直径(mm):");
            double DD = double.Parse(Console.ReadLine());
            Console.WriteLine("直径D={0}mm",DD);

            Console.Write("输入钢筋型心到边缘距离(mm):");
            double Cover = double.Parse(Console.ReadLine());
            Console.WriteLine("型心到边缘距离C={0}mm", Cover);

            Console.Write("输入主筋数量:");
            int NumS = int.Parse(Console.ReadLine());
            Console.WriteLine("主筋{0}根", NumS);

            Console.Write("输入主筋直径(mm):");
            int barD = int.Parse(Console.ReadLine());
            Console.WriteLine("主筋直径d={0}mm", barD);

            Console.Write("输入轴压(N，压力为负值):");
            int Ns = int.Parse(Console.ReadLine());
            Console.WriteLine("轴力={0}N", Ns);

            ReinforcedCircleSection kitty = new ReinforcedCircleSection()
            {
                
                D = DD,
                CoverT = Cover,
                NumRebar = NumS,
                MRebar = new Reinforcement(400, barD),
                SRebar = new Reinforcement(300, 12),
                Conc = new Concrete(40),
                Sv = 100.0,

            };
            var f = kitty.SectionAnalysis(Ns);

            Console.WriteLine("Fu={0:F1}kN,Mu={1:F1}kNm.",f.Item1*1e-3,f.Item2*1e-6);
            //kitty.SectionABCD();


            //FileStream fs = new FileStream("E:\\FM.csv", FileMode.Create);
            //StreamWriter sw = new StreamWriter(fs);
            //int steps = 20;
            //double N0 = -1000e3;
            //double N1 = -18000e3;
            //double dN = (N1 - N0) / steps;
            //for (int i = 0; i <= steps; i++)
            //{
            //    var f = kitty.SectionAnalysis(N0 + i * dN);
            //    sw.WriteLine("{0:F1},{1:F1}", f.Item2 / 1e6, f.Item1 / 1e3);
            //    sw.Flush();
            //}
            //sw.Close();
            //fs.Close();

            Console.WriteLine("分析完成.");
            Console.ReadKey();

        }



    }
}



