using System.Windows.Controls;


namespace UnifiedTheoryConcreteStructure.NewUI
{
    /// <summary>
    /// MaterialControl.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialControl : UserControl
    {
        //public MaterialViewModel MatViewModel;
        //public MaterialControl()
        //{    
        //    InitializeComponent();            
        //    MatViewModel = DataContext as MaterialViewModel;
        //    MatViewModel.Name = "ff";
        //}


        //public UTCSViewModel MatViewModel;
        public MaterialControl(ref UTCSViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            
        }
    }
}
