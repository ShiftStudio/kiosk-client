using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace MyBaseLib.Network
{
    public abstract class HttpApiResponseBase : EventArgs, INotifyPropertyChanged
    {
        protected HttpApiResponseBase() { }

        protected virtual void ParseResponseStr() { }

        private string _ResponseStr;
        [JsonIgnore]
        public string ResponseStr
        {
            get
            {
                return this._ResponseStr;
            }
            set
            {
                this._ResponseStr = value;
                this.ParseResponseStr();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}