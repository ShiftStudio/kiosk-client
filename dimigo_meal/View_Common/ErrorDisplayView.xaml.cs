﻿using dimigo_meal.Common;
using MyAPI.RESTAPI;
using MyBaseLib.Network;
using System;

namespace dimigo_meal.View
{
    /// <summary>
    /// ResultDisplayView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ErrorDisplayView : RFIDReaderViewBaseEx
    {
        #region Constructor

        public ErrorDisplayView(ErrorDisplayViewModel ViewModel)
        {
            InitializeComponent();
            this.ViewModel = ViewModel;
            this.Unloaded += (s, e) =>
            {
                this.Dispose();
            };

            switch (this.ViewModel.Status)
            {
                /*case ApiStatus.UNKNOWN_ERROR:
                    this.PageLimitTime = 8;
                    break;
                case ApiStatus.NETWORK_ERROR:
                    this.PageLimitTime = 8;
                    break;*/
                default:
                    this.PageLimitTime = 5;
                    break;
            }
        }

        #endregion Constructor

        #region Properties

        public ErrorDisplayViewModel ViewModel
        {
            get
            {
                return this.DataContext as ErrorDisplayViewModel;
            }
            set
            {
                if (this.DataContext != value)
                {
                    this.DataContext = value;
                }
            }
        }

        #endregion Properties

        protected override void RFIDCode_Received(string RFIDCode)
        {
            FoodTicketStudentApiRequest request = new FoodTicketStudentApiRequest
            {
                RFIDCode = RFIDCode,
                TimeStamp = (DateTime.Now - DateTime.Parse("1970-01-01 09:00:00")).TotalSeconds
            };
            FoodTicketStudentApi api = new FoodTicketStudentApi();
            api.ResponseSucceeded += this.Api_ResponseSucceeded;
            api.ResponseFailed += this.Api_ResponseFailed;
            api.Send(request);
        }
    }
}