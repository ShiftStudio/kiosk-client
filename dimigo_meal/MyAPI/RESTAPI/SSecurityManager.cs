using MyBaseLib.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dimigo_meal.MyAPI.RESTAPI
{
    public class SSecurityManager
    {
        public static string SerializeAuth(HttpApiRequestBase apibase)
        {
            double timestamp = apibase.TimeStamp;

            return "";
        }

    }
}
