namespace API_with_Postgres.Common.Enums
{
    public enum ResponseCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 1,
        /// <summary>
        /// No authenticate
        /// </summary>
        NoAuthentication = 0,
        /// <summary>
        /// Generic Error
        /// </summary>
        Error = -1
    }

    public enum ExtDataType
    {
        Object = 1,
        Value = 2,
        List = 3
    }
}
