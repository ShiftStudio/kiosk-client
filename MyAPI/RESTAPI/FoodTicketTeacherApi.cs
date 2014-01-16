using MyBaseLib.Diagnostics;
using MyBaseLib.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAPI.RESTAPI
{
    public class FoodTicketTeacherApi : HttpApiBase
    {
        protected override void _GenerateRequest(HttpHelper httpHelper, HttpApiRequestBase httpApiRequest)
        {
            FoodTicketTeacherApiRequest request = httpApiRequest as FoodTicketTeacherApiRequest;
            if (request == null)
            {
                DebugEx.Assert(false, "request을 확인해 주세요");
            }
            else
            {
                httpHelper.Method = "POST";
                httpHelper.RequestUri = new Uri(string.Concat(APISettings.SERVER_HOST, APISettings.SERVER_MEAL_VERIFY_TEACHER));
                httpHelper.RequestHeaders["User-Agent"] = SSecurityManager.SerializeAuth(request as HttpApiRequestBase, httpHelper.RequestUri);
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
