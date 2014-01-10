using MyAPI.Model;

namespace MyAPI.RESTAPI
{
    public class FoodTicketCheckApiResponse : HttpApiResponseBaseEx
    {
        private FoodTicketCheckApiStatus _status;
        public FoodTicketCheckApiStatus Status
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

        private clsPeople _user;
        public clsPeople User
        {
            get
            {
                return this._user;
            }
            set
            {
                this._user = value;
                this.OnPropertyChanged("User");
            }
        }

        private clsEvent _event;
        public clsEvent Event
        {
            get
            {
                return this._event;
            }
            set
            {
                this._event = value;
                this.OnPropertyChanged("Event");
            }
        }
    }
}