using System;
using System.ComponentModel;

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
}
