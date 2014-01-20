using MyAPI.Common;
using MyAPI.Model;
namespace MyAPI.RESTAPI
{
    public class NewDataCheckApiResponse : HttpApiResponseBaseEx
    {
        private clsMeal _meal;
        public clsMeal Meal
        {
            get
            {
                return this._meal;
            }
            set
            {
                this._meal = value;
                this.OnPropertyChanged("Meal");
            }
        }

        /*private clsEvent _event;
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
        }*/
    }
}