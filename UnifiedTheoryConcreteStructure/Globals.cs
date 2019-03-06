using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedTheoryConcreteStructure
{
    public static class Globals
    {
        public static Dictionary<string, double[]> ConcParas=new Dictionary<string, double[]>();
        public static Dictionary<string, double[]> RebarParas = new Dictionary<string, double[]>();
        public static System.Windows.Forms.RichTextBox LogBox;
        static Globals()
        {
            // 混凝土参数字典
            var item = Resource1.ConcParas;
            Decoder d = Encoding.UTF8.GetDecoder();
            int charSize = d.GetCharCount(item, 0, item.Length);
            char[] chs = new char[charSize];
            d.GetChars(item, 0, item.Length, chs, 0);
            string s = new string(chs);
            var ss = s.Split('\n');

            for (int i = 0; i < ss.Length; i++)
            {
                string rcd = ss[i];
                rcd = rcd.Trim();
                var rcds = rcd.Split(',');

                ConcParas.Add(rcds[0], new double[] {
                    double.Parse(rcds[1]),
                    double.Parse(rcds[2]),
                    double.Parse(rcds[3]),
                    double.Parse(rcds[4]),
                    double.Parse(rcds[5])
                });                
            }

            // 混凝土参数字典
            item = Resource1.RebarParas;
            charSize = d.GetCharCount(item, 0, item.Length);
            chs = new char[charSize];
            d.GetChars(item, 0, item.Length, chs, 0);
            s = new string(chs);
            ss = s.Split('\n');
            for (int i = 0; i < ss.Length; i++)
            {
                string rcd = ss[i];
                rcd = rcd.Trim();
                var rcds = rcd.Split(',');

                RebarParas.Add(rcds[0], new double[] {
                    double.Parse(rcds[1]),
                    double.Parse(rcds[2]),
                    double.Parse(rcds[3]),
                });
            }




            LogBox = new System.Windows.Forms.RichTextBox();
        }

    }
}
