using dimigo_meal.Common;
using MyAPI.Model;
using MyAPI.RESTAPI;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace dimigo_meal.View
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Constructor

        public MainWindowViewModel()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                FoodTicketCheckApiResponse sample = MainWindowViewModel.getSampleData();
                this.MealData = sample.Meal.MealData;
                this.MealState = sample.Meal.MealState;
            }
        }

        public static FoodTicketCheckApiResponse getSampleData()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<FoodTicketCheckApiResponse>(
                "{\"status\" : 0, \"user\" :{\"name\" : \"김동현\", \"department\" : \"개발3팀장\", \"grade\" : 2, \"class\" : 4, \"number\" : 4, \"profileUrl\" : \"http://sstatic.naver.net/search/img3/h1_naver2.png\"}, \"event\" :{\"regularTime\" : \"17:00:00\", \"checkoutTime\" : \"17:10:00\", \"diffTime\" : \"00:10:00\"}, \"meal\" :{\"mealData\" :{\"isUsableRFIDCard\" : false, \"mealTime\" : \"저녁\", \"mealStartTime\" : \"18:20:00\", \"mealStopTime\" : \"19:30:00\", \"mealSupplyStartTime\" : \"18:20:00\", \"mealSupplyStopTime\" : \"19:15:00\", \"mealInstanceStartTime\" : \"19:10:00\", \"mealInstanceCouponNum\" : 30, \"mealName\" : \"자장면 데이\", \"foodList\" :[{\"id\" : 1, \"name\" : \"코다리 튀김\", \"isSpecial\" : true,  \"isAllergy\" : false},{\"id\" : 2, \"name\" : \"짜장 라이스\", \"isSpecial\" : false, \"isAllergy\" : false},{\"id\" : 3, \"name\" : \"카레 라이스\", \"isSpecial\" : false, \"isAllergy\" : false},{\"id\" : 4, \"name\" : \"총각 김치\", \"isSpecial\" : true,  \"isAllergy\" : true},{\"id\" : 5, \"name\" : \"야쿠르트\", \"isSpecial\" : false, \"isAllergy\" : true}]}, \"mealState\" :{\"studentNum\" : 300, \"instanceStudentNum\" : 6, \"completedMealStudentNum\" : 158, \"completedInstanceMealStudentNum\": 2}}}");
        }

        #endregion Constructor

        #region Properties

        private DateTime _now = DateTime.Now;
        public DateTime Now
        {
            get
            {
                return _now;
            }
            set
            {
                this._now = value;
                OnPropertyChanged("Now");
            }
        }

        private clsMealData _mealData;
        public clsMealData MealData
        {
            get
            {
                return _mealData;
            }
            set
            {
                this._mealData = value;
                OnPropertyChanged("MealData");
            }
        }

        private clsMealState _mealState;
        public clsMealState MealState
        {
            get
            {
                return _mealState;
            }
            set
            {
                this._mealState = value;
                OnPropertyChanged("MealState");
            }
        }
        
        #endregion Properties
        
        #region Converter

        public string TimeConverter1(DateTime _time)
        {
            return _time.ToString("tt");
        }

        public string TimeConverter2(DateTime _time)
        {
            return _time.ToString("hh:mm:ss");
        }

        public string TimeConverter3(DateTime _time)
        {
            return _time.ToString("tt hh시 mm분");
        }

        public string DateConverter1(DateTime _time)
        {
            return _time.ToString("yyyy년");
        }

        public string DateConverter2(DateTime _time)
        {
            return _time.ToString("MM월 dd일");
        }

        public string DateConverter3(DateTime _time)
        {
            return _time.ToString("dddd");
        }

        public List<Inline> FoodListConverter(clsFood[] list)
        {
            List<Inline> inlineCollection = new List<Inline>();
            for (int i = 0; i < list.Length; ++i)
            {
                Run buf = new Run(list[i].Name);
                if (list[i].IsSpecial)
                    buf.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

                inlineCollection.Add(buf);
                if (i < list.Length - 1) inlineCollection.Add(new Run(","));
            }
            return inlineCollection;
        }

        #endregion Converter

    }
}