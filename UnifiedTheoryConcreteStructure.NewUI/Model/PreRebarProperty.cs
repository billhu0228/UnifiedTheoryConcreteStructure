using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UnifiedTheoryConcreteStructure.NewUI
{
    public class PreRebarProperty : INotifyPropertyChanged
    {

        private int _id;
        private double _ds;
        private double _locx;
        private double _locy;
        private string _material;




        public int Index
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public double X
        {
            get { return _locx; }
            set
            {
                if (_locx == value) return;
                _locx = value;
                OnPropertyChanged();
            }
        }

        public double Y
        {
            get { return _locy; }
            set
            {
                if (_locy == value) return;
                _locy = value;
                OnPropertyChanged();
            }
        }


        public double Ds
        {
            get { return _ds; }
            set
            {
                if (_ds == value) return;
                _ds = value;
                OnPropertyChanged();
            }
        }
        public string Material
        {
            get { return _material; }
            set
            {
                if (_material == value) return;
                _material = value;
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
