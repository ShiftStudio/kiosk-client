using System;
using System.ComponentModel;
using System.Threading;

namespace MyBaseLib.Diagnostics
{
    public class IntendedCrashException : Exception, INotifyPropertyChanged
    {
        private string _DebugStr;
        public const string STR_DebugStr = "DebugStr";

        public event PropertyChangedEventHandler PropertyChanged;

        private void _Update_DebugStr()
        {
        }

        public string DebugStr
        {
            get
            {
                return this._DebugStr;
            }
            set
            {
                this._DebugStr = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("DebugStr"));
                }
            }
        }
    }
}

