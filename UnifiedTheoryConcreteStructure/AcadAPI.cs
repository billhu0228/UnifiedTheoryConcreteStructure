using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AutoCAD;


namespace UnifiedTheoryConcreteStructure
{
    class AcadAPI
    {        

        public static void Test()
        {
            AcadApplication AcadApp = (AcadApplication)Marshal.GetActiveObject("AutoCAD.Application.22");
            AcadDocument doc = AcadApp.ActiveDocument;
            AcadModelSpace ms = doc.ModelSpace;
            AcadSelectionSets SelSets = doc.SelectionSets;
            //SelSets.
            AcadSelectionSet sles = SelSets.Add("Ssd");

            sles.SelectOnScreen();
            Console.WriteLine("{0}", sles.Count);

            List<AcadLWPolyline> curList = new List<AcadLWPolyline>();

            foreach (var item in sles)
            {
                try
                {
                    AcadLWPolyline PL = (AcadLWPolyline)item;
                    if (PL.Closed)
                    {
                        curList.Add(PL);
                    }

                }
                catch (Exception)
                {

                    continue;
                }


               // Console.WriteLine("Length={0}", L.Length);

            }
            
            sles.Delete();

        }
    }
}
