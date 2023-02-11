namespace PictureLibrary.DataAccess.DatabaseAccess
{
    public interface IDatabaseAccess<TModel> 
        where TModel : class
    {
        Task<IEnumerable<TModel>> LoadDataAsync(string sql, object parameters);
        Task SaveDataAsync<TParameters>(string sql, TParameters parameters) where TParameters : class;
    }
}