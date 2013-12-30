using dimigo_meal.Common;
using dimigo_meal.MyAPI.RESTAPI;
using MyAPI.RESTAPI;
using MyBaseLib.Network;
using System;

namespace dimigo_meal.View
{
    /// <summary>
    /// RFIDScanViewStudent.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RFIDScanViewStudent : RFIDReaderViewBaseEx
    {
        #region Constructor

        public RFIDScanViewStudent()
        {
            InitializeComponent();
            this.PageLimitTime = -1;
        }

        #endregion Constructor

        protected override void RFIDCode_Received(string RFIDCode)
        {
            FoodTicketStudentApiRequest request = new FoodTicketStudentApiRequest
            {
                RFIDCode = RFIDCode,
                TimeStamp = (DateTime.Now - DateTime.Parse("1970-01-01 09:00:00")).TotalSeconds
            };
            FoodTicketStudentApi api = new FoodTicketStudentApi();
            api.ResponseSucceeded += base.Api_ResponseSucceeded;
            api.ResponseFailed += base.Api_ResponseFailed;
            api.Send(request);
        }
    }
}