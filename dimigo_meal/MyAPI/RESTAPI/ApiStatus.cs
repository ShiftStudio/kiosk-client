namespace MyAPI.RESTAPI
{
    public enum ApiStatus
    {
        SUCCESS = 0,
        NEED_AUTH = -998,
        NEED_UPDATE = -999,
        NETWORK_ERROR = -501,
        UNKNOWN_ERROR = -500
    }
}