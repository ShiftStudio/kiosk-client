using dimigo_meal.Model;
using System.Windows;
using System.Windows.Controls;

namespace dimigo_meal.View.Teacher
{
    /// <summary>
    /// NormalView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NormalView : Page
    {
        public NormalView()
        {
            InitializeComponent();
        }

        private void btn_CheckTeacherMeal_Click(object sender, RoutedEventArgs e)
        {
            ViewStateManager.Navigate(new ChooseMealCountView());
        }

        private void btn_AddNewStudentMeal_Click(object sender, RoutedEventArgs e)
        {
            ViewStateManager.Navigate(new Student.RFIDScanView());
        }
    }
}
