namespace MyAPI.RESTAPI
{
    public enum FoodTicketCheckApiStatus
    {
        SUCCESS = 0,
        ALREADY_EATEN = -2,
        INVALID_USER = -1,
        BANNED = -997,
        NEED_AUTH = -998,
        NEED_UPDATE = -999,
        NETWORK_ERROR = -501,
        UNKNOWN_ERROR = -500
    }
}