using dimigo_meal.MyAPI.RESTAPI;
using MyBaseLib.Diagnostics;
using MyBaseLib.Network;
using Newtonsoft.Json;
using System;

namespace MyAPI.RESTAPI
{
    public class FoodTicketCheckApi : HttpApiBase
    {
        protected override void _GenerateRequest(HttpHelper httpHelper, HttpApiRequestBase httpApiRequest)
        {
            FoodTicketCheckApiRequest request = httpApiRequest as FoodTicketCheckApiRequest;
            if (request == null)
            {
                DebugEx.Assert(false, "request을 확인해 주세요");
            }
            else
            {
                httpHelper.Method = "POST";
                httpHelper.RequestUri = new Uri("http://y.suseme.me/meal/verify/student");
                httpHelper.RequestHeaders["User-Agent"] = SSecurityManager.SerializeAuth(request as HttpApiRequestBase);
                httpHelper.RequestBodyJsonObjects["data"] = request;
            }
        }

        protected override HttpApiResponseBase _GenerateResponse(HttpHelperEventArgs httpHelperEventArgs)
        {
            string strResponse = httpHelperEventArgs.Content;
            HttpApiResponseBase response = JsonConvert.DeserializeObject<FoodTicketCheckApiResponse>(strResponse);
            response.ResponseStr = strResponse;
            return response;
        }
    }
}