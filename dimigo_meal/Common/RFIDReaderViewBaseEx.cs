using dimigo_meal.Common;
using dimigo_meal.View;
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

            if (response.Status >= 0)
            {
                App.MainWindow.ViewModel.MealState = response.Meal.MealState;
                App.MainWindow.ViewModel.MealData = response.Meal.MealData;

                ResultDisplayViewModel vm = new ResultDisplayViewModel()
                {
                    User = response.User,
                    Event = response.Event
                };
                App.MainFrame.Navigate(new ResultDisplayView(vm));

                NarrationPlayer sp = new NarrationPlayer();
                if (response.Event.Status >= 0)
                {
                    sp.Play("띵동");
                }
            }
            else
            {
                ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                {
                    Status = response.Status,
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
                    Status = ApiStatus.NETWORK_ERROR,
                    Title = "네트워크 에러",
                    Message = e.ExceptionObj.Message
                };
                App.MainFrame.Navigate(new ErrorDisplayView(vm));
            }
            else if (e.ExceptionObj is System.Net.Sockets.SocketException)
            {
                ErrorDisplayViewModel vm = new ErrorDisplayViewModel()
                {
                    Status = ApiStatus.NETWORK_ERROR,
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
            
            switch (App.KioskViewMode)
            {
               case View.ViewMode.STUDENT_KIOSK:
                    apiObj = apiObj as FoodTicketStudentApi;
                    break;
                case View.ViewMode.TEACHER_KIOSK:
                    apiObj = apiObj as FoodTicketTeacherApi;
                    break;
            }
            
                FoodTicketCheckApiResponse response = apiObj.HttpApiResponse as FoodTicketCheckApiResponse;
                return response;            
            
        }
    }
}
