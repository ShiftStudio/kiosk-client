using MealAPI.RESTAPI;
using MyBaseLib.Network;
using dimigo_meal.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using dimigo_meal.View.Common;
using MealAPI.Model;

namespace dimigo_meal.Model
{
    public static class DataSyncManager
    {
        #region Field

        static private BackgroundWorker _worker;

        static private NewDataCheckApi newDataCheckApi = null;

        #endregion Field
        
        #region Method

        static public void Start()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerAsync();
        }

        #endregion Method

        #region Event

        static private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (newDataCheckApi != null)
                {
                    newDataCheckApi.ResponseSucceeded -= DataSyncManager.NewDataCheckApi_ResponseSucceeded;
                    newDataCheckApi.ResponseFailed -= DataSyncManager.NewDataCheckApi_ResponseFailed;
                    newDataCheckApi.Cancel();
                }

                NewDataCheckApiRequest request = new NewDataCheckApiRequest
                {
                    TimeStamp = DateTime.Now.GetUnixTime()
                };

                newDataCheckApi = new NewDataCheckApi();
                newDataCheckApi.ResponseSucceeded += DataSyncManager.NewDataCheckApi_ResponseSucceeded;
                newDataCheckApi.ResponseFailed += DataSyncManager.NewDataCheckApi_ResponseFailed;
                newDataCheckApi.Send(request);

                Thread.Sleep(5000);
            }
        }

        static private void NewDataCheckApi_ResponseSucceeded(object sender, HttpApiResponseBase e)
        {
            NewDataCheckApi apiObj = sender as NewDataCheckApi;
            NewDataCheckApiResponse response = apiObj.HttpApiResponse as NewDataCheckApiResponse;

            if (response.Status >= 0)
            {
                //Used delegate to prevent thread conflict
                ViewStateManager.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (response.Meal != null)
                    {
                        ViewStateManager.MainWindow.ViewModel.MealState = response.Meal.MealState;
                        ViewStateManager.MainWindow.ViewModel.MealData = response.Meal.MealData;
                    }
                }));
            }
        }

        static private void NewDataCheckApi_ResponseFailed(object sender, HttpHelperEventArgs e)
        {
            ViewStateManager.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (e == null)
                {
                    ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                    {
                        Status = clsEventStatus.TIMEOUT,
                        Title = "타임아웃 - 1",
                        Message = "서버 응답 시간이 초과하였습니다."
                    };
                    ViewStateManager.Navigate(new ErrorDisplayView(vm));
                }
                else if (e.ExceptionObj is TimeoutException)
                {
                    ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                    {
                        Status = clsEventStatus.TIMEOUT,
                        Title = "타임아웃 - 2",
                        Message = "서버 응답 시간이 초과하였습니다."
                    };
                    ViewStateManager.Navigate(new ErrorDisplayView(vm));
                }
                else if (e.ExceptionObj is System.Net.WebException)
                {
                    ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                    {
                        Status = clsEventStatus.NETWORK_ERROR,
                        Title = "네트워크 에러",
                        Message = e.ExceptionObj.Message
                    };
                    ViewStateManager.Navigate(new ErrorDisplayView(vm));
                }
                else if (e.ExceptionObj is System.Net.Sockets.SocketException)
                {
                    ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                    {
                        Status = clsEventStatus.NETWORK_ERROR,
                        Title = "네트워크 에러",
                        Message = e.ExceptionObj.Message
                    };
                    ViewStateManager.Navigate(new ErrorDisplayView(vm));
                }
            }));
        }

        #endregion Event
    }
}
