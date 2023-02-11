namespace PictureLibrary.DataAccess.DatabaseAccess
{
    public interface IDatabaseAccess<TModel> 
        where TModel : class
    {
        Task<IEnumerable<TModel>> LoadDataAsync(string storedProcedure, object parameters);
        Task SaveDataAsync<TParameters>(string storedProcedure, TParameters parameters) where TParameters : class;
    }
}