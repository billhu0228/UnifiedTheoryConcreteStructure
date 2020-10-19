using System.ComponentModel;
using UnifiedTheoryConcreteStructure.Material;

namespace UnifiedTheoryConcreteStructure.NewUI
{
    public class MaterialViewModel:INotifyPropertyChanged
    {
        public MaterialViewModel()
        {
            _concrete = new ConcreteAASHTO("B40", 40);
        }

        #region 字段
        ConcreteAASHTO _concrete;
        #endregion

        #region 属性
        public string Name
        {
            get { return _concrete.Name; }
            set { _concrete.Name = value; RaisePropertyChanged("Name"); }
        }
        public double Fck_cube
        {
            get { return _concrete.Fck_cube; }
            set { _concrete.Fck_cube = value; RaisePropertyChanged("Fck_cube"); }
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
