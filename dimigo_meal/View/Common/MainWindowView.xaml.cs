using dimigo_meal.Model;
using MyAPI.RESTAPI;
using System.Windows;

namespace dimigo_meal.View.Common
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

            MainWindowViewModel viewModel = new MainWindowViewModel();
            this.ViewModel = viewModel;
            #if DEBUG
            NewDataCheckApiResponse sample = MainWindowViewModel.getSampleData();
            this.ViewModel.MealData = sample.Meal.MealData;
            this.ViewModel.MealState = sample.Meal.MealState;
            this.KeyUp += ViewStateManager.ViewChangeFromKeyPress;
            #endif

            MainVideoPlayer.MovieDirectory = GlobalSettings.MovieDirectory;
            MainVideoPlayer.Play();

            //send it to second screen
            System.Windows.Forms.Screen secondaryScreen = ViewStateManager.GetSecondaryScreen();
            this.Left = secondaryScreen.Bounds.Left;
            this.Top = secondaryScreen.Bounds.Top;
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

        #endregion Properties
    }
}