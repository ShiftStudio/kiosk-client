using dimigo_meal.Common;
using MyAPI.RESTAPI;
using System;

namespace dimigo_meal.View
{
    /// <summary>
    /// RFIDScanViewWithButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RFIDScanViewWithButton : RFIDReaderViewBase
    {
        public RFIDScanViewWithButton()
        {
            InitializeComponent();
        }

        protected override void RFIDCode_Received(string RFIDCode)
        {
            FoodTicketTeacherApiRequest request = new FoodTicketTeacherApiRequest
            {
                RFIDCode = RFIDCode,
                TimeStamp = (DateTime.Now - DateTime.Parse("1970-01-01 09:00:00")).TotalSeconds
            };
            FoodTicketTeacherApi api = new FoodTicketTeacherApi();
            //api.ResponseSucceeded += this.Api_ResponseSucceeded;
            //api.ResponseFailed += this.Api_ResponseFailed;
            api.Send(request);
        }
    }
}
