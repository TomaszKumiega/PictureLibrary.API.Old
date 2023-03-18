namespace PictureLibrary.DataAccess.DatabaseAccess
{
    public interface IDatabaseAccess<TModel> 
        where TModel : class
    {
        Task<IEnumerable<TModel>> LoadDataAsync(string sql, object parameters);
        Task<IEnumerable<(TFirst First, TSecond Second)>> LoadDataAsync<TFirst, TSecond>(
            string sql,
            Func<TFirst, TSecond, (TFirst, TSecond)> map,
            object? parameters = null);
        Task SaveDataAsync<TParameters>(string sql, TParameters parameters) where TParameters : class;
    }
}