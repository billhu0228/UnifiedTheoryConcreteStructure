using System;
using System.Collections.Generic;
using UnifiedTheoryConcreteStructure.Section;

namespace UnifiedTheoryConcreteStructure.Member
{
    public class Column
    {
        private double Nt, Mxt, Myt, Nb, Mxb, Myb;

        public double L0;
        public double ELFy,ELFx;
        public BaseSection topSection, botSection;

        public Column(double l0, BaseSection topSection, BaseSection botSection, double elfy=1.3,double elfx=1.2)
        {
            L0 = l0;
            this.topSection = topSection;
            this.botSection = botSection;
            ELFy = elfy;
            ELFx = elfx;
        }








        public void SetDesignLoad(double Nt,double Mxt,double Myt,double Nb,double Mxb,double Myb)
        {
            this.Nt = Nt;
            this.Mxt = Mxt;
            this.Myt = Myt;
            this.Nb = Nb;
            this.Mxb = Mxb;
            this.Myb = Myb;
        }

        public List<string> PrintReport()
        {
            Console.WriteLine("---------------分析开始------------------");
            List<string>res =new List<string>();
            double slenderX = ELFx * L0 / (0.3 * botSection.B);
            double slenderY = ELFy * L0 / (0.3 * botSection.H);

            double Prtop = 0.75 * topSection.P0();
            double Prbot = 0.75 * botSection.P0();
            //Console.WriteLine("{0:F0}", Math.Abs(Pr) * 1e-3);
            // 1-求轴压承载力Pr
            if (Nt>=0.1*0.75*topSection.ConcProperty.Fck_cylinder* topSection.Ag)
            {
                // 受压控制
                Console.WriteLine("结构受压控制");
                double Pxr = 0.75*Math.Abs(topSection.GetPnX(Myt / Nt));
                double Pyr = 0.75 * Math.Abs(topSection.GetPnY(Mxt / Nt));
                double Pxyr = 1 / (1 / Pxr + 1 / Pyr - 1 / Prtop);
                Console.WriteLine("{0:F0}\n{1:F0}\n{2:F0}\n{3:F2}", Prtop*1e-3,Pyr*1e-3,Pxr*1e-3,Pxyr / Nt);
            }
            else
            {
                // 弯矩控制
                Console.WriteLine("结构受弯控制");

                double Mux = topSection.GetMnX(-Nb);
                double Muy = topSection.GetMnY(-Nb);

                Console.WriteLine("{0:F0}", Math.Abs(Mux) * 1e-6);
                Console.WriteLine("{0:F0}", Math.Abs(Muy) * 1e-6);
            }

            if (Nb >= 0.1 * 0.75 * botSection.ConcProperty.Fck_cylinder * botSection.Ag)
            {
                // 受压控制
                Console.WriteLine("结构受压控制");
                double Pxr = 0.75 * Math.Abs(botSection.GetPnX(Myb / Nb));
                double Pyr = 0.75 * Math.Abs(botSection.GetPnY(Mxb / Nb));
                double Pxyr = 1 / (1 / Pxr + 1 / Pyr - 1 / Prbot);
                Console.WriteLine("{0:F0}\n{1:F0}\n{2:F0}\n{3:F2}", Prbot * 1e-3, Pyr * 1e-3, Pxr * 1e-3, Pxyr / Nb);
            }
            else
            {
                // 弯矩控制
                Console.WriteLine("结构受弯控制");

                double Mux = botSection.GetMnX(-Nb);
                double Muy = botSection.GetMnY(-Nb);

                Console.WriteLine("{0:F0}", Math.Abs(Mux) * 1e-6);
                Console.WriteLine("{0:F0}", Math.Abs(Muy) * 1e-6);
            }

            return res;
        }
    }
}
