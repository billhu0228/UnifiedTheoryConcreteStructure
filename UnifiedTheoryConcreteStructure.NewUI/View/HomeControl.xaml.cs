using System.Windows.Controls;


namespace UnifiedTheoryConcreteStructure.NewUI
{
    /// <summary>
    /// HomeControl.xaml 的交互逻辑
    /// </summary>
    public partial class HomeControl : UserControl
    {
        
        public HomeControl(ref UTCSViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
