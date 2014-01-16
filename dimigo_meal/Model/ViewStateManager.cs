using dimigo_meal.View;
using dimigo_meal.View.Common;
using MyAPI.Model;
using MyAPI.RESTAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace dimigo_meal.Model
{
    public static class ViewStateManager
    {
        #region Field

        static private DispatcherTimer _timer;

        static private MainWindowViewState _mainWindowViewState;

        #endregion Field

        #region Property

        static public MainWindowViewState MainWindowViewState
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

        static public Uri HomePageUri { get; set; }

        static public MainWindowView MainWindow { get; set; }

        #endregion Property

        #region Method

        static public void Start()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(800);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        static public bool Navigate(object content)
        {
            return ViewStateManager.MainWindow.MainFrame.Navigate(content);
        }

        static public bool Navigate(Uri source)
        {
            return ViewStateManager.MainWindow.MainFrame.Navigate(source);
        }

        static public bool Navigate(object content, object extraData)
        {
            return ViewStateManager.MainWindow.MainFrame.Navigate(content, extraData);
        }

        static public bool Navigate(Uri source, object extraData)
        {
            return ViewStateManager.MainWindow.MainFrame.Navigate(source, extraData);
        }

        static public bool NavigateHome()
        {
            if (ViewStateManager.HomePageUri != null)
            {
                return ViewStateManager.Navigate(ViewStateManager.HomePageUri);
            }
            return false;
        }

        static private bool CheckMealTime(DateTime dateTime)
        {
            if (ViewStateManager.MainWindow.ViewModel != null && ViewStateManager.MainWindow.ViewModel.MealData != null)
            {
                return ViewStateManager.MainWindow.ViewModel.MealData.MealStartTime <= dateTime && dateTime <= ViewStateManager.MainWindow.ViewModel.MealData.MealStopTime;
            }
            return false;
        }

        static public System.Windows.Forms.Screen GetSecondaryScreen()
        {
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                if (screen != System.Windows.Forms.Screen.PrimaryScreen)
                    return screen;
            }
            return System.Windows.Forms.Screen.PrimaryScreen;
        }

        #endregion Method

        #region Event

        static private void MainWindowView_StateChanged(MainWindowViewState ViewState)
        {
            switch (ViewState)
            {
                case MainWindowViewState.NORMAL_VIEW:
                    ViewStateManager.MainWindow.MainHeader.Visibility = Visibility.Collapsed;
                    ViewStateManager.HomePageUri = new Uri("View/Common/NormalView.xaml", UriKind.Relative);
                    ViewStateManager.Navigate(ViewStateManager.HomePageUri);
                    break;
                case MainWindowViewState.NOT_MEAL_SUPPLY_TIME_VIEW:
                    ViewStateManager.MainWindow.MainHeader.Visibility = Visibility.Visible;
                    ViewStateManager.MainWindow.SubHeader3.Visibility = Visibility.Visible;
                    ViewStateManager.HomePageUri = new Uri("View/Common/NotMealSupplyTimeView.xaml", UriKind.Relative);
                    ViewStateManager.Navigate(ViewStateManager.HomePageUri);
                    break;
                case MainWindowViewState.NOT_RFIDSCAN_VIEW:
                    ViewStateManager.MainWindow.MainHeader.Visibility = Visibility.Visible;
                    ViewStateManager.MainWindow.SubHeader3.Visibility = Visibility.Visible;
                    ViewStateManager.HomePageUri = new Uri("View/Student/NotRFIDScanView.xaml", UriKind.Relative);
                    ViewStateManager.Navigate(ViewStateManager.HomePageUri);
                    break;
                case MainWindowViewState.RFIDSCAN_VIEW_STUDENT:
                    ViewStateManager.MainWindow.MainHeader.Visibility = Visibility.Visible;
                    ViewStateManager.MainWindow.SubHeader3.Visibility = Visibility.Visible;
                    ViewStateManager.HomePageUri = new Uri("View/Student/RFIDScanView.xaml", UriKind.Relative);
                    ViewStateManager.Navigate(ViewStateManager.HomePageUri);
                    break;
                case MainWindowViewState.RFIDSCAN_VIEW_TEACHER:
                    ViewStateManager.MainWindow.MainHeader.Visibility = Visibility.Visible;
                    ViewStateManager.MainWindow.SubHeader3.Visibility = Visibility.Visible;
                    ViewStateManager.HomePageUri = new Uri("View/Teacher/RFIDScanView.xaml", UriKind.Relative);
                    ViewStateManager.Navigate(ViewStateManager.HomePageUri);
                    break;
                case MainWindowViewState.MAIN_VIEW_TEACHER:
                    ViewStateManager.MainWindow.MainHeader.Visibility = Visibility.Visible;
                    ViewStateManager.MainWindow.SubHeader3.Visibility = Visibility.Visible;
                    ViewStateManager.HomePageUri = new Uri("View/Teacher/NormalView.xaml", UriKind.Relative);
                    ViewStateManager.Navigate(ViewStateManager.HomePageUri);
                    break;
                case MainWindowViewState.MEAL_COUNTCH_VIEW:
                    ViewStateManager.MainWindow.MainHeader.Visibility = Visibility.Visible;
                    ViewStateManager.MainWindow.SubHeader3.Visibility = Visibility.Visible;
                    ViewStateManager.HomePageUri = new Uri("View/Teacher/ChooseMealCountView.xaml", UriKind.Relative);
                    ViewStateManager.Navigate(ViewStateManager.HomePageUri);
                    break;
            }
        }

        static private void Timer_Tick(object sender, EventArgs e)
        {
            MainWindowViewModel viewModel = ViewStateManager.MainWindow.ViewModel;
            viewModel.Now = DateTime.Now;

#if DEBUG
            switch (GlobalSettings.KioskViewMode)
            {
                case ViewMode.TEACHER_KIOSK:
                    ViewStateManager.MainWindowViewState = MainWindowViewState.MAIN_VIEW_TEACHER;
                    break;
                case ViewMode.STUDENT_KIOSK:
                    ViewStateManager.MainWindowViewState = MainWindowViewState.RFIDSCAN_VIEW_STUDENT;
                    break;
            }

            return;
#endif

            switch (GlobalSettings.KioskViewMode)
            {
                case ViewMode.TEACHER_KIOSK:
                    if (CheckMealTime(viewModel.Now))
                    {
                        ViewStateManager.MainWindowViewState = MainWindowViewState.MAIN_VIEW_TEACHER;
                    }
                    else
                    {
                        ViewStateManager.MainWindowViewState = MainWindowViewState.NORMAL_VIEW;
                    }
                    break;
                case ViewMode.STUDENT_KIOSK:
                    if (CheckMealTime(viewModel.Now))
                    {
                        if (viewModel.MealData.IsUsableRFIDCard)
                        {
                            ViewStateManager.MainWindowViewState = MainWindowViewState.RFIDSCAN_VIEW_STUDENT;
                        }
                        else
                        {
                            ViewStateManager.MainWindowViewState = MainWindowViewState.NOT_RFIDSCAN_VIEW;
                        }
                    }
                    else
                    {
                        ViewStateManager.MainWindowViewState = MainWindowViewState.NORMAL_VIEW;
                    }
                    break;
            }
        }

        static public void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                {
                    Status = clsEventStatus.UNKNOWN_ERROR,
                    Title = "An unhandled exception occurred",
                    Message = e.ExceptionObject.ToString()
                };
                ViewStateManager.Navigate(new ErrorDisplayView(vm));
            }
            catch
            {
                string errorMessage = string.Format("An unhandled exception occurred: {0}", e.ExceptionObject.ToString());
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion Event

        #region DEBUG CODE
#if DEBUG

        static int pageIndex = 0;

        static public void ViewChangeFromKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Q)
            {
                //ViewStateManager.MainWindow.Wrapper.SetValue(System.Windows.Controls.Grid.RowProperty, 0);
                //ViewStateManager.MainWindow.Wrapper.SetValue(System.Windows.Controls.Grid.RowSpanProperty, 2);
                switch (++pageIndex)
                {
                    case 0:
                        _timer.Start();
                        ViewStateManager.MainWindow.Header.Visibility = Visibility.Visible;
                        ViewStateManager.MainWindow.VideoContainer.Visibility = Visibility.Visible;
                        ViewStateManager.MainWindowViewState = MainWindowViewState.NORMAL_VIEW;
                        break;
                    case 1:
                        _timer.Stop();
                        ViewStateManager.MainWindow.Header.Visibility = Visibility.Visible;
                        //                        ViewStateManager.MainWindow.VideoContainer.Visibility = Visibility.Collapsed;
                        ViewStateManager.MainWindowViewState = MainWindowViewState.NORMAL_VIEW;
                        break;
                    default:
                        _timer.Stop();
                        //ViewStateManager.MainWindow.Header.Visibility = Visibility.Collapsed;
                        //ViewStateManager.MainWindow.VideoContainer.Visibility = Visibility.Collapsed;

                        ViewStateManager.MainWindowViewState = (MainWindowViewState)(pageIndex);
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

        static private void NavigateResultDisplayView(FoodTicketCheckApiResponse response)
        {
            if (response.Status >= 0)
            {
                ResultDisplayViewModel vm = new ResultDisplayViewModel()
                {
                    User = response.User,
                    Event = response.Event
                };
                ViewStateManager.Navigate(new ResultDisplayView(vm));

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
                ViewStateManager.Navigate(new ErrorDisplayView(vm));

                NarrationPlayer sp = new NarrationPlayer();
                sp.Play("띵동");
            }
        }

#endif
        #endregion DEBUG CODE
    }
}
