using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using DataBase;

namespace WpfApp20
{
    public class NameObject : INotifyPropertyChanged
    {
        string name;
        public NameObject(string name)
        {
            this.name = name;
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(Name);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
