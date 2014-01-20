using MealAPI.Common;
using MealAPI.Model;

namespace MealAPI.RESTAPI
{
    public class FoodTicketCheckApiResponse : HttpApiResponseBaseEx
    {

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