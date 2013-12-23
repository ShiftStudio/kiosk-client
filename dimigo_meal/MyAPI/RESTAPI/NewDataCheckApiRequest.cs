using MyBaseLib.Network;

namespace MyAPI.RESTAPI
{
    public class NewDataCheckApiRequest : HttpApiRequestBase
    {
        private double _TimeStamp;
        public double TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
                this.OnPropertyChanged("TimeStamp");
            }
        }
    }
}