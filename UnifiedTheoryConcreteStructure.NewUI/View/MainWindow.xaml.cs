using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace UnifiedTheoryConcreteStructure.NewUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        UTCSViewModel vm = new UTCSViewModel();

        ContentControl HomeCon;
        ContentControl ConfigCon;
        ContentControl MaterialCon;
        ContentControl SectionCon;


        public MainWindow()
        {
            InitializeComponent();
            MaterialCon = new MaterialControl(ref vm);
            HomeCon = new HomeControl(ref vm);
            ConfigCon = new ConfigControl(ref vm);
            SectionCon = new SectionControl(ref vm);

            ContentControl.Content = HomeCon;
            DataContext = vm;

        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            //界面更新

        }

        private void HomePage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentControl.Content = HomeCon;

        }

        private void Config_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentControl.Content = ConfigCon;
        }

        private void Material_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentControl.Content = MaterialCon;

        }

        private void Section_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentControl.Content = SectionCon;

        }

        private void Member_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
