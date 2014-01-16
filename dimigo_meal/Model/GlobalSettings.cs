using dimigo_meal.Common;
using dimigo_meal.View;
using dimigo_meal.View.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace dimigo_meal.Model
{
    public class GlobalSettings
    {
        #region Constructor

        static readonly private object padLock = new object();
        static private GlobalSettings _instance = null;
        private GlobalSettings() { }

        static public GlobalSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (padLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GlobalSettings();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion Constructor

        #region Field

        #if STUDENT_MODE

        private static readonly ViewMode _KioskViewMode = ViewMode.STUDENT_KIOSK;
        
        #elif TEACHER_MODE
        
        private static readonly ViewMode _KioskViewMode = ViewMode.TEACHER_KIOSK;

        #endif

        #endregion Field

        #region Property

        static public ViewMode KioskViewMode
        {
            get
            {
                return _KioskViewMode;
            }
        }

        static public string ASDFQWERASDF
        {
            get
            {
                switch (GlobalSettings.KioskViewMode)
                {
                    case ViewMode.STUDENT_KIOSK:
                        return "학생 배식 가능 시간";
                    case ViewMode.TEACHER_KIOSK:
                        return "교사 배식 가능 시간";
                    default:
                        return null;
                }
            }
        }

        static public string QWERASDFQWER
        {
            get
            {
                switch (GlobalSettings.KioskViewMode)
                {
                    case ViewMode.STUDENT_KIOSK:
                        return "식사 학생 수";
                    case ViewMode.TEACHER_KIOSK:
                        return "식사 교직원 수";
                    default:
                        return null;
                }
            }
        }

        static public ImageSource Background
        {
            get
            {
                string path = null;
                switch (GlobalSettings.KioskViewMode)
                {
                    case ViewMode.STUDENT_KIOSK:
                        path = @"pack://application:,,,/dimigo_meal;component/Assets/Resources/s/background_static.png";
                        break;
                    case ViewMode.TEACHER_KIOSK:
                        path = @"pack://application:,,,/dimigo_meal;component/Assets/Resources/t/background_static.png";
                        break;
                    default:
                        return null;
                }
                
                return (ImageSource)new ImageSourceConverter().ConvertFromString(path);
            }
        }

        static public string MovieDirectory
        {
            get
            {
                return System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "mov");
            }
        }

        #endregion Property

        #region CONST

        public readonly string SOUND_DIRECTORY = Path.GetFullPath("song");

        #endregion
    }
}
