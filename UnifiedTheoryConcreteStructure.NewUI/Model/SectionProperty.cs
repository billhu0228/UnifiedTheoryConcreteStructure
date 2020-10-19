using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UnifiedTheoryConcreteStructure.NewUI
{
    public class SectionProperty: INotifyPropertyChanged
    {
        private double _area;
        private double _centerx;
        private double _centery;
        private string _Ix;
        private string _Iy;
        private string _Ix0;
        private string _Iy0;


        public double 换算面积A0
        {
            get { return _area; }
            set
            {
                if (_area == value) return;
                _area = value;
                OnPropertyChanged();
            }
        }
        public double X0
        {
            get { return _centerx; }
            set
            {
                if (_centerx == value) return;
                _centerx = value;
                OnPropertyChanged();
            }
        }
        
        public double Y0
        {
            get { return _centery; }
            set
            {
                if (_centery == value) return;
                _centery = value;
                OnPropertyChanged();
            }
        }
        
        public string 混凝土Ix
        {
            get { return _Ix; }
            set
            {
                if (_Ix == value) return;
                _Ix = value;
                OnPropertyChanged();
            }
        }        
        public string 混凝土Iy
        {
            get { return _Iy; }
            set
            {
                if (_Iy == value) return;
                _Iy = value;
                OnPropertyChanged();
            }
        }


        public string 换算截面Ix
        {
            get { return _Ix0; }
            set
            {
                if (_Ix0 == value) return;
                _Ix0 = value;
                OnPropertyChanged();
            }
        }
        public string 换算截面Iy
        {
            get { return _Iy0; }
            set
            {
                if (_Iy0 == value) return;
                _Iy0 = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
