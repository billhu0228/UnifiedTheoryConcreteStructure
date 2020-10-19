using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedTheoryConcreteStructure.Public;

namespace UnifiedTheoryConcreteStructure.NewUI
{
    public class SpecViewModel:INotifyPropertyChanged
    {

        public SpecViewModel()
        {
            _spec = new Specification();
        }

        #region 字段
        Specification _spec;
        #endregion

        #region 属性
        public string Name
        {
            get { return _spec.Name.ToString(); }
            set {

                var ff = value;

                _spec.Name = SpecName.BS5400;




                RaisePropertyChanged("Name"); }
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
