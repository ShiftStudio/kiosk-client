using MyBaseLib.Diagnostics;
using MyBaseLib.Network;
using Newtonsoft.Json;
using System;

namespace MyAPI.RESTAPI
{
    public class NewDataCheckApi : HttpApiBase
    {
        protected override void _GenerateRequest(HttpHelper httpHelper, HttpApiRequestBase httpApiRequest)
        {
            NewDataCheckApiRequest request = httpApiRequest as NewDataCheckApiRequest;
            if (request == null)
            {
                DebugEx.Assert(false, "request을 확인해 주세요");
            }
            else
            {
                httpHelper.Method = "POST";
                httpHelper.RequestUri = new Uri(string.Concat(APISettings.SERVER_HOST, APISettings.SERVER_NEW_DATA_CHECK_API));
                httpHelper.RequestHeaders["User-Agent"] = SSecurityManager.SerializeAuth(request as HttpApiRequestBase, httpHelper.RequestUri);
                httpHelper.RequestBodyJsonObjects["data"] = request;
            }
        }

        protected override HttpApiResponseBase _GenerateResponse(HttpHelperEventArgs httpHelperEventArgs)
        {
            string strResponse = httpHelperEventArgs.Content;
            HttpApiResponseBase response = JsonConvert.DeserializeObject<NewDataCheckApiResponse>(strResponse, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            response.ResponseStr = strResponse;
            return response;
        }
    }
}