using dimigo_meal.Common;
using MyAPI.RESTAPI;
using MyBaseLib.Network;
using System;

namespace dimigo_meal.View.Student
{
    /// <summary>
    /// RFIDScanStudentView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RFIDScanView : RFIDReaderViewBaseEx
    {
        #region Constructor

        public RFIDScanView()
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
                TimeStamp = DateTime.Now.GetUnixTime()
            };
            FoodTicketStudentApi api = new FoodTicketStudentApi();
            api.ResponseSucceeded += base.Api_ResponseSucceeded;
            api.ResponseFailed += base.Api_ResponseFailed;
            api.Send(request);
        }
    }
}