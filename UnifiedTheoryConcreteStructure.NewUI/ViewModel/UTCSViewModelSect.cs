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

    public enum LoadType { ConstantForce,ConstantMoment,ConstantEc}

    public partial class UTCSViewModel : INotifyPropertyChanged
    {
        #region 字段
        LoadType _loadtype;
        double _nxValue;
        double _momValue;
        double _ec;
        CommonSection commonSection;
        ObservableCollection<PreRebarProperty> _preRebarCollection;
        ObservableCollection<RebarProperty> _rebarCollection;
        ObservableCollection<SectionProperty> _sectionCollection;
        bool _isVertMoment;
        bool _isCompression;
        #endregion

                     

        #region 属性

        public ObservableCollection<SectionProperty> SectionCollection
        {
            get => _sectionCollection;
            set
            {
                if (!this._sectionCollection.Equals(value))
                {
                    _sectionCollection = value;
                    RaisePropertyChanged("SectionCollection");
                }
            }
        }
        public ObservableCollection<RebarProperty> BarCollection
        {
            get => _rebarCollection;
            set
            {
                if (!this._rebarCollection.Equals(value))
                {
                    _rebarCollection = value;
                    RaisePropertyChanged("BarCollection");
                }
            }
        }

        public ObservableCollection<PreRebarProperty> PreBarCollection
        {
            get => _preRebarCollection;
            set
            {
                if (!this._rebarCollection.Equals(value))
                {
                    _preRebarCollection = value;
                    RaisePropertyChanged("PreBarCollection");
                }
            }
        }

        public DelegateCommand GetGetGet
        {
            get { return new DelegateCommand(PickSection); }
        }
        public DelegateCommand GetMsg
        {

            get 
            {
                ;
                return new DelegateCommand(GetMessage); 
            }
        }


        public LoadType CurLoadType
        {
            get { return _loadtype; }
            set { _loadtype = value; RaisePropertyChanged("CurLoadTyep"); RaisePropertyChanged("LoadTypeName"); }
        }

        public string LoadTypeName
        {
            get { return _loadtype.ToString(); }
            
        }

        public bool IsVertMoment
        {
            get { return _isVertMoment; }
            set { _isVertMoment = value;RaisePropertyChanged("IsVertMoment"); }
        }     
        public bool IsCompression
        {
            get { return _isCompression; }
            set { _isCompression = value;RaisePropertyChanged("IsCompression"); }
        }

        public double NxValue
        {
            get { return _nxValue; }
            set { _nxValue = value; RaisePropertyChanged("NxValue"); }
        }
        public double MomValue
        {
            get { return _momValue; }
            set { _momValue = value; RaisePropertyChanged("MomValue"); }
        }
        public double Ecenter
        {
            get { return _ec; }
        }





        #endregion


        #region 方法

        void GetMessage(object parameter)
        {
            
            var ff = commonSection.SectionAnalysis(out double CC);
            OutMessage += ff.ToString();
            OutMessage += "受压区高度：";
            OutMessage += CC.ToString();
            OutMessage += "\n";
        }

        void PickSection(object obj)
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

            if (curSelection.Count==0)
            {
                App.Current.MainWindow.Activate();
                return;

            }

            commonSection = new CommonSection(curSelection);

            commonSection.Conc = _concrete;
            commonSection.MRebar = _rebar;
            commonSection.SRebar = _rebar;
            commonSection.PrestressProperty = _prerebar;



            BarCollection = GetRebarCollection(ref commonSection);
            SectionCollection = GetSectionCollection(ref commonSection);
            PreBarCollection = GetPreRebarCollection(ref commonSection);




            App.Current.MainWindow.Activate();

        }


        static private ObservableCollection<SectionProperty> GetSectionCollection(ref CommonSection commonSection)
        {
            var res = new ObservableCollection<SectionProperty>();

            res.Add(new SectionProperty()
            {
                换算面积A0 = double.Parse(commonSection.A0.ToString("F2")),
                X0 = double.Parse(commonSection.PlasticCenterX.ToString("F2")),
                Y0 = double.Parse(commonSection.PlasticCenterY.ToString("F2")),
                混凝土Ix = commonSection.Icx.ToString("E2"),
                混凝土Iy = commonSection.Icy.ToString("E2"),
                换算截面Ix = commonSection.Ix0.ToString("E2"),
                换算截面Iy = commonSection.Iy0.ToString("E2"),
            });


            return res;
        }

        static private ObservableCollection<RebarProperty> GetRebarCollection(ref CommonSection commonSection)
        {
            var res = new ObservableCollection<RebarProperty>();
            if (commonSection.Rebars.Count==0)
            {
                return res;
            }
            foreach (var item in commonSection.Rebars)
            {
                res.Add(new RebarProperty()
                {
                    Index = item.GetHashCode(),
                    X = double.Parse(item.Center.X.ToString("F2")),
                    Y = double.Parse(item.Center.Y.ToString("F2")),
                    Ds = double.Parse(item.Diameter.ToString("F2")),         
                    Material=commonSection.MRebar.Name
                });
            }
            return res;
        }


        static private ObservableCollection<PreRebarProperty> GetPreRebarCollection(ref CommonSection commonSection)
        {
            var res = new ObservableCollection<PreRebarProperty>();
            if (commonSection.PreRebars.Count == 0)
            {
                return res;
            }
            foreach (var item in commonSection.PreRebars)
            {
                res.Add(new PreRebarProperty()
                {
                    Index = item.GetHashCode(),
                    X = double.Parse(item.Center.X.ToString("F2")),
                    Y = double.Parse(item.Center.Y.ToString("F2")),
                    Ds = double.Parse(item.Diameter.ToString("F2")),
                    Material = commonSection.PrestressProperty.Name
                });
            }
            return res;
        }


        [DllImport("user32.dll ")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
