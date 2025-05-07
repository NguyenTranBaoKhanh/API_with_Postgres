using API_with_Postgres.Common.Extentions;
using API_with_Postgres.Common.Response;
using Common.Core.Domain.Request;
using Common.Library.Collection.Dapper;
using Newtonsoft.Json;
using System.Data;

namespace Common.Core.DAL
{
    public abstract class BaseRepository<S> : BasePGRepository
        where S : Enum
    {
        #region Protected Members

        protected readonly IDbTransaction _transaction;
        protected virtual string DefaultSchema => "public";

        #endregion


        #region Constructor

        protected BaseRepository(Type concreteType)
            : this(null, concreteType)
        {
        }

        protected BaseRepository(IDbTransaction transaction, Type concreteType)
            : base(concreteType)
        {
            _transaction = transaction;
        }

        #endregion


        #region Protected Methods
        
        protected async Task<CommonResponse<T>> ExecuteStoredProcSingleAsync<T>(DBExecuteRequestSp<S> request)
        {
            return await ExecuteStoredProcSingleAsync<T>(request as DBExecuteRequest);
        }

        protected async Task<CommonResponse<T>> ExecuteStoredProcSingleAsync<T>(DBExecuteRequest request)
        {
            return await ExecuteStoredProcSingleAsync<T>(request.Schema, request.SpName, request.Request, request.Timeout);
        }

        protected async Task<CommonResponse<T>> ExecuteStoredProcSingleAsync<T>(S spName, object request, int? timeout = null)
        {
            return await ExecuteStoredProcSingleAsync<T>(DefaultSchema, spName, request, timeout);
        }

        protected async Task<CommonResponse<T>> ExecuteStoredProcSingleAsync<T>(string schema, S spName, object request, int? timeout = null)
        {
            return await ExecuteStoredProcSingleAsync<T>(schema, spName.ToString(), request, timeout);
        }

        protected async Task<CommonResponse<T>> ExecuteStoredProcSingleAsync<T>(string schema, string spName, object request, int? timeout = null)

        {
            var con = GetDbCon();

            try
            {
                var result = await ExecuteStoredProcAsync<T>(con,
                    GetSchemaName(schema), spName, request, _transaction, timeout);
                return new CommonResponse<T>() { Payload = result.FirstOrDefault() };
            }
            catch (Exception ex)
            {
                return new CommonResponse<T>() { Success = false, Error = new ErrorItem(ex.Message, ex) };
            }
            finally
            {
                if (_transaction == null)
                    con.Dispose();
            }
        }

        protected async Task<CommonResponse<string>> ExecuteStoredProcSingleAsJsonAsync(DBExecuteRequestSp<S> request)
        {
            return await ExecuteStoredProcSingleAsJsonAsync(request as DBExecuteRequest);
        }

        protected async Task<CommonResponse<string>> ExecuteStoredProcSingleAsJsonAsync(DBExecuteRequest request)
        {
            return await ExecuteStoredProcSingleAsJsonAsync(request.Schema, request.SpName, request.Request, request.Timeout);
        }

        protected async Task<CommonResponse<string>> ExecuteStoredProcSingleAsJsonAsync(string schema, string spName, object request, int? timeout = null)

        {
            var con = GetDbCon();
            try
            {
                var result = await ExecuteStoredProcAsync<dynamic>(con, GetSchemaName(schema), spName, request, _transaction, timeout);
                return new CommonResponse<string>() { Payload = TypeExtension.ToJson(result.FirstOrDefault()) };
            }
            catch (Exception ex)
            {
                return new CommonResponse<string>() { Success = false, Error = new ErrorItem(ex.Message, ex) };
            }
            finally
            {
                if (_transaction == null)
                    con.Dispose();
            }
        }

        protected async Task<CommonResponse<IEnumerable<T>>> ExecuteStoredProcListAsync<T>(DBExecuteRequestSp<S> request)
        {
            return await ExecuteStoredProcListAsync<T>(request as DBExecuteRequest);
        }

        protected async Task<CommonResponse<IEnumerable<T>>> ExecuteStoredProcListAsync<T>(DBExecuteRequest request)
        {
            return await ExecuteStoredProcListAsync<T>(request.Schema, request.SpName, request.Request, request.Timeout);
        }

        protected async Task<CommonResponse<IEnumerable<T>>> ExecuteStoredProcListAsync<T>(S spName, object request, int? timeout = null)
        {
            return await ExecuteStoredProcListAsync<T>(DefaultSchema, spName, request, timeout);

        }

        protected async Task<CommonResponse<IEnumerable<T>>> ExecuteStoredProcListAsync<T>(string schema, S spName, object request, int? timeout = null)
        {
           return await ExecuteStoredProcListAsync<T>(schema, spName.ToString(), request, timeout);
        }

        protected async Task<CommonResponse<IEnumerable<T>>> ExecuteStoredProcListAsync<T>(string schema, string spName, object request, int? timeout = null)
        {
            var con = GetDbCon();
            try
            {
                var result = await ExecuteStoredProcAsync<T>(con, GetSchemaName(schema), spName, request, _transaction, timeout);
                return new CommonResponse<IEnumerable<T>>() { Payload = result };
            }
            catch (Exception ex)
            {
                return new CommonResponse<IEnumerable<T>>() { Success = false, Error = new ErrorItem(ex.Message, ex) };
            }
            finally
            {
                if (_transaction == null)
                    con.Dispose();
            }
        }

        protected async Task<CommonResponse<IEnumerable<string>>> ExecuteStoredProcListAsJsonAsync(DBExecuteRequestSp<S> request)
        {
            return await ExecuteStoredProcListAsJsonAsync(request as DBExecuteRequest);
        }

        protected async Task<CommonResponse<IEnumerable<string>>> ExecuteStoredProcListAsJsonAsync(DBExecuteRequest request)
        {
            return await ExecuteStoredProcListAsJsonAsync(request.Schema, request.SpName, request.Request, request.Timeout);
        }

        protected async Task<CommonResponse<IEnumerable<string>>> ExecuteStoredProcListAsJsonAsync(string schema, string spName, object request, int? timeout = null)
        {
            var con = GetDbCon();
            try
            {
                var result = await ExecuteStoredProcAsync<dynamic>(con, GetSchemaName(schema), spName, request, _transaction, timeout);
                return new CommonResponse<IEnumerable<string>>() { Payload = result.ToList().Select(TypeExtension.ToJson) };
            }
            catch (Exception ex)
            {
                return new CommonResponse<IEnumerable<string>>() { Success = false, Error = new ErrorItem(ex.Message, ex) };
            }
            finally
            {
                if (_transaction == null)
                    con.Dispose();
            }
        }

        protected async Task<CommonResponse<T>> ExecuteStoredProcSingleJsonAsync<T>(DBExecuteRequestSp<S> request, params JsonConverter[] converters)
        {
            return await ExecuteStoredProcSingleJsonAsync<T>(request as DBExecuteRequest, converters);
        }

        protected async Task<CommonResponse<T>> ExecuteStoredProcSingleJsonAsync<T>(DBExecuteRequest request, params Newtonsoft.Json.JsonConverter[] converters)
        {
            return await ExecuteStoredProcSingleJsonAsync<T>(request.Schema, request.SpName, request.Request, request.Timeout, converters);
        }

        protected async Task<CommonResponse<T>> ExecuteStoredProcSingleJsonAsync<T>(S spName, object request, int? timeout = null, params Newtonsoft.Json.JsonConverter[] converters)
        {
            return await ExecuteStoredProcSingleJsonAsync<T>(DefaultSchema, spName, request, timeout, converters);
        }

        protected async Task<CommonResponse<T>> ExecuteStoredProcSingleJsonAsync<T>(string schema, S spName, object request, int? timeout = null, params Newtonsoft.Json.JsonConverter[] converters)
        {
            return await ExecuteStoredProcSingleJsonAsync<T>(schema, spName.ToString(), request, timeout, converters);
        }

        protected async Task<CommonResponse<T>> ExecuteStoredProcSingleJsonAsync<T>(string schema, string spName, object request, int? timeout = null, params Newtonsoft.Json.JsonConverter[] converters)
        {
            var result = await ExecuteStoredProcJsonListAsync<T>(schema, spName, request, timeout, converters);

            return new CommonResponse<T>()
            {
                Success = result.Success,
                Error = result.Error,
                Payload = (
                    (result.Success && result.Payload != null && result.Payload.Any())
                        ? result.Payload.First() : default(T)),
                Status = result.Status
            };
        }

        protected async Task<CommonResponse<IEnumerable<T>>> ExecuteStoredProcJsonListAsync<T>(DBExecuteRequestSp<S> request, params Newtonsoft.Json.JsonConverter[] converters)
        {
            return await ExecuteStoredProcJsonListAsync<T>(request as DBExecuteRequest, converters);
        }

        protected async Task<CommonResponse<IEnumerable<T>>> ExecuteStoredProcJsonListAsync<T>(DBExecuteRequest request, params Newtonsoft.Json.JsonConverter[] converters)
        {
            return await ExecuteStoredProcJsonListAsync<T>(request.Schema, request.SpName, request.Request, request.Timeout, converters);
        }

        protected async Task<CommonResponse<IEnumerable<T>>> ExecuteStoredProcJsonListAsync<T>(S spName, object request, int? timeout = null, params Newtonsoft.Json.JsonConverter[] converters)
        {
            return await ExecuteStoredProcJsonListAsync<T>(DefaultSchema, spName, request, timeout, converters);
        }

        protected async Task<CommonResponse<IEnumerable<T>>> ExecuteStoredProcJsonListAsync<T>(string schema, S spName, object request, int? timeout = null, params Newtonsoft.Json.JsonConverter[] converters)
        {
            return await ExecuteStoredProcJsonListAsync<T>(schema, spName.ToString(), request, timeout, converters);
        }

        protected async Task<CommonResponse<IEnumerable<T>>> ExecuteStoredProcJsonListAsync<T>(string schema, string spName, object request, int? timeout = null, params Newtonsoft.Json.JsonConverter[] converters)
        {
            var result = await ExecuteStoredProcListAsync<string>(schema, spName, request, timeout);

            if (result.Success && result.Payload != null && result.Payload.Any())
            {
                try
                {
                    return new CommonResponse<IEnumerable<T>>()
                    {
                        Payload = result.Payload.Select(r => ConvertJsonData<T>(r, converters)).ToList(),
                        Error = null,
                        Status = result.Status,
                        Success = true
                    };

                }
                catch (Exception ex)
                {
                    return new CommonResponse<IEnumerable<T>>()
                    {
                        Payload = null,
                        Error = new ErrorItem("Fail on conversion", ex),
                        Status = result.Status,
                        Success = false
                    };
                }
            }

            return new CommonResponse<IEnumerable<T>>()
            {
                Payload = null,
                Error = result.Error,
                Status = result.Status,
                Success = false
            };
        }

        protected virtual IDbConnection GetDbCon()
        {
            if (_transaction is {Connection: { }} && !string.IsNullOrWhiteSpace(_transaction.Connection.ConnectionString))
                return _transaction.Connection;

            return GetDb(DBConnStr());
        }

        protected abstract string DBConnStr();

        #endregion


        #region Private Methods

        private T ConvertJsonData<T>(string rawData, params Newtonsoft.Json.JsonConverter[] converters)
        {
            try
            {
                if (converters != null && converters.Length != 0)
                {
                    return JsonConvert.DeserializeObject<T>(rawData, converters);
                }

                return JsonConvert.DeserializeObject<T>(rawData);
            }
            catch (Exception ex)
            {
                GetLogger.Error("Conversion error, data: " + rawData, ex);
                return default(T);
            }
        }

        private string GetSchemaName(string schema)
        {
            return schema.Equals("UseDefaultSchema") ? DefaultSchema : schema;
        }

        #endregion
    }
}
