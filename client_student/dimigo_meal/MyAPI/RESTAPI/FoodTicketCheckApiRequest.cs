using MyBaseLib.Network;

namespace MyAPI.RESTAPI
{
    public class FoodTicketCheckApiRequest : HttpApiRequestBase
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
    }
}