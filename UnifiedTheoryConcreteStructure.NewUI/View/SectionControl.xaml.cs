using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace UnifiedTheoryConcreteStructure.NewUI
{
    /// <summary>
    /// MaterialControl.xaml 的交互逻辑
    /// </summary>
    /// 


    public partial class SectionControl : UserControl
    {
        
        public SectionControl(ref UTCSViewModel vm)
        {
            InitializeComponent();            
            DataContext = vm;
        }


        //SectionViewModel RVM;
        //public SectionControl()
        //{
        //    InitializeComponent();
        //    RVM = new SectionViewModel();
        //    this.DataContext = RVM;
        //}

        private void NumberValidationTextBox(object sender,TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
