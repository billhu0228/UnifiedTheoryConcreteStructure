using AutoCAD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using UnifiedTheoryConcreteStructure.Public;
using UnifiedTheoryConcreteStructure.Section;
using System.Linq;

namespace UnifiedTheoryConcreteStructure.NewUI
{
    public partial class UTCSViewModel : INotifyPropertyChanged
    {


        #region 字段
        string _message;

        #endregion







        #region 属性

        public string OutMessage
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    RaisePropertyChanged("OutMessage");

                }
            }
        }

        #endregion


        #region 方法

        #endregion
    }
}
