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
    public class SectionViewModel : INotifyPropertyChanged
    {
        #region 字段
        List<int> _barDia;
        List<string> _barClass;
        int _selectedDia;
        string _selectedBarClass;
        private ICommand _pickCAD;
        private ICommand _run;
        

        CommonSection commonSection;
        private ObservableCollection<RebarProperty> _items3;
        private ObservableCollection<SectionProperty> _items1;
        #endregion




        public SectionViewModel()
        {
            _barDia = new List<int>() { 12, 14, 16, 18 };
            _barClass = new List<string>() { "HPB300", "HRB400", "HRB500" };
            _selectedDia = 12;
            _pickCAD = new RelayCommand(PickSection);
            _run = new RelayCommand(RunCalculation);
            commonSection = new CommonSection();
            _items3 = GetRebarCollection(ref commonSection);
            _items1 = GetSectionCollection(ref commonSection);
            
        }

        internal void SetMat(object sender, PropertyChangedEventArgs e)
        {
            MaterialViewModel ss = (MaterialViewModel)sender;
            if (e.PropertyName=="Name")
            {

                foreach (var reb in _items3)
                {
                    reb.Material = ss.Name;

                }
            }
        }

        static private ObservableCollection<SectionProperty> GetSectionCollection(ref CommonSection commonSection)
        {
            var res = new ObservableCollection<SectionProperty>();

            res.Add(new SectionProperty()
            {
                换算面积A0 = double.Parse(commonSection.Ac.ToString("F2")),
                X0 = double.Parse(commonSection.PlasticCenterX.ToString("F2")),
                Y0 = double.Parse(commonSection.PlasticCenterY.ToString("F2")),
                Ix ="",
                Iy = "",
            });


            return res;
        }

        static private ObservableCollection<RebarProperty> GetRebarCollection(ref CommonSection commonSection)
        {
            var res = new ObservableCollection<RebarProperty>();

            foreach (var item in commonSection.Rebars)
            {
                res.Add(new RebarProperty()
                {
                    Index = item.GetHashCode(),
                    X = double.Parse(item.Center.X.ToString("F2")),
                    Y = double.Parse(item.Center.Y.ToString("F2")),
                    Ds = double.Parse(item.Diameter.ToString("F2")),                   
                });

            }


            return res;
        }



        #region 属性

        public ObservableCollection<SectionProperty> SectionCollection
        {
            get => _items1;
            set
            {
                if (!this._items1.Equals(value))
                {
                    _items1 = value;
                    RaisePropertyChanged("SectionCollection");
                }
            }
        }

        public ObservableCollection<RebarProperty> BarCollection
        {
            get => _items3;
            set
            {
                if (!this._items3.Equals(value))
                {
                    _items3 = value;
                    RaisePropertyChanged("BarCollection");
                }
            }
        }

        public ICommand PickCADCommand
        {
            get
            {
                return _pickCAD;
            }
            set
            {
                _pickCAD = value;
            }
        }


        public ICommand Run
        {
            get
            {
                return _run;
            }
            set
            {
                _run = value;
            }
        }

        public List<int> BarDia
        {
            get => _barDia;
            set
            {
                if (!this._barDia.Equals(value))
                {
                    _barDia = value;
                    RaisePropertyChanged("BarDia");
                }
            }
        }
        public int SellectDia
        {
            get { return _selectedDia; }
            set
            {
                if (_selectedDia!=value)
                {
                    _selectedDia = value;
                    RaisePropertyChanged("Selected");

                }
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;

        [DllImport("user32.dll ")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RunCalculation(object obj)
        {
            var ff=commonSection.SectionAnalysis();
            var f=App.Current;
            //OUT.Content += ff.ToString();

        }
        public void PickSection(object obj)
        {
    
            AcadApplication AcadApp = null;
            AcadDocument doc = null;
            AcadSelectionSets SelSets = null;
            AcadSelectionSet curSelection = null;
            //InnerLines.Clear();
            AcadApp = (AcadApplication)Marshal.GetActiveObject("AutoCAD.Application");
            SetForegroundWindow((IntPtr)AcadApp.HWND);
            doc = AcadApp.ActiveDocument;
            SelSets = doc.SelectionSets;
            curSelection = SelSets.Add(Guid.NewGuid().ToString());
            curSelection.Clear();
            curSelection.SelectOnScreen();

            commonSection = new CommonSection(curSelection);


            //var bars = from AcadEntity a in curSelection where a.EntityType == 8 select a as AcadCircle;
            //commonSection = new CommonSection();
            //foreach (AcadCircle item in bars)
            //{                
            //    commonSection.ComRebars.Add(item.Circle2D());
            //}



            BarCollection = GetRebarCollection(ref commonSection);
            SectionCollection = GetSectionCollection(ref commonSection);



            App.Current.MainWindow.Activate();

            //BarDia.Add(55);
            //MessageBox.Show(string.Format("共选择{0}个对象。", curSelection.Count));
        }
    }
}
