using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnifiedTheoryConcreteStructure.Material;
using UnifiedTheoryConcreteStructure.Section;

namespace UnifiedTheoryConcreteStructure.NewUI
{
    public partial class UTCSViewModel : INotifyPropertyChanged
    {
        public UTCSViewModel()
        {

            _selConcreteList = new Dictionary<int, string>();
            _selRebarList = new Dictionary<int, string>();
            _selPreRebarList = new Dictionary<int, string>();


            _selConcreteList.Add(0, "AASHTO-Fc45");
            _selConcreteList.Add(1, "AASHTO-Fc35");
            _selConcreteList.Add(2, "JTG-C40");
            _selConcreteList.Add(3, "JTG-C50");

            _concID = 0;
            _concrete = new ConcreteProperty(_selConcreteList[_concID]);

            _selRebarList.Add(0, "HRB400");
            _selRebarList.Add(1, "HRB500");
            _rebarID = 0;
            _rebar = new Reinforcement(_selRebarList[_rebarID]);


            _selPreRebarList.Add(0, "Grade270-12.7");
            _selPreRebarList.Add(1, "Grade270-15.2");
            _prerebarID = 0;
            _prerebar = new Prestress(_selPreRebarList[_prerebarID], 1860);


            commonSection = new CommonSection();
            commonSection.Conc = _concrete;
            commonSection.MRebar = _rebar;
            commonSection.SRebar = _rebar;
            commonSection.PrestressProperty = _prerebar;
            
            _rebarCollection = GetRebarCollection(ref commonSection);
            _sectionCollection = GetSectionCollection(ref commonSection);

            _isVertMoment = true;
            _isCompression = true;
            
            _message = "Hello Kitty";
        }

        #region 字段
        //ConcretePropertyBase _concrete;
        ConcreteProperty _concrete;
        Reinforcement _rebar;
        Prestress _prerebar;
        Dictionary<int, string> _selConcreteList;
        Dictionary<int, string> _selRebarList;
        Dictionary<int, string> _selPreRebarList;
        int _concID;
        int _rebarID;
        int _prerebarID;



        #endregion

        #region 属性
        public string ConcreteName
        {
            get { return _concrete.Name; }
            set { _concrete.Name = value; RaisePropertyChanged("ConcreteName"); }
        }
        public string RebarName
        {
            get { return _rebar.Name; }
            set { _rebar.Name = value; RaisePropertyChanged("RebarName"); }
        }

        public string PreRebarName
        {
            get { return _prerebar.Name; }
            set { _prerebar.Name = value; RaisePropertyChanged("PreRebarName"); }
        }

        public Dictionary<int, string> SelConcreteList
        {
            get { return _selConcreteList; }
            set
            {
                _selConcreteList = value;
                RaisePropertyChanged("SelConcreteList");
            }
        }
        public Dictionary<int, string> SelRebarList
        {
            get { return _selRebarList; }
            set
            {
                _selRebarList = value;
                RaisePropertyChanged("SelRebarList");
            }
        }
        public Dictionary<int, string> SelPreRebarList
        {
            get { return _selPreRebarList; }
            set
            {
                _selPreRebarList = value;
                RaisePropertyChanged("SelPreRebarList");
            }
        }
        public int ConcID
        {
            get { return _concID; }
            set
            {
                _concID = value;
                _concrete = new ConcreteProperty(_selConcreteList[_concID]);
                RaisePropertyChanged("ConcID");
                RaisePropertyChanged("ConcreteName");
            }
        }
        public int RebarID
        {
            get { return _rebarID; }
            set
            {
                _rebarID = value;
                _rebar = new Reinforcement(_selRebarList[_rebarID]);
                RaisePropertyChanged("RebarID");
                RaisePropertyChanged("RebarName");
                commonSection.MRebar = _rebar;
                BarCollection = GetRebarCollection(ref commonSection);
                
            }
        }

        public int PreRebarID
        {
            get { return _prerebarID; }
            set
            {
                _prerebarID = value;
                _prerebar = new Prestress(_selPreRebarList[_prerebarID],1860);
                RaisePropertyChanged("PreRebarID");
                RaisePropertyChanged("PreRebarName");
                commonSection.PrestressProperty = _prerebar;
                PreBarCollection = GetPreRebarCollection(ref commonSection);

            }
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
