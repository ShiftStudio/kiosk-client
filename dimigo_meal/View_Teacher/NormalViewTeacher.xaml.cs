using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dimigo_meal.View
{
    /// <summary>
    /// NormalViewTeacher.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NormalViewTeacher : Page
    {
        public NormalViewTeacher()
        {
            InitializeComponent();
        }

        private void btn_CheckTeacherMeal_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainWindowViewState = MainWindowViewState.MEAL_COUNTCH_VIEW;
        }

        private void btn_AddNewStudentMeal_Click(object sender, RoutedEventArgs e)
        {
            //if(App.MainWindow.ViewModel.
            App.MainWindow.MainWindowViewState = MainWindowViewState.RFIDSCAN_VIEW_STUDENT;
        }

    }
}
