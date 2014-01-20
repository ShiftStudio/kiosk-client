using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MealAPI.RESTAPI
{
    public enum ApiStatus
    {
        SUCCESS = 0,
        NETWORK_ERROR = -500,
        UNKNOWN_ERROR = -999
    }
}
