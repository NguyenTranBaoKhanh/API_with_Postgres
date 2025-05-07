namespace Common.Core.Domain.Request
{
    public class DBExecuteRequest
    {
        #region Public Properties

        public string Schema { get; set; }
        public string SpName { get; }
        public object Request { get; }
        public int? Timeout { get; }

        #endregion


        #region Constructor

        public DBExecuteRequest(string spName, object request = null, int? timeout = null)
            : this("UseDefaultSchema", spName, request, timeout)
        {
        }

        public DBExecuteRequest(string schema, string spName, object request, int? timeout = null)
        {
            Schema = schema;
            SpName = spName;
            Request = request;
            Timeout = timeout;
        }

        #endregion
    }

    public class DBExecuteRequestSp<T> : DBExecuteRequest
        where T: Enum
    {

        public DBExecuteRequestSp(T spName, object request = null, int? timeout = null)
            : this(string.Empty, spName, request, timeout)
        {
        }

        public DBExecuteRequestSp(string schema, T spName, object request, int? timeout = null)
            :base (schema, spName.ToString(), request, timeout)
        {
        }
    }
}
