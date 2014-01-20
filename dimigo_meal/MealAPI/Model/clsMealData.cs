using System;

namespace MealAPI.Model
{
    public class clsMealData : MealAPI.Common.NotifiableModelBase
    {
        private bool _isUsableRFIDCard = false;
        public bool IsUsableRFIDCard
        {
            get
            {
                return _isUsableRFIDCard;
            }
            set
            {
                _isUsableRFIDCard = value;
                OnPropertyChanged("IsUsableRFIDCard");
            }
        }

        private bool _isServing = false;
        public bool IsServing
        {
            get
            {
                return _isServing;
            }
            set
            {
                _isServing = value;
                OnPropertyChanged("IsServing");
            }
        }

        private string _mealTime = null;
        public string MealTime
        {
            get
            {
                return _mealTime;
            }
            set
            {
                _mealTime = value;
                OnPropertyChanged("MealTime");
            }
        }

        private DateTime _mealStartTime = default(DateTime);
        public DateTime MealStartTime
        {
            get
            {
                return _mealStartTime;
            }
            set
            {
                _mealStartTime = value;
                OnPropertyChanged("MealStartTime");
            }
        }

        private DateTime _mealStopTime = default(DateTime);
        public DateTime MealStopTime
        {
            get
            {
                return _mealStopTime;
            }
            set
            {
                _mealStopTime = value;
                OnPropertyChanged("MealStopTime");
            }
        }

        private DateTime _mealSupplyStartTime = default(DateTime);
        public DateTime MealSupplyStartTime
        {
            get
            {
                return _mealSupplyStartTime;
            }
            set
            {
                _mealSupplyStartTime = value;
                OnPropertyChanged("MealSupplyStartTime");
            }
        }

        private DateTime _mealSupplyStopTime = default(DateTime);
        public DateTime MealSupplyStopTime
        {
            get
            {
                return _mealSupplyStopTime;
            }
            set
            {
                _mealSupplyStopTime = value;
                OnPropertyChanged("MealSupplyStopTime");
            }
        }

        private DateTime _mealInstanceStartTime = default(DateTime);
        public DateTime MealInstanceStartTime
        {
            get
            {
                return _mealInstanceStartTime;
            }
            set
            {
                _mealInstanceStartTime = value;
                OnPropertyChanged("MealInstanceStartTime");
            }
        }

        private int _mealInstanceCouponNum = 0;
        public int MealInstanceCouponNum
        {
            get
            {
                return _mealInstanceCouponNum;
            }
            set
            {
                _mealInstanceCouponNum = value;
                OnPropertyChanged("MealInstanceCouponNum");
            }
        }

        private string _mealName = null;
        public string MealName
        {
            get
            {
                return _mealName;
            }
            set
            {
                _mealName = value;
                OnPropertyChanged("MealName");
            }
        }

        private clsFood[] _foodList = null;
        public clsFood[] FoodList
        {
            get
            {
                return _foodList;
            }
            set
            {
                _foodList = value;
                OnPropertyChanged("FoodList");
            }
        }
    }
}
