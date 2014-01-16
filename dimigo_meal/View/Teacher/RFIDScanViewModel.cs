using dimigo_meal.Common;

namespace dimigo_meal.View.Teacher
{
    public class RFIDScanViewModel : ViewModelBase
    {
        #region Properties

        private string _MealCount;
        public string MealCount
        {
            get
            {
                return _MealCount;
            }
            set
            {
                this._MealCount = value;
                OnPropertyChanged("MealCount");
            }
        }
        
        #endregion Properties
    }
}
