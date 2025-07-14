namespace DapperAuthApi.Interfaces
{
    public interface IRepositoryGetbyId<T,D>
    {
        Task<List<T>> GetById(D id);
    }
}
