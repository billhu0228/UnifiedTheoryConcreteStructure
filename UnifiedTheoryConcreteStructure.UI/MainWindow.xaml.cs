using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;


namespace UnifiedTheoryConcreteStructure.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RectPropSp.Visibility=Visibility.Collapsed;
            TeePropSp.Visibility = Visibility.Collapsed;

            img.Source = Imaging.CreateBitmapSourceFromHBitmap(Resource1.Rect1.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private void SectionTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }
            ComboBox SelectBox = (ComboBox)sender;
            var f= SelectBox.SelectedIndex;
            switch (f)
            {
                case 0:
                    CirclePropSP.Visibility = Visibility.Visible;
                    RectPropSp.Visibility = Visibility.Collapsed;
                    TeePropSp.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    CirclePropSP.Visibility = Visibility.Collapsed;
                    RectPropSp.Visibility = Visibility.Visible;
                    TeePropSp.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    CirclePropSP.Visibility = Visibility.Collapsed;
                    RectPropSp.Visibility = Visibility.Collapsed;
                    TeePropSp.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }

        }
    }
}
