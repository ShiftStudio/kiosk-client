using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAPI.Model
{
    public enum clsEventStatus
    {
        SUCCESS = 0,
        E_INVALID_USER = -1,
        UNKNOWS_ERROR = -500,
        BANNED = -997,
        NEED_AUTH = -998,
        NEED_UPDATE = -999,
        NETWORK_ERROR = -501
    }
}
