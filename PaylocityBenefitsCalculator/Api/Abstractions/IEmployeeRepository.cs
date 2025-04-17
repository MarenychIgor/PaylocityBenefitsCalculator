using Api.Models;

namespace Api.Abstractions
{
    public interface IEmployeeRepository
    {
        Task<Employee?> Get(int id);

        Task<List<Employee>> GetAll();
    }
}
