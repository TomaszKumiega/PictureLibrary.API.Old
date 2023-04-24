using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace PictureLibrary.DataAccess.DatabaseAccess
{
    public class DatabaseAccess<TModel> : IDatabaseAccess<TModel> 
        where TModel : class
    {
        private const string _connectionString = @"Data Source=.\PictureLibrary.db;Version=3;";

        public async Task<IEnumerable<T>> LoadDataAsync<T>(string sql, object parameters)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<T>(sql, parameters, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<TModel>> LoadDataAsync(string sql, object parameters)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TModel>(sql, parameters, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<(TFirst, TSecond)>> LoadDataAsync<TFirst, TSecond>(
            string sql, 
            Func<TFirst, TSecond, (TFirst, TSecond)> map, 
            object? parameters = null)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync(sql, map, parameters, commandType: CommandType.Text);
        }

        public async Task SaveDataAsync<TParameters>(string sql, TParameters parameters)
            where TParameters : class
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, parameters, commandType: CommandType.Text);
        }
    }
}
