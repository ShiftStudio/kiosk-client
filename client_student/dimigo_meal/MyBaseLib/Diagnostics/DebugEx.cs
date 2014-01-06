using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Text;
using System.Threading;

namespace MyBaseLib.Diagnostics
{
    public static class DebugEx
    {
        private static DateTime _DateTimeOld = DateTime.Now;
        private static SynchronizationContext _uiThreadContext = SynchronizationContext.Current;
        public static StringBuilder RotatedLogStr = new StringBuilder();
        public static ObservableCollection<string> RotatedLogStrCollection = new ObservableCollection<string>();

        [SecuritySafeCritical]
        public static void Assert(bool condition)
        {
            Assert(condition, "", "", null);
        }

        [SecuritySafeCritical]
        public static void Assert(bool condition, string message)
        {
            Assert(condition, message, "", null);
        }

        [SecuritySafeCritical]
        public static void Assert(bool condition, string message, string detailMessage)
        {
            Assert(condition, message, detailMessage, null);
        }

        [SecuritySafeCritical]
        public static void Assert(bool condition, string message, string detailMessage, params object[] args)
        {
            if (!condition) // && !DesignerProperties.get_IsInDesignTool())
            {
                StringBuilder builder = new StringBuilder();
                StackTrace trace = new StackTrace();
                builder.Append("CALL_STACK = \n");
                builder.Append(trace.ToString() + "\n");
                if (!string.IsNullOrEmpty(message))
                {
                    builder.Append("MSG = \n");
                    builder.Append(message + "\n");
                }
                if (!string.IsNullOrEmpty(detailMessage))
                {
                    builder.Append("DETAIL_MSG = \n");
                    builder.Append(string.Format(detailMessage, args) + "\n");
                }
                if (Debugger.IsAttached)
                {
                    WriteLine(builder.ToString());
                }
                else
                {
                    IntendedCrashException exception = new IntendedCrashException();
                    builder.Append("TOTAL_LOG = \n");
                    builder.Append(RotatedLogStr);
                    exception.DebugStr = builder.ToString();
                    throw exception;
                }
            }
        }

        [SecuritySafeCritical]
        public static void AssertWithOutCrash(bool condition)
        {
            AssertWithOutCrash(condition, "");
        }

        [SecuritySafeCritical]
        public static void AssertWithOutCrash(bool condition, string message)
        {
            if (!condition) // && !DesignerProperties.get_IsInDesignTool())
            {
                StackTrace trace = new StackTrace();
                StringBuilder builder = new StringBuilder();
                builder.Append("CALL_STACK = \n");
                builder.Append(trace.ToString() + "\n");
                if (!string.IsNullOrEmpty(message))
                {
                    builder.Append("MSG = \n");
                    builder.Append(message + "\n");
                }
                if (Debugger.IsAttached)
                {
                    WriteLine(builder.ToString());
                }
                else
                {
                    IntendedNonCrashException exception = new IntendedNonCrashException();
                    builder.Append("TOTAL_LOG = \n");
                    builder.Append(RotatedLogStr);
                    exception.DebugStr = builder.ToString();
                    throw exception;
                }
            }
        }

        [SecuritySafeCritical]
        public static void WriteLine(object value)
        {
            WriteLine(value.ToString());
        }

        [SecuritySafeCritical]
        public static void WriteLine(string message)
        {
            StringBuilder builder = new StringBuilder(DateTime.Now.ToString("[yyMMdd HH:mm:ss.ff]"));
            DateTime now = DateTime.Now;
            TimeSpan span = (TimeSpan)(now - _DateTimeOld);
            builder.Append(string.Format("[{0}ms] ", span.TotalMilliseconds));
            _DateTimeOld = now;
            builder.Append(message);
            builder.Append("\n");
            string str = builder.ToString();
            if (message.Contains("\n"))
            {
                builder.Append("\n");
            }
            str.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            RotatedLogStr.Append(builder);
            if (RotatedLogStr.Length > 0x7530)
            {
                RotatedLogStr.Remove(0, 0x3a98);
                int num = RotatedLogStrCollection.Count / 2;
                for (int i = 0; i < num; i++)
                {
                    RotatedLogStrCollection.RemoveAt(0);
                }
            }
        }

        [SecuritySafeCritical]
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        [SecuritySafeCritical]
        public static void WriteLineByJson(string message, object value)
        {
            WriteLine(message + "\n" + JsonConvert.SerializeObject(value));
        }
    }
}

