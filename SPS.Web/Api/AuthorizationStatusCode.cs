namespace SPS.Web.Api
{
    public enum AuthorizationStatusCode : byte
    {
        Ok = 100,
        InvalidTag = 101,
        InvalidParking = 102,
        InvalidTagParking = 103,
        UnknownError = 104
    }
}