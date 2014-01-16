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
            #if !DEBUG
            MyUtil.ShowCursor(false);
            #endif

            ViewStateManager.MainWindow = new MainWindowView();
            ViewStateManager.MainWindow.Show();

            //global exHandling
            AppDomain.CurrentDomain.UnhandledException += ViewStateManager.OnUnhandledException;

            ViewStateManager.Start();
            DataSyncManager.Start();
        }
    }
}