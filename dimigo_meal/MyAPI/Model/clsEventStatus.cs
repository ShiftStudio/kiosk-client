using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAPI.Model
{
    public enum clsEventStatus
    {
        SUCCESS = 0,
        ALREADY_EATEN = -1,
        BANNED = -2,
        INVALID_USER = -10
    }
}
