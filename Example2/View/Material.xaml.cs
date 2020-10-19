using Example2.Test;
using System.Windows;
using System.Windows.Controls;


namespace Example2
{
    /// <summary>
    /// Material.xaml 的交互逻辑
    /// </summary>
    public partial class Material : UserControl
    {

        NameViewModel _viewModel;

        public Material()
        {
            InitializeComponent();
            _viewModel = base.DataContext as NameViewModel;
        }



        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            //界面更新
            _viewModel.Epsu = "0.01125";
            _viewModel.Ec = "4546";
        }
    }
}
