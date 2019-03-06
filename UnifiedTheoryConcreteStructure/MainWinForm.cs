using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnifiedTheoryConcreteStructure
{
    public partial class MainWinForm : Form
    {
        private Concrete MainConcrete;
        private Reinforcement MainRebar;
        private CommonSection commonSection;

        void InitializeObjec()
        {
            MainConcrete = new Concrete(comboBox1.Text);
            MainRebar = new Reinforcement(comboBox2.Text);
            commonSection = new CommonSection(MainConcrete,MainRebar);
            Globals.LogBox = curLogBox;
        }

        public MainWinForm()
        {
            InitializeComponent();

            Icon = Resource1.application;           
            
            InitializeObjec();
        }

        private void MainWinForm_Load(object sender, EventArgs e)
        {
            for (int i = treeView1.GetNodeCount(false) - 1; i > -1; i--)
            {
                treeView1.SelectedNode = treeView1.Nodes[i];
                treeView1.SelectedNode.ExpandAll();
            }

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ConcBox = (ComboBox)sender;
            double cname = double.Parse(ConcBox.Text.Substring(1));
            MainConcrete = new Concrete(cname);
            label10.Text = string.Format("{0:F1}", MainConcrete.Fck );
            label11.Text = string.Format("{0:F1}", MainConcrete.Fcd);
            label13.Text = string.Format("{0:F2}", MainConcrete.Ftk);
            label12.Text = string.Format("{0:F2}", MainConcrete.Ftd);
            label14.Text = string.Format("{0:E2}", MainConcrete.Ec);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            commonSection.Conc = MainConcrete;
            commonSection.MRebar = MainRebar;
            commonSection.SRebar = MainRebar;
            commonSection.GetSection();


        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            commonSection.SectionAnalysis();
        }

        private void RunBut_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox BarBox = (ComboBox)sender;
            string cname = BarBox.Text;
            MainRebar = new Reinforcement(cname);
            label28.Text = string.Format("{0:F1}", MainRebar.Fsk);
            label18.Text = string.Format("{0:F1}", MainRebar.Fy);            
            label31.Text = string.Format("{0:E2}", MainRebar.Es);
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }
    }
}
