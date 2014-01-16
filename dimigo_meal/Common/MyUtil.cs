using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace dimigo_meal.Common
{
    public static class MyUtil
    {
        [DllImport("user32.dll")﻿]
        public static extern int ShowCursor(bool bShow);

        public static double GetUnixTime(this DateTime dateTime)
        {
            return (dateTime - DateTime.Parse("1970-01-01 09:00:00")).TotalSeconds;
        }

        public static void WPFNavigateSoundDisable()
        {
            RegistryKey rkey = null;
            rkey = Registry.CurrentUser.OpenSubKey(@"AppEvents\Schemes\Apps\Explorer\Navigating\.Current", true);
            rkey.SetValue(null, "", RegistryValueKind.ExpandString);

            rkey = Registry.CurrentUser.OpenSubKey(@"AppEvents\Schemes\Apps\Explorer\Navigating\.Default", true);
            rkey.SetValue(null, "", RegistryValueKind.ExpandString);

            //http://stackoverflow.com/questions/10456/howto-disable-webbrowser-click-sound-in-your-app-only
        }
    }
}
