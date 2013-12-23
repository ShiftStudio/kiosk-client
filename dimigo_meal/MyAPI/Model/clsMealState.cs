using MyAPI.Common;

namespace MyAPI.Model
{
    public class clsMealState : NotifiableModelBase
    {
        private int _studentNum;
        public int StudentNum
        {
            get
            {
                return _studentNum;
            }
            set
            {
                _studentNum = value;
                OnPropertyChanged("StudentNum");
            }
        }

        private int _instanceStudentNum;
        public int InstanceStudentNum
        {
            get
            {
                return _instanceStudentNum;
            }
            set
            {
                _instanceStudentNum = value;
                OnPropertyChanged("InstanceStudentNum");
            }
        }

        private int _completedMealStudentNum;
        public int CompletedMealStudentNum
        {
            get
            {
                return _completedMealStudentNum;
            }
            set
            {
                _completedMealStudentNum = value;
                OnPropertyChanged("CompletedMealStudentNum");
            }
        }

        private int _completedInstanceMealStudentNum;
        public int CompletedInstanceMealStudentNum
        {
            get
            {
                return _completedInstanceMealStudentNum;
            }
            set
            {
                _completedInstanceMealStudentNum = value;
                OnPropertyChanged("CompletedInstanceMealStudentNum");
            }
        }
    }
}
