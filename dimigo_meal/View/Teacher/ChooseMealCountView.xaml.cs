using dimigo_meal.Model;
using System.Windows;
using System.Windows.Controls;

namespace dimigo_meal.View.Teacher
{
    /// <summary>
    /// ChooseMealCountView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChooseMealCountView : Page
    {
        public ChooseMealCountView()
        {
            InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            ViewStateManager.NavigateHome();
        }

        private void btn_Okay_Click(object sender, RoutedEventArgs e)
        {
            Teacher.RFIDScanViewModel vm = new Teacher.RFIDScanViewModel()
            {
                MealCount = (sender as Button).Content.ToString()
            };
            ViewStateManager.Navigate(new Teacher.RFIDScanView(vm));
        }
    }
}
