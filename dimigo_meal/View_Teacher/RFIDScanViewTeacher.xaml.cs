using dimigo_meal.Common;
using dimigo_meal.MyAPI.RESTAPI;
using MyAPI.RESTAPI;
using System;

namespace dimigo_meal.View
{
    /// <summary>
    /// RFIDScanViewWithButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RFIDScanViewTeacher : RFIDReaderViewBaseEx
    {
        public RFIDScanViewTeacher()
        {
            InitializeComponent();
        }

        protected override void RFIDCode_Received(string RFIDCode)
        {
            FoodTicketTeacherApiRequest request = new FoodTicketTeacherApiRequest
            {
                RFIDCode = RFIDCode,
                TimeStamp = (DateTime.Now - DateTime.Parse("1970-01-01 09:00:00")).TotalSeconds,
                MealCount = App.mc
            };
            FoodTicketTeacherApi api = new FoodTicketTeacherApi();
            api.ResponseSucceeded += base.Api_ResponseSucceeded;
            api.ResponseFailed += base.Api_ResponseFailed;
            api.Send(request);

        }

        private void btn_Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //wow
            //App.GoHomeCommand.Execute(null);
            App.MainWindow.MainWindowViewState = MainWindowViewState.MAIN_VIEW_TEACHER;
        }
    }
}
