using dimigo_meal.Common;
using MyAPI.Model;
using MyAPI.RESTAPI;
using System;

namespace dimigo_meal.View.Common
{
    /// <summary>
    /// ResultDisplayView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResultDisplayView : RFIDReaderViewBaseEx
    {

        #region Constructor

        public ResultDisplayView(ResultDisplayViewModel ViewModel)
        {
            InitializeComponent();
            this.ViewModel = ViewModel;
            this.Unloaded += (s, e) =>
            {
                this.Dispose();
            };

            switch (this.ViewModel.Event.Status)
            {
                case clsEventStatus.SUCCESS:
                    this.PageLimitTime = 3;
                    break;
                default:
                    this.PageLimitTime = 5;
                    break;
            }
        }

        #endregion Constructor

        #region Properties

        public ResultDisplayViewModel ViewModel
        {
            get
            {
                return this.DataContext as ResultDisplayViewModel;
            }
            set
            {
                if (this.DataContext != value)
                {
                    this.DataContext = value;
                }
            }
        }

        #endregion Properties

        protected override void RFIDCode_Received(string RFIDCode)
        {
            FoodTicketStudentApiRequest request = new FoodTicketStudentApiRequest
            {
                RFIDCode = RFIDCode,
                TimeStamp = DateTime.Now.GetUnixTime(),
                        
            };
            FoodTicketStudentApi api = new FoodTicketStudentApi();
            api.ResponseSucceeded += this.Api_ResponseSucceeded;
            api.ResponseFailed += this.Api_ResponseFailed;
            api.Send(request);
        }

    
    }
}