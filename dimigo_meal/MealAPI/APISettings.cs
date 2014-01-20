using dimigo_meal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealAPI
{
    public static class APISettings
    {
        public static readonly object padLock = new object();

        public const string SERVER_HOST = "http://closeapi.shiftstud.io";

        public const string SERVER_NEW_DATA_CHECK_API = "/meal/now";

        public const string SERVER_MEAL_VERIFY_TEACHER = "/meal/verify/teacher";

        public const string SERVER_MEAL_VERIFY_STUDENT = "/meal/verify/student";

        private static string _UserAgent = null;

        public static string UserAgent
        {
            get
            {
                lock (padLock)
                {
                    if (string.IsNullOrWhiteSpace(_UserAgent))
                    {
                        string programVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                        string dotnetVer = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion().Substring(1);
                        switch (GlobalSettings.KioskViewMode)
                        {
                            case ViewMode.TEACHER_KIOSK:
                                _UserAgent = string.Concat("ShiftKiosk/", programVer, " (Windows NT 6.1; .NET/", dotnetVer, "; Meal; Teacher)");
                                break;
                            case ViewMode.STUDENT_KIOSK:
                                _UserAgent = string.Concat("ShiftKiosk/", programVer, " (Windows NT 6.1; .NET/", dotnetVer, "; Meal; Student)");
                                break;
                        }
                    }
                    return _UserAgent;
                }
            }
        }
    }
}
