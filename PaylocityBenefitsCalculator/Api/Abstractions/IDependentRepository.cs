using Api.Models;

namespace Api.Abstractions
{
    public interface IDependentRepository
    {
        Task<Dependent?> Get(int id);

        Task<List<Dependent>> GetAll();
    }
}
