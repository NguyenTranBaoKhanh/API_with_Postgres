using API_with_Postgres.Common.Contract;
using API_with_Postgres.Common.Enums;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Dynamic;

namespace Common.Library.Collection.Dapper
{
    public abstract class BaseRepository_Old
    {
        #region Constructor

        //protected BaseRepository(Type concreteType) : base(concreteType)
        //{
        //}
        protected BaseRepository_Old()
        {
            
        }
        #endregion


        #region Protected Methods

        protected static IDbConnection GetDb(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        protected static IEnumerable<T> QueryExecute<T>(IDbConnection con, string sql, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            return con.Query<T>(sql, param, transaction, commandTimeout: timeout);
        }

        protected static async Task<IEnumerable<T>> QueryExecuteAsync<T>(IDbConnection con, string sql, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            return await con.QueryAsync<T>(sql, param, transaction, timeout);
        }

        protected static IEnumerable<T> ExecuteStoredProc<T>(IDbConnection con, string spName, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            return con.Query<T>(spName, param, transaction, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
        }

        protected static async Task<IEnumerable<T>> ExecuteStoredProcAsync<T>(IDbConnection con, string spName, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            return await con.QueryAsync<T>(spName, param, transaction, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
        }

        protected static async Task ExecuteStoredProcAsync(IDbConnection con, string spName, object param = null, IDbTransaction transaction = null, int? timeout = null)
        {
            await con.ExecuteAsync(spName, param, transaction, timeout, CommandType.StoredProcedure);
        }

        protected static async Task<dynamic> ExecuteQueryMultipleAsync(IDbConnection con, string spName, object param = null, IDbTransaction transaction = null, IEnumerable<MapItem> mapItems = null, int? timeout = null)
        {
            var data = new ExpandoObject();

            using (var multi = await con.QueryMultipleAsync(spName, param, transaction, commandType: CommandType.StoredProcedure, commandTimeout: timeout))
            {
                if (mapItems == null) return data;

                foreach (var item in mapItems)
                {
                    if (item.DataRetrieveType == DataRetrieveType.FirstOrDefault)
                    {
                        var singleItem = multi.Read(item.Type).FirstOrDefault();
                        ((IDictionary<string, object>)data).Add(item.PropertyName, singleItem);
                    }

                    if (item.DataRetrieveType == DataRetrieveType.List)
                    {
                        var listItem = multi.Read(item.Type).ToList();
                        ((IDictionary<string, object>)data).Add(item.PropertyName, listItem);
                    }
                }

                return data;
            }
        }

        protected static async Task ExecuteBulkInsert(string connectionString, string tableName, DataTable data, int batchSize = 0, int timeout = 120)
        {
            using (var dbCon = new SqlConnection(connectionString))
            {
                dbCon.Open();

                using (var s = new SqlBulkCopy(dbCon))
                {
                    s.DestinationTableName = tableName;
                    s.BatchSize = batchSize;
                    s.BulkCopyTimeout = timeout;

                    foreach (var column in data.Columns)
                    {
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    }

                    await s.WriteToServerAsync(data);
                }
            }
        } 

        #endregion
    }
}
