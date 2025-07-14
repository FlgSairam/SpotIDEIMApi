namespace DapperAuthApi.Interfaces
{
    public interface IRepositoryInsert<T, U>
    {
        Task<T> InsertAsync(U entity);
    }
}
