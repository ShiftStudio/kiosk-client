using dimigo_meal.MyAPI.RESTAPI;
using MyBaseLib.Diagnostics;
using MyBaseLib.Network;
using Newtonsoft.Json;
using System;

namespace MyAPI.RESTAPI
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
                //meal/verify/student에서 식권현장발급까지 동시에 처리하도록 core 구상하자
                //씨샵 나부랭이보단 파이썬쨔응이 훨씬 편함
                httpHelper.Method = "POST";
                httpHelper.RequestUri = new Uri("http://closeapi.shiftstud.io/meal/verify/student");
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