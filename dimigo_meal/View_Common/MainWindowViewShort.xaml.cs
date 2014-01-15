
using MyAPI.Model;
//#define DEBUG
using MyAPI.RESTAPI;
using MyBaseLib.Diagnostics;
using MyBaseLib.Network;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace dimigo_meal.View
{
    /// <summary>
    /// MainWindowView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindowViewShort : Window
    {

        #region Constructor

        public MainWindowViewShort()
        {
            InitializeComponent();
#if DEBUG
            //send it to second screen
            System.Windows.Forms.Screen secondaryScreen = this.GetSecondaryScreen();
            this.Left = secondaryScreen.Bounds.Left;
            this.Top = secondaryScreen.Bounds.Top;
#endif
            Brush _brush = null;

            if (App.KioskViewMode == ViewMode.TEACHER_KIOSK)
            {
                this.Background.ImageSource = (ImageSource)new ImageSourceConverter()
                .ConvertFromString(@"pack://application:,,,/Assets/Resources/new/t/background_static.png");
            
                _brush = new SolidColorBrush(Color.FromArgb(255, 177, 190, 226));
            }
            else if (App.KioskViewMode == ViewMode.STUDENT_KIOSK)
            {
                this.Background.ImageSource = (ImageSource)new ImageSourceConverter()
                .ConvertFromString(@"pack://application:,,,/Assets/Resources/new/s/background_static.png");
            
               _brush =  new SolidColorBrush(Color.FromArgb(255, 243, 191, 195));
            }

            l1.Fill = _brush;
            l2.Fill = _brush;

            MainWindowViewModel viewModel = new MainWindowViewModel();
            this.ViewModel = viewModel;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += _timer_Tick;
            _timer.Start();
            /*new Thread(new ThreadStart(() =>
            {
                throw new Exception();
            })).Start();
            */
            //_worker = new BackgroundWorker();
            //_worker.DoWork += worker_DoWork;
            //_worker.RunWorkerAsync();
            _backgroundThread = new Thread(new ThreadStart(this.worker_DoWork));
            _backgroundThread.Start();

            #if DEBUG
            NewDataCheckApiResponse sample = MainWindowViewModel.getSampleData();
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

        //timer for refreshing view
        private void _timer_Tick(object sender, EventArgs e)
        {
            MainWindowViewModel viewModel = this.ViewModel;
            viewModel.Now = DateTime.Now;


            if (viewModel.MealData != null)
            {

                if (viewModel.MealData.IsServing == false)
                {
                    this.MainWindowViewState = MainWindowViewState.NORMAL_VIEW;
                }
                else
                {
                    if (App.KioskViewMode == ViewMode.STUDENT_KIOSK)
                        this.MainWindowViewState = MainWindowViewState.RFIDSCAN_VIEW_STUDENT;
                    else if (App.KioskViewMode == ViewMode.TEACHER_KIOSK &&
                                (this.MainWindowViewState == MainWindowViewState.NOT_MEAL_SUPPLY_TIME_VIEW ||
                                this.MainWindowViewState == MainWindowViewState.NORMAL_VIEW))
                    {
                        this.MainWindowViewState = MainWindowViewState.MAIN_VIEW_TEACHER;
                    }

                    //학생 미급식 경우 처리
                    if (viewModel.MealData.IsUsableRFIDCard == false)
                    {
                        this.SubHeader3.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this.SubHeader3.Visibility = Visibility.Visible;
                    }
                }

            }
            else
            {
                this.MainWindowViewState = MainWindowViewState.NORMAL_VIEW;
            }

            return;
            
        }

        private void MainWindowView_StateChanged(MainWindowViewState ViewState)
        {
            switch (ViewState)
            {
                case MainWindowViewState.NORMAL_VIEW:
                    this.MainHeader.Visibility = Visibility.Collapsed;
                    this.HomePageUri = new Uri("View_Common/NormalView.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
                case MainWindowViewState.NOT_MEAL_SUPPLY_TIME_VIEW:
                    this.MainHeader.Visibility = Visibility.Visible;
                    this.SubHeader3.Visibility = Visibility.Visible;
                    this.HomePageUri = new Uri("View_Common/NotMealSupplyTimeView.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
                case MainWindowViewState.NOT_RFIDSCAN_VIEW:
                    this.MainHeader.Visibility = Visibility.Visible;
                    this.SubHeader3.Visibility = Visibility.Visible;
                    this.HomePageUri = new Uri("View_Student/NotRFIDScanView.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
                case MainWindowViewState.RFIDSCAN_VIEW_STUDENT:
                    this.MainHeader.Visibility = Visibility.Visible;
                    this.SubHeader3.Visibility = Visibility.Visible;
                    this.HomePageUri = new Uri("View_Student/RFIDScanViewStudent.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
                case MainWindowViewState.RFIDSCAN_VIEW_TEACHER:
                    this.MainHeader.Visibility = Visibility.Visible;
                    this.SubHeader3.Visibility = Visibility.Visible;
                    this.HomePageUri = new Uri("View_Teacher/RFIDScanViewTeacher.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
                case MainWindowViewState.MAIN_VIEW_TEACHER:
                    this.MainHeader.Visibility = Visibility.Visible;
                    this.SubHeader3.Visibility = Visibility.Visible;
                    this.HomePageUri = new Uri("View_Teacher/NormalViewTeacher.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
                case MainWindowViewState.MEAL_COUNTCH_VIEW:
                    this.MainHeader.Visibility = Visibility.Visible;
                    this.SubHeader3.Visibility = Visibility.Visible;
                    this.HomePageUri = new Uri("View_Teacher/ChooseMealCountView.xaml", UriKind.Relative);
                    this.Navigate(this.HomePageUri);
                    break;
            }
        }

        private void NewDataCheckApi_ResponseSucceeded(object sender, HttpApiResponseBase e)
        {
            NewDataCheckApi apiObj = sender as NewDataCheckApi;
            NewDataCheckApiResponse response = apiObj.HttpApiResponse as NewDataCheckApiResponse;

                //Used delegate to prevent thread conflict
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (response.Meal != null)
                    {
                        App.MainWindow.ViewModel.MealState = response.Meal.MealState;
                        App.MainWindow.ViewModel.MealData = response.Meal.MealData;
                    }
                }));
            
        }

        private void NewDataCheckApi_ResponseFailed(object sender, HttpHelperEventArgs e)
        {
            NewDataCheckApi apiObj = sender as NewDataCheckApi;
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

        #endregion Event

        #region Method

        private System.Windows.Forms.Screen GetSecondaryScreen()
        {
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                if (screen != System.Windows.Forms.Screen.PrimaryScreen)
                    return screen;
            }
            return System.Windows.Forms.Screen.PrimaryScreen;
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

        private MainWindowViewState _mainWindowViewState;

        //private BackgroundWorker _worker;

        private NewDataCheckApi newDataCheckApi = null;

        private FoodTicketStudentApi foodTicketCheckApi = null;

        private DispatcherTimer _timer;

        private String _rfidCodeBuffer = String.Empty;

        private Thread _backgroundThread;

        #endregion Field

        #region BackgroundWorker
        
        private void worker_DoWork()
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

                //1초마다 한번씩은 너무 심하잖아
                Thread.Sleep(8000);
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
                switch (++pageIndex)
                {
                    case 0:
                        _timer.Start();
                        App.MainWindow.Header.Visibility = Visibility.Visible;
                        //App.MainWindow.VideoContainer.Visibility = Visibility.Visible;
                        App.MainWindow.MainWindowViewState = View.MainWindowViewState.NORMAL_VIEW;
                        break;
                    case 1:
                        _timer.Stop();
                        App.MainWindow.Header.Visibility = Visibility.Visible;
//                        App.MainWindow.VideoContainer.Visibility = Visibility.Collapsed;
                        App.MainWindow.MainWindowViewState = View.MainWindowViewState.NORMAL_VIEW;
                        break;
                    default:
                        _timer.Stop();
//                        App.MainWindow.Header.Visibility = Visibility.Collapsed;
//                        App.MainWindow.VideoContainer.Visibility = Visibility.Collapsed;

                        App.MainWindow.MainWindowViewState = (MainWindowViewState)(pageIndex);
                        if (pageIndex > 8)
                        {
                            FoodTicketCheckApiResponse response = ResultDisplayViewModel.getSampleData();
                            switch (pageIndex)
                            {
                                case 9:
                                    response.Status = ApiStatus.SUCCESS;
                                    response.Event.Status = clsEventStatus.SUCCESS;
                                    response.Event.Message = "급식을 먹을 수 있습니다.";
                                    break;
                                case 10:
                                    response.Status = ApiStatus.SUCCESS;
                                    response.Event.Status = clsEventStatus.BANNED;
                                    response.Event.Message = "이미 급식을 먹었습니다. ㅗㅗ";
                                    break;
                                case 11:
                                    response.Status = ApiStatus.SUCCESS;
                                    response.Event.Status = clsEventStatus.INVALID_USER;
                                    response.Event.Message = "확인되지 않는 학생증입니다.";
                                    break;
                                case 12:
                                    response.Status = ApiStatus.UNKNOWN_ERROR;
                                    response.Title = "알수없는 에러";
                                    response.Message = "에러 ㅜㅜ";
                                    break;
                                case 13:
                                    pageIndex = -1;
                                    response.Status = ApiStatus.NETWORK_ERROR;
                                    response.Title = "네트워크 에러";
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
                    /*case ApiStatus.SUCCESS:
                        sp.Play("띵동");
                        break;
                    case ApiStatus.NETWORK_ERROR:
                        sp.Play("띵동");
                        break;
                    case ApiStatus.UNKNOWN_ERROR:
                        sp.Play("띵동");
                        break;*/
                    default:
                        sp.Play("띵동");
                        break;
                }
            }
            else
            {
                ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                {
                    Status = response.Event.Status,
                    Title = response.Title,
                    Message = response.Message
                };
                App.MainFrame.Navigate(new ErrorDisplayView(vm));

                NarrationPlayer sp = new NarrationPlayer();
                //sp.Play("띵동");
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _backgroundThread.Abort();
        }

    }
}