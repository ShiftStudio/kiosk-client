using System;

namespace MyAPI.Model
{
    public class clsEvent : MyAPI.Common.NotifiableModelBase
    {
        private clsEventStatus _status;
        public clsEventStatus Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
                this.OnPropertyChanged("Status");
            }
        }

        private string _message;
        public string Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
                this.OnPropertyChanged("Message");
            }
        }

        private DateTime _regularTime;
        public DateTime RegularTime
        {
            get
            {
                return _regularTime;
            }
            set
            {
                _regularTime = value;
                OnPropertyChanged("RegularTime");
            }
        }

        private DateTime _checkoutTime;
        public DateTime CheckoutTime
        {
            get
            {
                return _checkoutTime;
            }
            set
            {
                _checkoutTime = value;
                OnPropertyChanged("CheckoutTime");
            }
        }

        private TimeSpan _diffTime;
        public TimeSpan DiffTime
        {
            get
            {
                return _diffTime;
            }
            set
            {
                _diffTime = value;
                OnPropertyChanged("DiffTime");
            }
        }
    }
}

//public int Status { get; set; }
//public string Message { get; set; }

//public DateTime RegularTime { get; set; }
//public DateTime CheckoutTime { get; set; }

//public TimeSpan DiffTime { get; set; }