using dimigo_meal.Common;
using MyAPI.RESTAPI;
using MyBaseLib.Network;
using System;

namespace dimigo_meal.View
{
    /// <summary>
    /// ResultDisplayView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ErrorDisplayView : RFIDReaderViewBase
    {
        #region Constructor

        public ErrorDisplayView(ErrorDisplayViewModel ViewModel)
        {
            InitializeComponent();
            this.ViewModel = ViewModel;
            this.Unloaded += (s, e) =>
            {
                this.Dispose();
            };

            switch (this.ViewModel.Status)
            {
                case ApiStatus.UNKNOWN_ERROR:
                    this.PageLimitTime = 8;
                    break;
                case ApiStatus.NETWORK_ERROR:
                    this.PageLimitTime = 8;
                    break;
                default:
                    this.PageLimitTime = 5;
                    break;
            }
        }

        #endregion Constructor

        #region Properties

        public ErrorDisplayViewModel ViewModel
        {
            get
            {
                return this.DataContext as ErrorDisplayViewModel;
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
            FoodTicketCheckApiRequest request = new FoodTicketCheckApiRequest
            {
                RFIDCode = RFIDCode,
                TimeStamp = (DateTime.Now - DateTime.Parse("1970-01-01 09:00:00")).TotalSeconds
            };
            FoodTicketCheckApi api = new FoodTicketCheckApi();
            api.ResponseSucceeded += this.Api_ResponseSucceeded;
            api.ResponseFailed += this.Api_ResponseFailed;
            api.Send(request);
        }

        private void Api_ResponseSucceeded(object sender, HttpApiResponseBase e)
        {
            FoodTicketCheckApi apiObj = sender as FoodTicketCheckApi;
            FoodTicketCheckApiRequest request = apiObj.HttpApiRequest as FoodTicketCheckApiRequest;
            FoodTicketCheckApiResponse response = apiObj.HttpApiResponse as FoodTicketCheckApiResponse;

            if (response.Status >= 0)
            {
                App.MainWindow.ViewModel.MealState = response.Meal.MealState;
                App.MainWindow.ViewModel.MealData = response.Meal.MealData;

                ResultDisplayViewModel vm = new ResultDisplayViewModel()
                {
                    User = response.User,
                    Event = response.Event
                };
                App.MainFrame.Navigate(new ResultDisplayView(vm));

                NarrationPlayer sp = new NarrationPlayer();
                if (response.Event.Status >= 0)
                {
                    sp.Play("띵동");
                }
            }
            else
            {
                ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                {
                    Status = response.Status,
                    Title = response.Title,
                    Message = response.Message
                };
                App.MainFrame.Navigate(new ErrorDisplayView(vm));

                NarrationPlayer sp = new NarrationPlayer();
                sp.Play("띵동");
            }
        }

        private void Api_ResponseFailed(object sender, HttpHelperEventArgs e)
        {
            FoodTicketCheckApi apiObj = sender as FoodTicketCheckApi;
            FoodTicketCheckApiRequest request = apiObj.HttpApiRequest as FoodTicketCheckApiRequest;
            FoodTicketCheckApiResponse response = apiObj.HttpApiResponse as FoodTicketCheckApiResponse;

            if (e == null)
            {
                // TimeOut;
            }
            else if (e.ExceptionObj is System.Net.WebException)
            {
                ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                {
                    Status = ApiStatus.NETWORK_ERROR,
                    Title = "네트워크 에러",
                    Message = e.ExceptionObj.Message
                };
                App.MainFrame.Navigate(new ErrorDisplayView(vm));
            }
            else if (e.ExceptionObj is System.Net.Sockets.SocketException)
            {
                ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                {
                    Status = ApiStatus.NETWORK_ERROR,
                    Title = "네트워크 에러",
                    Message = e.ExceptionObj.Message
                };
                App.MainFrame.Navigate(new ErrorDisplayView(vm));
            }
            else if (e.ExceptionObj is TimeoutException)
            {
                // TimeOut;
            }
        }
    }
}