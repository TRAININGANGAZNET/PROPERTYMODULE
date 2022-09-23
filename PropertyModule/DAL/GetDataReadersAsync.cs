using Dapper;
using Microsoft.Extensions.Configuration;
using PropertyModule.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyModule.DAL
{
    public class GetDataReadersAsync: IGetDataReadersAsync
    {
        private readonly IConfiguration _configuration;
        public string myConnectionString = string.Empty;

        public GetDataReadersAsync(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public PropertyResponse Saveproperty<T, U>(string func, U Parameters, string myConnectionString)
        {
            PropertyResponse Reslist = new PropertyResponse();
            try
            {
                Reslist.id = SaveDataScalar<T, U>(Parameters, myConnectionString, func).ToString();
                if (Reslist.id != null)
                {
                    Reslist.Responses = _configuration["Responses:AccountSucess"];
                }
            }
            catch (Exception ex)
            {
                Reslist.Responses = _configuration["Responses:FailMessage"];
            }
            return (Reslist);
        }
        public object SaveDataScalar<T, U>(U Parameters, string connectionstring, string spname)
        {
            object obj = new object();
            using (IDbConnection connection = new SqlConnection(connectionstring))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                using (IDbTransaction _dbTransaction = connection.BeginTransaction())
                {
                    try
                    {
                        obj = connection.ExecuteScalar(spname, Parameters, commandType: CommandType.StoredProcedure);
                        _dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _dbTransaction.Rollback();
                    }
                    return obj;
                }
            }
        }


        public async Task<IEnumerable<T>> GetChildDataAsync<T, U>(string sql,  string connectionstring)
        {
            List<T> DataList = new List<T>();
            try
            {
                DataList = await LoadData<T, dynamic>(sql,  connectionstring);
            }
            catch (Exception ex)
            {
                return null;
            }
            return DataList;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, string connectionstring)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionstring))
                {
                    var rows = await connection.QueryAsync<T>(sql);
                    return rows.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<IEnumerable<T>> GetChildDataAsync<T, U>(string sql, U param, string connectionstring)
        {
            List<T> DataList = new List<T>();
            try
            {
                DataList = await LoadData<T, dynamic>(sql, param, connectionstring);
            }
            catch (Exception ex)
            {
                return null;
            }
            return DataList;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U Parameters, string connectionstring)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionstring))
                {
                    var rows = await connection.QueryAsync<T>(sql, Parameters);
                    return rows.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }


    }
}
