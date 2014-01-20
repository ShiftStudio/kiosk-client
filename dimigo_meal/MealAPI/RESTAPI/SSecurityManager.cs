using MyBaseLib.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MealAPI.RESTAPI
{
    public class SSecurityManager
    {
        public static string SerializeAuth(HttpApiRequestBase apibase, Uri desturl)
        {
            double timestamp = apibase.TimeStamp;
            string urls = desturl.ToString();

            string rv = urls + timestamp.ToString();

            return "";
        }

    }
}
