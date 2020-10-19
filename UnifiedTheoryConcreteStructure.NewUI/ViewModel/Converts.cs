using System;
using System.Windows.Data;

namespace UnifiedTheoryConcreteStructure.NewUI
{
    public class LoadTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            LoadType s = (LoadType)value;
            return s == (LoadType)int.Parse(parameter.ToString());
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null && value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }


    public class LoadTypeToBoolConverterForEnable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            LoadType s = (LoadType)value;
            return s == (LoadType)int.Parse(parameter.ToString());
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null && value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }


}
