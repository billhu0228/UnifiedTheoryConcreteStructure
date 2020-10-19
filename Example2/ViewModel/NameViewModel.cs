
/******************作者：黄昏前黎明后*********************************
*   作者：黄昏前黎明后
*   CLR版本：4.0.30319.42000
*   创建时间：2018-04-02 13:53:52
*   命名空间：Example1
*   唯一标识：617472f7-cdf9-49f9-afa4-7655f1f5774a
*   机器名称：HLPC
*   联系人邮箱：hl@cn-bi.com
* 
*   描述说明：
*
*   修改历史：
*
*
*****************************************************************/
using System.ComponentModel;
using UnifiedTheoryConcreteStructure.Material;

namespace Example2.Test
{
    public class NameViewModel : INotifyPropertyChanged
    {
        public NameViewModel()
        {
            //_userName = new Name() { UserName = "黄昏前黎明后", CompanyName = "SoftEasy" };
            _C4 = new ConcreteAASHTO("fuck", 22);
        }

        #region 字段
        ConcreteAASHTO _C4;
        //Name _userName;
        #endregion

        #region 属性
        public string Ec
        {
            get { return _C4.Ec.ToString(); }
            set { _C4.Ec = double.Parse(value); RaisePropertyChanged("Ec"); }
        }
        public string Epsu
        {
            get { return _C4.Epsu.ToString(); }
            set { _C4.Epsu = double.Parse(value); RaisePropertyChanged("Epsu"); }
        }
        #endregion

        #region INotifyPropertyChanged属性
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region 属性更改方法
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
