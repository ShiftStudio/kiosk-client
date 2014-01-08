using dimigo_meal.Common;
using MyAPI.Model;
using MyAPI.RESTAPI;
using System.Windows;
using System.Windows.Media;

namespace dimigo_meal.View
{
    public class ErrorDisplayViewModel : ViewModelBase
    {
        #region Constructor

        public ErrorDisplayViewModel()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                FoodTicketCheckApiResponse sample = ErrorDisplayViewModel.getSampleData();
                this.Status = sample.Event.Status;
            }
        }

        public static FoodTicketCheckApiResponse getSampleData()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<FoodTicketCheckApiResponse>("{\"status\" : 0, \"user\" :{\"name\" : \"김동현\", \"department\" : \"개발3팀장\", \"grade\" : 2, \"class\" : 4, \"number\" : 4, \"profileUrl\" : \"\"}, \"event\" :{\"regularTime\" : \"17:00:00\", \"checkoutTime\" : \"17:10:00\", \"diffTime\" : \"00:10:00\"}, \"meal\" :{\"mealData\" :{\"isUsableRFIDCard\" : false, \"mealTime\" : \"저녁\", \"mealStartTime\" : \"18:20:00\", \"mealStopTime\" : \"19:30:00\", \"mealSupplyStartTime\" : \"18:20:00\", \"mealSupplyStopTime\" : \"19:15:00\", \"mealInstanceStartTime\" : \"19:10:00\", \"mealInstanceCouponNum\" : 30, \"mealName\" : \"자장면 데이\", \"foodList\" :[{\"id\" : 1, \"name\" : \"코다리 튀김\", \"isSpecial\" : true,  \"isAllergy\" : false},{\"id\" : 2, \"name\" : \"짜장 라이스\", \"isSpecial\" : false, \"isAllergy\" : false},{\"id\" : 3, \"name\" : \"카레 라이스\", \"isSpecial\" : false, \"isAllergy\" : false},{\"id\" : 4, \"name\" : \"총각 김치\", \"isSpecial\" : true,  \"isAllergy\" : true},{\"id\" : 5, \"name\" : \"야쿠르트\", \"isSpecial\" : false, \"isAllergy\" : true}]}, \"mealState\" :{\"studentNum\" : 300, \"instanceStudentNum\" : 6, \"completedMealStudentNum\" : 158, \"completedInstanceMealStudentNum\": 2}}}");
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<FoodTicketCheckApiResponse>("{\"status\" : 0, \"user\" :{\"name\" : \"김동현\", \"department\" : \"개발3팀장\", \"grade\" : 2, \"class\" : 4, \"number\" : 4, \"profileUrl\" : \"http://sstatic.naver.net/search/img3/h1_naver2.png\"}, \"event\" :{\"regularTime\" : \"17:00:00\", \"checkoutTime\" : \"17:10:00\", \"diffTime\" : \"00:10:00\"}, \"meal\" :{\"mealData\" :{\"isUsableRFIDCard\" : false, \"mealTime\" : \"저녁\", \"mealStartTime\" : \"18:20:00\", \"mealStopTime\" : \"19:30:00\", \"mealSupplyStartTime\" : \"18:20:00\", \"mealSupplyStopTime\" : \"19:15:00\", \"mealInstanceStartTime\" : \"19:10:00\", \"mealInstanceCouponNum\" : 30, \"mealName\" : \"자장면 데이\", \"foodList\" :[{\"id\" : 1, \"name\" : \"코다리 튀김\", \"isSpecial\" : true,  \"isAllergy\" : false},{\"id\" : 2, \"name\" : \"짜장 라이스\", \"isSpecial\" : false, \"isAllergy\" : false},{\"id\" : 3, \"name\" : \"카레 라이스\", \"isSpecial\" : false, \"isAllergy\" : false},{\"id\" : 4, \"name\" : \"총각 김치\", \"isSpecial\" : true,  \"isAllergy\" : true},{\"id\" : 5, \"name\" : \"야쿠르트\", \"isSpecial\" : false, \"isAllergy\" : true}]}, \"mealState\" :{\"studentNum\" : 300, \"instanceStudentNum\" : 6, \"completedMealStudentNum\" : 158, \"completedInstanceMealStudentNum\": 2}}}");
        }

        #endregion Constructor

        #region Properties

        private clsEventStatus _status;
        public clsEventStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        #endregion Properties
    }
}