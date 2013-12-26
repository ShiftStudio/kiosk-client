
using MyAPI.Model;
//#define DEBUG
using MyAPI.RESTAPI;
using MyBaseLib.Diagnostics;
using MyBaseLib.Network;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace dimigo_meal.View
{
    /// <summary>
    /// MainWindowView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindowView : Window
    {

        #region Constructor

        public MainWindowView()
        {
            InitializeComponent();

            Screen secondaryScreen = this.GetSecondaryScreen();
            this.Left = secondaryScreen.Bounds.Left;
            this.Top = secondaryScreen.Bounds.Top;

            MainWindowViewModel viewModel = new MainWindowViewModel();
            this.ViewModel = viewModel;

            MainVideoPlayer.MovieDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "mov");
            MainVideoPlayer.Play();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += _timer_Tick;
            _timer.Start();

            _worker = new BackgroundWorker();
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerAsync();

            #if DEBUG
            FoodTicketCheckApiResponse sample = MainWindowViewModel.getSampleData();
            this.ViewModel.MealData = sample.Meal.MealData;
            this.ViewModel.MealState = sample.Meal.MealState;
            this.KeyUp += Grid_KeyUp_1;
            #endif
        }
        
        #endregion Constructor

        #region Properties

        public MainWindowViewModel ViewModel
        {
            get
            {
                return this.DataContext as MainWindowViewModel;
            }
            set
            {
                if (this.DataContext != value)
                {
                    this.DataContext = value;
                }
            }
        }
        
        public MainWindowViewState MainWindowViewState
        {
            get
            {
                return _mainWindowViewState;
            }
            set
            {
                if (_mainWindowViewState != value)
                {
                    _mainWindowViewState = value;
                    MainWindowView_StateChanged(_mainWindowViewState);
                }
            }
        }

        public Uri HomePageUri { get; set; }

        #endregion Properties

        #region Event

        private void _timer_Tick(object sender, EventArgs e)
        {
            MainWindowViewModel viewModel = this.ViewModel;
            viewModel.Now = DateTime.Now;

            this.MainWindowViewState = MainWindowViewState.RFIDSCAN_VIEW;

            //return;
            if (viewModel.MealData.MealStartTime <= viewModel.Now && viewModel.Now <= viewModel.MealData.MealStopTime)
            {
                if (viewModel.MealData.MealSupplyStartTime <= viewModel.Now && viewModel.Now <= viewModel.MealData.MealSupplyStopTime)
                {
                    if (viewModel.MealData.IsUsableRFIDCard)
                    {
                        //식권에 학생증을 사용할 수 있을때
                        this.MainWindowViewState = MainWindowViewState.RFIDSCAN_VIEW;
                    }
                    else
                    {
                        //식권에 학생증을 사용할 수 없을때
                        this.MainWindowViewState = MainWindowViewState.NOT_RFIDSCAN_VIEW;
                    }
                }
                else
                {
                    //식사시간이 아닐 때
                    this.MainWindowViewState = MainWindowViewState.NOT_MEAL_SUPPLY_TIME_VIEW;
                }
            }
            else
            {
                //기타
                this.MainWindowViewState = MainWindowViewState.NORMAL_VIEW;
            }
        }

        private void MainWindowView_StateChanged(MainWindowViewState ViewState)
        {
            switch (ViewState)
            {
                case MainWindowViewState.NORMAL_VIEW:
                    this.MainHeader.Visibility = Visibility.Visible;
                    this.SubHeader3.Visibility = Visibility.Visible;
                    this.HomePageUri = new Uri("View/NormalView.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
                case MainWindowViewState.NOT_MEAL_SUPPLY_TIME_VIEW:
                    this.MainHeader.Visibility = Visibility.Visible;
                    this.SubHeader3.Visibility = Visibility.Visible; // 쓸떄도, 안쓸떄도...
                    this.HomePageUri = new Uri("View/NotMealSupplyTimeView.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
                case MainWindowViewState.NOT_RFIDSCAN_VIEW:
                    this.MainHeader.Visibility = Visibility.Collapsed;
                    this.HomePageUri = new Uri("View/NotRFIDScanView.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
                case MainWindowViewState.RFIDSCAN_VIEW:
                    this.MainHeader.Visibility = Visibility.Visible;
                    this.SubHeader3.Visibility = Visibility.Visible; // 쓸떄도, 안쓸떄도...
                    this.HomePageUri = new Uri("View/RFIDScanView.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
            }
        }

        private void NewDataCheckApi_ResponseSucceeded(object sender, HttpApiResponseBase e)
        {
            NewDataCheckApi apiObj = sender as NewDataCheckApi;
            NewDataCheckApiRequest request = apiObj.HttpApiRequest as NewDataCheckApiRequest;
            NewDataCheckApiResponse response = apiObj.HttpApiResponse as NewDataCheckApiResponse;

            if (response.Status >= 0)
            {
                App.MainWindow.ViewModel.MealState = response.Meal.MealState;
                App.MainWindow.ViewModel.MealData = response.Meal.MealData;
            }
        }

        private void NewDataCheckApi_ResponseFailed(object sender, HttpHelperEventArgs e)
        {
            NewDataCheckApi apiObj = sender as NewDataCheckApi;
            NewDataCheckApiRequest request = apiObj.HttpApiRequest as NewDataCheckApiRequest;
            NewDataCheckApiResponse response = apiObj.HttpApiResponse as NewDataCheckApiResponse;

            if (e == null)
            {
                // TimeOut;
            }
            else if (e.ExceptionObj is System.Net.WebException)
            {
                // 네트워크 에러
            }
            else if (e.ExceptionObj is System.Net.Sockets.SocketException)
            {
                // 네트워크 에러
            }
            else if (e.ExceptionObj is TimeoutException)
            {
                // TimeOut;
            }
        }

        private void foodTicketCheckApi_ResponseSucceeded(object sender, HttpApiResponseBase e)
        {
            FoodTicketCheckApi apiObj = sender as FoodTicketCheckApi;
            FoodTicketCheckApiResponse response = apiObj.HttpApiResponse as FoodTicketCheckApiResponse;

            this.NavigateResultDisplayView(response);
        }

        private void foodTicketCheckApi_ResponseFailed(object sender, HttpHelperEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (this._rfidCodeBuffer.Length != 10)
                {
                    DebugEx.WriteLine("Key Input detected but not considered as RFID code");
                }
                else if (this.MainWindowViewState != View.MainWindowViewState.RFIDSCAN_VIEW)
                {
                    DebugEx.WriteLine("RFID scan not needed");
                }
                else
                {
                    if (foodTicketCheckApi != null)
                    {
                        foodTicketCheckApi.ResponseSucceeded -= this.foodTicketCheckApi_ResponseSucceeded;
                        foodTicketCheckApi.ResponseFailed -= this.foodTicketCheckApi_ResponseFailed;
                        foodTicketCheckApi.Cancel();
                    }

                    //FoodCheck API Call
                    FoodTicketCheckApiRequest request = new FoodTicketCheckApiRequest()
                    {
                        TimeStamp = (DateTime.Now - DateTime.Parse("1970-01-01 09:00:00")).TotalSeconds,
                        RFIDCode = this._rfidCodeBuffer
                    };

                    foodTicketCheckApi = new FoodTicketCheckApi();
                    foodTicketCheckApi.ResponseSucceeded += this.foodTicketCheckApi_ResponseSucceeded;
                    foodTicketCheckApi.ResponseFailed += this.foodTicketCheckApi_ResponseFailed;
                    foodTicketCheckApi.Send(request);

                    this._rfidCodeBuffer = string.Empty;
                }
            }
            else
            {
                string rawKey = e.Key.ToString().Replace("D", "").Replace("NumPad", "");
                int i;
                if (int.TryParse(rawKey, out i))
                {
                    this._rfidCodeBuffer += i.ToString();
                }
            }
        }

        #endregion Event

        #region Method

        private Screen GetSecondaryScreen()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen != Screen.PrimaryScreen)
                    return screen;
            }
            return Screen.PrimaryScreen;
        }

        public bool Navigate(object content)
        {
            return this.MainFrame.Navigate(content);
        }

        public bool Navigate(Uri source)
        {
            return this.MainFrame.Navigate(source);
        }

        public bool Navigate(object content, object extraData)
        {
            return this.MainFrame.Navigate(content, extraData);
        }

        public bool Navigate(Uri source, object extraData)
        {
            return this.MainFrame.Navigate(source, extraData);
        }

        #endregion Method

        #region Field

        private MainWindowViewState _mainWindowViewState = MainWindowViewState.NORMAL_VIEW;

        private BackgroundWorker _worker;

        private NewDataCheckApi newDataCheckApi = null;

        private FoodTicketCheckApi foodTicketCheckApi = null;

        private DispatcherTimer _timer;

        private String _rfidCodeBuffer = String.Empty;

        #endregion Field

        #region BackgroundWorker
        
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (newDataCheckApi != null)
                {
                    newDataCheckApi.ResponseSucceeded -= this.NewDataCheckApi_ResponseSucceeded;
                    newDataCheckApi.ResponseFailed -= this.NewDataCheckApi_ResponseFailed;
                    newDataCheckApi.Cancel();
                }

                NewDataCheckApiRequest request = new NewDataCheckApiRequest
                {
                    TimeStamp = (DateTime.Now - DateTime.Parse("1970-01-01 09:00:00")).TotalSeconds
                };

                newDataCheckApi = new NewDataCheckApi();
                newDataCheckApi.ResponseSucceeded += this.NewDataCheckApi_ResponseSucceeded;
                newDataCheckApi.ResponseFailed += this.NewDataCheckApi_ResponseFailed;
                newDataCheckApi.Send(request);

                Thread.Sleep(1000);
            }
        }
        
        #endregion BackgroundWorker



        #region DEBUG CODE
        #if DEBUG

        int pageIndex = 0;

        private void Grid_KeyUp_1(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Q)
            {
                //App.MainWindow.Wrapper.SetValue(System.Windows.Controls.Grid.RowProperty, 0);
                //App.MainWindow.Wrapper.SetValue(System.Windows.Controls.Grid.RowSpanProperty, 2);
                switch (++pageIndex)
                {
                    case 0:
                        _timer.Start();
                        App.MainWindow.Header.Visibility = Visibility.Visible;
                        App.MainWindow.VideoContainer.Visibility = Visibility.Visible;
                        App.MainWindow.MainWindowViewState = View.MainWindowViewState.NORMAL_VIEW;
                        break;
                    case 1:
                        _timer.Start();
                        App.MainWindow.Header.Visibility = Visibility.Visible;
                        App.MainWindow.VideoContainer.Visibility = Visibility.Collapsed;
                        App.MainWindow.MainWindowViewState = View.MainWindowViewState.NORMAL_VIEW;
                        break;
                    default:
                        _timer.Stop();
                        App.MainWindow.Header.Visibility = Visibility.Collapsed;
                        App.MainWindow.VideoContainer.Visibility = Visibility.Collapsed;

                        App.MainWindow.MainWindowViewState = (MainWindowViewState)(pageIndex);
                        if (pageIndex >= 5)
                        {
                            FoodTicketCheckApiResponse response = ResultDisplayViewModel.getSampleData();
                            switch (pageIndex)
                            {
                                case 5:
                                    response.Status = FoodTicketCheckApiStatus.SUCCESS;
                                    response.Event.Status = clsEventStatus.SUCCESS;
                                    response.Event.Message = "급식을 먹을 수 있습니다.";
                                    break;
                                case 6:
                                    response.Status = FoodTicketCheckApiStatus.SUCCESS;
                                    response.Event.Status = clsEventStatus.BANNED;
                                    response.Event.Message = "이미 급식을 먹었습니다. ㅗㅗ";
                                    break;
                                case 7:
                                    pageIndex = -1;
                                    response.Status = FoodTicketCheckApiStatus.UNKNOWN_ERROR;
                                    response.Title = "알수없는 에러";
                                    response.Message = "에러 ㅜㅜ";
                                    break;
                            }
                            NavigateResultDisplayView(response);
                        }
                        break;
                }
            }
        }
        #endif
        #endregion DEBUG CODE

        private void NavigateResultDisplayView(FoodTicketCheckApiResponse response)
        {
            if (response.Status >= 0)
            {
                ResultDisplayViewModel vm = new ResultDisplayViewModel()
                {
                    User = response.User,
                    Event = response.Event
                };
                App.MainFrame.Navigate(new ResultDisplayView(vm));

                NarrationPlayer sp = new NarrationPlayer();
                switch (response.Status)
                {
                    case FoodTicketCheckApiStatus.SUCCESS:
                        sp.Play("띵동");
                        break;
                    case FoodTicketCheckApiStatus.BANNED:
                        sp.Play("띵동");
                        break;
                    case FoodTicketCheckApiStatus.UNKNOWN_ERROR:
                        sp.Play("띵동");
                        break;
                    default:
                        sp.Play("띵동");
                        break;
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


    }
}