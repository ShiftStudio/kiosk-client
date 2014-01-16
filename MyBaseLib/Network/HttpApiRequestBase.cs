using System.ComponentModel;

namespace MyBaseLib.Network
{
    public abstract class HttpApiRequestBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private double _TimeStamp;
        public double TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
                this.OnPropertyChanged("TimeStamp");
            }
        }
    }
}