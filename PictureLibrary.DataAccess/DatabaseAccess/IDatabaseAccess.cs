namespace PictureLibrary.DataAccess.DatabaseAccess
{
    public interface IDatabaseAccess<TModel> 
        where TModel : class
    {
        Task<IEnumerable<TModel>> LoadDataAsync(string sql, object parameters);
        Task<IEnumerable<TModel>> LoadDataAsync<TFirst, TSecond>(string sql, Func<TFirst, TSecond, TModel> map, object? parameters = null);
        Task SaveDataAsync<TParameters>(string sql, TParameters parameters) where TParameters : class;
    }
}