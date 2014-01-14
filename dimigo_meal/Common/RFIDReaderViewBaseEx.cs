using dimigo_meal.Common;
using dimigo_meal.View;
using MyAPI.Model;
using MyAPI.RESTAPI;
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

            if (App.KioskViewMode == ViewMode.TEACHER_KIOSK)
            {
                App.MainWindow.MainWindowViewState = MainWindowViewState.MAIN_VIEW_TEACHER;
            }

            //event.status returns lower than -200 when exception is throwed
            //should fix later
            if ((int)response.Status > -200)
            {
                ///meal/verify/<target> currently do not give MealData
                //App.MainWindow.ViewModel.MealState = response.Meal.MealState;
                //App.MainWindow.ViewModel.MealData = response.Meal.MealData;

                ResultDisplayViewModel vm = new ResultDisplayViewModel()
                {
                    User = response.User,
                    Event = response.Event
                };
                App.MainFrame.Navigate(new ResultDisplayView(vm));

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
                App.MainFrame.Navigate(new ErrorDisplayView(vm));

                NarrationPlayer sp = new NarrationPlayer();
                sp.Play("띵동");
            }
        }

        protected void Api_ResponseFailed(object sender, HttpHelperEventArgs e)
        {
            //HttpApiResponseBase response = this._GetResponseObjectfromEvent(sender);

            if (e == null)
            {
                // TimeOut;
            }
            else if (e.ExceptionObj is System.Net.WebException)
            {
                ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                {
                    Status = clsEventStatus.NETWORK_ERROR,
                    Title = "네트워크 에러",
                    Message = e.ExceptionObj.Message
                };
                App.MainFrame.Navigate(new ErrorDisplayView(vm));
            }
            else if (e.ExceptionObj is System.Net.Sockets.SocketException)
            {
                ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                {
                    Status = clsEventStatus.NETWORK_ERROR,
                    Title = "네트워크 에러",
                    Message = e.ExceptionObj.Message
                };
                App.MainFrame.Navigate(new ErrorDisplayView(vm));
            }
            else if (e.ExceptionObj is TimeoutException)
            {
                // TimeOut;
            }
        }

        private FoodTicketCheckApiResponse _GetResponseObjectfromEvent(object sender)
        {
            HttpApiBase apiObj = sender as HttpApiBase;
            
            FoodTicketCheckApiResponse response = apiObj.HttpApiResponse as FoodTicketCheckApiResponse;
            return response;            
            
        }
    }
}
