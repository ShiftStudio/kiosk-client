using MyBaseLib.Network;

namespace MyAPI.Model
{
    public abstract class HttpApiResponseBaseEx : HttpApiResponseBase
    {
        private clsMeal _meal;
        public clsMeal Meal
        {
            get
            {
                return _meal;
            }
            set
            {
                _meal = value;
                OnPropertyChanged("Meal");
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                this.OnPropertyChanged("Title");
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
    }
}
