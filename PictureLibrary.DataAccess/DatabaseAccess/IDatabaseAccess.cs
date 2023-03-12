namespace PictureLibrary.DataAccess.DatabaseAccess
{
    public interface IDatabaseAccess<TModel> 
        where TModel : class
    {
        Task<IEnumerable<TModel>> LoadDataAsync(string sql, object parameters);
        Task<IEnumerable<TModel>> LoadDataAsync<TFirst, TSecond>(string sql, Type[] types, Func<object[], TModel> map, object? parameters = null);
        Task SaveDataAsync<TParameters>(string sql, TParameters parameters) where TParameters : class;
    }
}