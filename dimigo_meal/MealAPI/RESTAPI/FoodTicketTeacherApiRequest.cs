﻿using MyBaseLib.Network;

namespace MealAPI.RESTAPI
{
    public class FoodTicketTeacherApiRequest : HttpApiRequestBase
    {
        private string _RFIDCode;
        public string RFIDCode
        {
            get
            {
                return this._RFIDCode;
            }
            set
            {
                this._RFIDCode = value;
                this.OnPropertyChanged("RFIDCode");
            }
        }
        private string _MealCount;
        public string MealCount
        {
            get
            {
                return this._MealCount;
            }
            set
            {
                this._MealCount = value;
                this.OnPropertyChanged("MealCount");
            }
        }
    }
}