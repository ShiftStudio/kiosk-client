using System;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace dimigo_meal.Common
{
    public abstract class RFIDReaderViewBase : PopupPage
    {
        private static int KEY_PRESS_LIMIT_TIME = 3;

        private DispatcherTimer _timer = new DispatcherTimer();

        private int _time;

        private StringBuilder _RFIDCode = new StringBuilder();

        protected RFIDReaderViewBase()
        {
            this.Focusable = true;
            this.Focus();
            this.FocusVisualStyle = null;
            this.KeyDown += Form_KeyDown;

            this._timer.Interval = TimeSpan.FromSeconds(1.0);
            this._timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (--_time <= 0)
            {
                if (Timer != null)
                {
                    Timer.Stop();
                    _RFIDCode = new StringBuilder();
                }
            }
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (checkDigit(e.Key))
            {
                this._time = RFIDReaderViewBase.KEY_PRESS_LIMIT_TIME;
                if (_RFIDCode.Length == 0)
                {
                    _timer.Start();
                }
                _RFIDCode.Append(ConvertKeyToChar(e.Key));
            }
            else if (_RFIDCode.Length > 0)
            {
                _timer.Stop();
                if (e.Key == Key.Enter)
                {
                    string code = _RFIDCode.ToString();
                    this.RFIDCode_Received(code);
                }
                _RFIDCode = new StringBuilder();
            }
        }
        
        protected abstract void RFIDCode_Received(string RFIDCode);

        private bool checkDigit(Key key)
        {
            if ((int)key <= (int)Key.D9 && (int)key >= (int)Key.D0)
            {
                return true;
            }
            if ((int)key <= (int)Key.NumPad0 && (int)key >= (int)Key.NumPad9)
            {
                return true;
            }
            return false;
        }

        private char ConvertKeyToChar(Key key)
        {
            if ((int)key <= (int)Key.D9 && (int)key >= (int)Key.D0)
            {
                return (key - Key.D0).ToString()[0];
            }
            if ((int)key <= (int)Key.NumPad0 && (int)key >= (int)Key.NumPad9)
            {
                return (key - Key.D0).ToString()[0];
            }
            return default(char);
        }
    }
}