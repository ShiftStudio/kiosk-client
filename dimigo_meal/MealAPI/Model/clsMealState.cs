﻿namespace MealAPI.Model
{
    public class clsMealState : MealAPI.Common.NotifiableModelBase
    {
        private int _processedUser;
        public int ProcessedUser
        {
            get
            {
                return _processedUser;
            }
            set
            {
                _processedUser = value;
                OnPropertyChanged("ProcessedUser");
            }
        }

        private int _totalUser;
        public int TotalUser
        {
            get
            {
                return _totalUser;
            }
            set
            {
                _totalUser = value;
                OnPropertyChanged("TotalUser");
            }
        }
    }
}