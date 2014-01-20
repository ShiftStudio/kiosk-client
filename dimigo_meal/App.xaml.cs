using dimigo_meal.Common;
using dimigo_meal.Model;
using dimigo_meal.View.Common;
using System;
using System.Windows;

namespace dimigo_meal
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MyUtil.WPFNavigateSoundDisable();
            
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(
                    System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag)));

            #if STUDENT_MODE && !DEBUG
            MyUtil.ShowCursor(false);
            #endif

            #if DEBUG
            ViewStateManager.MainWindow = new MainWindowView();
            ViewStateManager.MainWindow.Topmost = true;
            ViewStateManager.MainWindow.Show();
            #else
            ViewStateManager.MainWindow = new MainWindowView();
            ViewStateManager.MainWindow.Show();
            #endif

            //global exHandling
            AppDomain.CurrentDomain.UnhandledException += ViewStateManager.OnUnhandledException;

            ViewStateManager.Start();
            DataSyncManager.Start();
        }
    }
}