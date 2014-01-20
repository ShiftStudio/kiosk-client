namespace MealAPI.Model
{
    public class clsFood : MealAPI.Common.NotifiableModelBase
    {
        public clsFood() { }
        public clsFood(string name) { this.Name = name; }
        public clsFood(string name, bool isSpecial) { this.Name = name; this.IsSpecial = isSpecial; }

        private int _id = 0;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _name = null;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private bool _isSpecial = false;
        public bool IsSpecial
        {
            get
            {
                return _isSpecial;
            }
            set
            {
                _isSpecial = value;
                OnPropertyChanged("IsSpecial");
            }
        }

        private bool _isAllergy = false;
        public bool IsAllergy
        {
            get
            {
                return _isAllergy;
            }
            set
            {
                _isAllergy = value;
                OnPropertyChanged("IsAllergy");
            }
        }
    }
}
