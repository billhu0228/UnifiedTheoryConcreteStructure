using System.Windows.Controls;


namespace UnifiedTheoryConcreteStructure.NewUI
{
    /// <summary>
    /// HomeControl.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigControl : UserControl
    {
        
        public ConfigControl(ref UTCSViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        
        }
    }
}
