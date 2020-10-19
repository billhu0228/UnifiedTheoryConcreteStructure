using System.Windows;

namespace Example2
{
    /// <summary>
    /// 主页面逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 视图模型
        /// </summary>


        public MainWindow()
        {
            InitializeComponent();
            ContentControl.Content = new UnifiedTheoryConcreteStructure.NewUI.MaterialControl();
            //已经在xaml代码中声明了视图模型实例，获取到引用因此我们可以在按钮click事件里使用它
            

        }

    }
}
