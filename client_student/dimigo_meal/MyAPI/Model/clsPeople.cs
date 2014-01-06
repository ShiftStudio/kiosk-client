using MyAPI.Common;

namespace MyAPI.Model
{
    public class clsPeople : NotifiableModelBase
    {
        private int _id;
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

        private string _name;
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

        private string _department;
        public string Department
        {
            get
            {
                return _department;
            }
            set
            {
                _department = value;
                OnPropertyChanged("Department");
            }
        }

        private int _grade;
        public int Grade
        {
            get
            {
                return _grade;
            }
            set
            {
                _grade = value;
                OnPropertyChanged("Grade");
            }
        }

        private int _class;
        public int Class
        {
            get
            {
                return _class;
            }
            set
            {
                _class = value;
                OnPropertyChanged("Class");
            }
        }

        private int _number;
        public int Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                OnPropertyChanged("Number");
            }
        }

        private string _profileUrl;
        public string ProfileUrl
        {
            get
            {
                return _profileUrl;
            }
            set
            {
                _profileUrl = value;
                OnPropertyChanged("ProfileUrl");
            }
        }
    }
}
//public int Id { get; set; }

//public string Name { get; set; }
//public string Department { get; set; }

//public int Grade { get; set; }
//public int @Class { get; set; }
//public int Number { get; set; }

//public string ProfileUrl { get; set; }