using dimigo_meal.Common;
using dimigo_meal.Model;
using MyAPI.RESTAPI;
using System;

namespace dimigo_meal.View.Teacher
{
    /// <summary>
    /// RFIDScanView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RFIDScanView : RFIDReaderViewBaseEx
    {
        #region Constructor

        public RFIDScanView()
        {
            InitializeComponent();
            this.PageLimitTime = 5;
            this.Unloaded += (s, e) =>
            {
                this.Dispose();
            };
        }

        public RFIDScanView(RFIDScanViewModel ViewModel)
        {
            InitializeComponent();
            this.ViewModel = ViewModel;
            this.PageLimitTime = 5;
            this.Unloaded += (s, e) =>
            {
                this.Dispose();
            };
        }

        #endregion Constructor

        #region Properties

        public RFIDScanViewModel ViewModel
        {
            get
            {
                return this.DataContext as RFIDScanViewModel;
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

        #region Event

        protected override void RFIDCode_Received(string RFIDCode)
        {
            FoodTicketTeacherApiRequest request = new FoodTicketTeacherApiRequest
            {
                RFIDCode = RFIDCode,
                TimeStamp = DateTime.Now.GetUnixTime(),
                MealCount = this.ViewModel.MealCount
            };
            FoodTicketTeacherApi api = new FoodTicketTeacherApi();
            api.ResponseSucceeded += base.Api_ResponseSucceeded;
            api.ResponseFailed += base.Api_ResponseFailed;
            api.Send(request);
        }

        private void btn_Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewStateManager.NavigateHome();
        }

        #endregion Event
    }
}
