using dimigo_meal.Common;
using dimigo_meal.Model;
using dimigo_meal.View;
using dimigo_meal.View.Common;
using MealAPI.Model;
using MealAPI.RESTAPI;
using MyBaseLib.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dimigo_meal.Common
{
    public abstract class RFIDReaderViewBaseEx : RFIDReaderViewBase
    {
        protected void Api_ResponseSucceeded(object sender, HttpApiResponseBase e)
        {
            FoodTicketCheckApiResponse response = this._GetResponseObjectfromEvent(sender);

            //event.status returns lower than -200 when exception is throwed
            //should fix later
            if ((int)response.Status > -200)
            {
                ///meal/verify/<target> currently do not give MealData
                //ViewStateManager.MainWindow.ViewModel.MealState = response.Meal.MealState;
                //ViewStateManager.MainWindow.ViewModel.MealData = response.Meal.MealData;

                ResultDisplayViewModel vm = new ResultDisplayViewModel()
                {
                    User = response.User,
                    Event = response.Event
                };
                ViewStateManager.Navigate(new ResultDisplayView(vm));

                NarrationPlayer sp = new NarrationPlayer();
                if (response.Status >= 0)
                {
                    sp.Play("띵동");
                }
            }
            else
            {
                ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                {
                    Status = response.Event.Status,
                    Title = response.Title,
                    Message = response.Message
                };
                ViewStateManager.Navigate(new ErrorDisplayView(vm));

                NarrationPlayer sp = new NarrationPlayer();
               // sp.Play("띵동");
            }
        }

        protected void Api_ResponseFailed(object sender, HttpHelperEventArgs e)
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

        private FoodTicketCheckApiResponse _GetResponseObjectfromEvent(object sender)
        {
            HttpApiBase apiObj = sender as HttpApiBase;
            FoodTicketCheckApiResponse response = apiObj.HttpApiResponse as FoodTicketCheckApiResponse;
            return response;            
            
        }
    }
}
