namespace MealAPI.Model
{
    public enum clsEventStatus
    {
        SUCCESS = 0,
        ALREADY_EATEN = -101,
        BANNED = -102,
        INVALID_USER = -199,
        USER_ERROR = -301,
        DATABASE_ERROR = -302,
        NETWORK_ERROR = -500,
        TIMEOUT = -998,
        UNKNOWN_ERROR = -999
    }
}
