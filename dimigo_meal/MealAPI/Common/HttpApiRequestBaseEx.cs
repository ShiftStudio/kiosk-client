using MealAPI.RESTAPI;
using MyBaseLib.Network;

namespace MealAPI.Common
{
    public abstract class HttpApiResponseBaseEx : HttpApiResponseBase
    {
        private ApiStatus _status;
        public ApiStatus Status
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
