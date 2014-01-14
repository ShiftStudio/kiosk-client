﻿using dimigo_meal.Common;
using dimigo_meal.View;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using YangpaH;

namespace dimigo_meal
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public static readonly ViewMode KioskViewMode = ViewMode.STUDENT_KIOSK;

        static App()
        {
            App.GoHomeCommand = new DelegateCommand<object>((s) =>
            {
                if (App.MainWindow.HomePageUri != null)
                {
                    App.MainWindow.Navigate(App.MainWindow.HomePageUri);
                }
            });

        }

        void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.ExceptionObject.ToString());
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //global exHandling
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
//            App.ShowCursor(false);
            App.MainWindow = new MainWindowView();
            App.MainFrame = App.MainWindow.MainFrame;
            App.MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            App.ShowCursor(true);
        }

        [DllImport("user32.dll")﻿]
        public static extern int ShowCursor(bool bShow);

        static public new MainWindowView MainWindow { get; set; }
        static public Frame MainFrame { get; set; }

        static public DelegateCommand<object> GoHomeCommand { get; private set; }

        public static string mc { get; set; }
    }
}