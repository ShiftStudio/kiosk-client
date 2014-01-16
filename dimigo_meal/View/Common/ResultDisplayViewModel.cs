using dimigo_meal.Common;
using MyAPI.Model;
using MyAPI.RESTAPI;
using System.Windows.Media;

namespace dimigo_meal.View.Common
{
    public class ResultDisplayViewModel : ViewModelBase
    {
        #region Constructor

        public ResultDisplayViewModel()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                FoodTicketCheckApiResponse sample = ResultDisplayViewModel.getSampleData();
                this.User = sample.User;
                this.Event = sample.Event;
            }
        }

        public static FoodTicketCheckApiResponse getSampleData()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<FoodTicketCheckApiResponse>("{\"status\" : 0, \"user\" :{\"name\" : \"김동현\", \"department\" : \"개발3팀장\", \"grade\" : 2, \"class\" : 4, \"number\" : 4, \"profileUrl\" : \"\"}, \"event\" :{\"regularTime\" : \"17:00:00\", \"checkoutTime\" : \"17:10:00\", \"diffTime\" : \"00:10:00\"}, \"meal\" :{\"mealData\" :{\"isUsableRFIDCard\" : false, \"mealTime\" : \"저녁\", \"mealStartTime\" : \"18:20:00\", \"mealStopTime\" : \"19:30:00\", \"mealSupplyStartTime\" : \"18:20:00\", \"mealSupplyStopTime\" : \"19:15:00\", \"mealInstanceStartTime\" : \"19:10:00\", \"mealInstanceCouponNum\" : 30, \"mealName\" : \"자장면 데이\", \"foodList\" :[{\"id\" : 1, \"name\" : \"코다리 튀김\", \"isSpecial\" : true,  \"isAllergy\" : false},{\"id\" : 2, \"name\" : \"짜장 라이스\", \"isSpecial\" : false, \"isAllergy\" : false},{\"id\" : 3, \"name\" : \"카레 라이스\", \"isSpecial\" : false, \"isAllergy\" : false},{\"id\" : 4, \"name\" : \"총각 김치\", \"isSpecial\" : true,  \"isAllergy\" : true},{\"id\" : 5, \"name\" : \"야쿠르트\", \"isSpecial\" : false, \"isAllergy\" : true}]}, \"mealState\" :{\"studentNum\" : 300, \"instanceStudentNum\" : 6, \"completedMealStudentNum\" : 158, \"completedInstanceMealStudentNum\": 2}}}");
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<FoodTicketCheckApiResponse>("{\"status\" : 0, \"user\" :{\"name\" : \"김동현\", \"department\" : \"개발3팀장\", \"grade\" : 2, \"class\" : 4, \"number\" : 4, \"profileUrl\" : \"http://sstatic.naver.net/search/img3/h1_naver2.png\"}, \"event\" :{\"regularTime\" : \"17:00:00\", \"checkoutTime\" : \"17:10:00\", \"diffTime\" : \"00:10:00\"}, \"meal\" :{\"mealData\" :{\"isUsableRFIDCard\" : false, \"mealTime\" : \"저녁\", \"mealStartTime\" : \"18:20:00\", \"mealStopTime\" : \"19:30:00\", \"mealSupplyStartTime\" : \"18:20:00\", \"mealSupplyStopTime\" : \"19:15:00\", \"mealInstanceStartTime\" : \"19:10:00\", \"mealInstanceCouponNum\" : 30, \"mealName\" : \"자장면 데이\", \"foodList\" :[{\"id\" : 1, \"name\" : \"코다리 튀김\", \"isSpecial\" : true,  \"isAllergy\" : false},{\"id\" : 2, \"name\" : \"짜장 라이스\", \"isSpecial\" : false, \"isAllergy\" : false},{\"id\" : 3, \"name\" : \"카레 라이스\", \"isSpecial\" : false, \"isAllergy\" : false},{\"id\" : 4, \"name\" : \"총각 김치\", \"isSpecial\" : true,  \"isAllergy\" : true},{\"id\" : 5, \"name\" : \"야쿠르트\", \"isSpecial\" : false, \"isAllergy\" : true}]}, \"mealState\" :{\"studentNum\" : 300, \"instanceStudentNum\" : 6, \"completedMealStudentNum\" : 158, \"completedInstanceMealStudentNum\": 2}}}");
        }

        #endregion Constructor

        #region Properties
        
        private clsPeople _user;
        public clsPeople User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        private clsEvent _event;
        public clsEvent Event
        {
            get
            {
                return _event;
            }
            set
            {
                _event = value;
                OnPropertyChanged("Event");
            }
        }

        #endregion Properties

        #region Converter

        public string ProfileImageConverter(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                //url = string.Concat(User.Id, ".jpg");
                //url = Path.GetFullPath(string.Concat("profileImage/", url));
                url = "/dimigo_meal;component/Assets/Resources/noimage.jpg";
            }
            return url;
        }

        public string StatusToImageConverter(clsEventStatus Value)
        {
            if (Value >= 0)
            {
                return "/dimigo_meal;component/Assets/Resources/b_circle.png";
            }
            else
            {
                return "/dimigo_meal;component/Assets/Resources/b_cross.png";
            }
        }

        public Brush StatusToColorBrushConverter(clsEventStatus Value)
        {
            if (Value >= 0)
            {
                return new SolidColorBrush(Color.FromArgb(255, 13, 134, 173));
            }
            else
            {
                return new SolidColorBrush(Color.FromArgb(255, 124, 19, 22));
            }
        }

        public string StatusToUserConverter(clsPeople User)
        {
            if (User.Department == null || User.Position == null)
            {
                return User.Grade + "학년 " + User.Class + "반 " + User.Number + "번";
            }
            else
            {
                return User.Department + " " + User.Position;
            }
        }

        #endregion Converter
    }
}