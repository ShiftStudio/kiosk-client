using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAPI
{
    public static class APISettings
    {
        public const string SERVER_HOST = "http://closeapi.shiftstud.io";

        public const string SERVER_NEW_DATA_CHECK_API = "/meal/now";

        public const string SERVER_MEAL_VERIFY_TEACHER = "/meal/verify/teacher";

        public const string SERVER_MEAL_VERIFY_STUDENT = "/meal/verify/student";
    }
}
