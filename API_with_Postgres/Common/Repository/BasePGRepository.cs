using API_with_Postgres.Common.Logging;
using Dapper;
using Npgsql;
using System.Data;
using System.Dynamic;

namespace Common.Library.Collection.Dapper
{
    public abstract class BasePGRepository : LoggingService
    {
        #region Constructor

        protected BasePGRepository(Type concreteType) : base(concreteType)
        {
        }

        #endregion

        #region Protected Methods

        protected static IDbConnection GetDb(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        protected static IEnumerable<T> QueryExecute<T>(IDbConnection con, string sql, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            return con.Query<T>(sql, AppendParamOnRequestObject(param), transaction, commandTimeout: timeout);
        }

        protected static async Task<IEnumerable<T>> QueryExecuteAsync<T>(IDbConnection con, string sql, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            return await con.QueryAsync<T>(sql, AppendParamOnRequestObject(param), transaction, timeout);
        }

        protected static IEnumerable<T> ExecuteStoredProc<T>(IDbConnection con, string spName, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            return ExecuteStoredProc<T>(con, string.Empty, spName,param, transaction, timeout);
        }

        protected static IEnumerable<T> ExecuteStoredProc<T>(IDbConnection con, string schema, string spName, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            return con.Query<T>(BuildSpName(schema, spName), AppendParamOnRequestObject(param), transaction, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
        }

        protected static async Task<IEnumerable<T>> ExecuteStoredProcAsync<T>(IDbConnection con, string spName, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            return await ExecuteStoredProcAsync<T>(con, string.Empty, spName, param, transaction, timeout);
        }

        protected static async Task<IEnumerable<T>> ExecuteStoredProcAsync<T>(IDbConnection con, string schema, string spName, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            return await con.QueryAsync<T>(BuildSpName(schema, spName), AppendParamOnRequestObject(param), transaction, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
        }

        protected static async Task ExecuteStoredProcAsync(IDbConnection con, string spName, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            await ExecuteStoredProcAsync(con, string.Empty, spName, param, transaction, timeout);
        }

        protected static async Task ExecuteStoredProcAsync(IDbConnection con, string schema, string spName, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            await con.ExecuteAsync(BuildSpName(schema, spName), AppendParamOnRequestObject(param), transaction, timeout, CommandType.StoredProcedure);
        }

        #endregion


        #region Private Methods

        private static ExpandoObject AppendParamOnRequestObject(object param)
        {
            if (param == null)
                return null;

            var resultRequest = new ExpandoObject();

            if (param is ExpandoObject expandoObject)
            {
                foreach (var (key, value) in expandoObject)
                {
                    resultRequest.TryAdd("p_" + key.ToLower(), value);
                }
            }
            else
            {
                foreach (var propertyInfo in param.GetType().GetProperties())
                {
                    resultRequest.TryAdd("p_" + propertyInfo.Name.ToLower(), propertyInfo.GetValue(param));
                }
            }


            return resultRequest;
        }

        private static string BuildSpName(string schema, string spName)
        {
            if (!string.IsNullOrWhiteSpace(schema))
            {
                return $"\"{schema}\".\"{spName}\"";
            }

            return $"public.\"{spName}\"";
        }

        #endregion
    }
}
