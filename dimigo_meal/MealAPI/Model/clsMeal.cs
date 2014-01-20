namespace MealAPI.Model
{
    public class clsMeal : MealAPI.Common.NotifiableModelBase
    {
        private clsMealData _mealData = null;
        public clsMealData MealData
        {
            get
            {
                if (_mealData == null)
                {
                    _mealData = new clsMealData();
                }
                return _mealData;
            }
            set
            {
                _mealData = value;
            }
        }

        private clsMealState _mealState = null;
        public clsMealState MealState
        {
            get
            {
                if (_mealState == null)
                {
                    _mealState = new clsMealState();
                }
                return _mealState;
            }
            set
            {
                _mealState = value;
            }
        }
    }
}
