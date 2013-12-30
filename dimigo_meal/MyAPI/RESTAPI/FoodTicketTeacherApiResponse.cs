using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAPI.Model;


namespace MyAPI.RESTAPI
{
    public class FoodTicketTeacherApiResponse : HttpApiResponseBaseEx
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

    }
}
