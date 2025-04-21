namespace Api.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<T?> Get(int id);

        Task<List<T>> GetAll();
    }
}
