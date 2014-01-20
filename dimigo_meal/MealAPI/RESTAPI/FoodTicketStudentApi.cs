using MyBaseLib.Diagnostics;
using MyBaseLib.Network;
using Newtonsoft.Json;
using System;

namespace MealAPI.RESTAPI
{
    public class FoodTicketStudentApi : HttpApiBase
    {
        protected override void _GenerateRequest(HttpHelper httpHelper, HttpApiRequestBase httpApiRequest)
        {
            FoodTicketStudentApiRequest request = httpApiRequest as FoodTicketStudentApiRequest;
            if (request == null)
            {
                DebugEx.Assert(false, "request을 확인해 주세요");
            }
            else
            {
                httpHelper.Method = "POST";
                httpHelper.RequestUri = new Uri(string.Concat(APISettings.SERVER_HOST, APISettings.SERVER_MEAL_VERIFY_STUDENT));
                httpHelper.RequestHeaders["User-Agent"] = APISettings.UserAgent;
                httpHelper.RequestHeaders["Kiosk-Auth"] = SSecurityManager.SerializeAuth(request as HttpApiRequestBase, httpHelper.RequestUri);
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