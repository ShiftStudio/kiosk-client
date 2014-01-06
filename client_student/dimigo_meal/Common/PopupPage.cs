using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace dimigo_meal.Common
{
    public abstract class PopupPage : Page, IDisposable
    {
        public PopupPage()
        {
            this.PageLimitTime = 30;
            this.Timer = new DispatcherTimer();
            this.Timer.Interval = TimeSpan.FromSeconds(1.0);
            this.Timer.Tick += timer_Tick;
            this.Timer.Start();
        }

        public void Dispose()
        {
            if (this.Timer != null)
            {
                Timer.Stop();
                this.Timer.Tick -= timer_Tick;
                Timer = null;
            }
            GC.SuppressFinalize(this);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (--PageLimitTime <= 0)
            {
                Timer.Stop();
                App.GoHomeCommand.Execute(null);
            }
        }

        public DispatcherTimer Timer { get; set; }
        
        public EventHandler PageLimitTimeChanged;

        private int _pageLimitTime;
        public int PageLimitTime
        {
	        get
	        {
		        return _pageLimitTime;
	        }
	        set
	        {
		        if (_pageLimitTime != value)
		        {
			        _pageLimitTime = value;
                    if (_pageLimitTime == -1)
                    {
                        Timer.Stop();
                    }
                    if (PageLimitTimeChanged != null)
                    {
			            PageLimitTimeChanged.Invoke(_pageLimitTime, null);
                    }
		        }
	        }
        }
    }
}