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
            //App.GoHomeCommand.Execute(null);
            App.MainWindow.MainWindowViewState = MainWindowViewState.MAIN_VIEW_TEACHER;
        }

        private void btn_Okay_Click(object sender, RoutedEventArgs e)
        {
            //꼭 이렇게 써야돼?
            App.mc = (sender as Button).Content.ToString();
            
            App.MainWindow.MainWindowViewState = MainWindowViewState.RFIDSCAN_VIEW_TEACHER;
        }
    }
}
