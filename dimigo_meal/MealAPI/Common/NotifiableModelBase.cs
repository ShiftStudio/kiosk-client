using System.ComponentModel;

namespace MealAPI.Common
{
    public abstract class NotifiableModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);  // 내부적으로 프로퍼티 변경 체크후 이벤트 호출
                
            }
        }
    }
}