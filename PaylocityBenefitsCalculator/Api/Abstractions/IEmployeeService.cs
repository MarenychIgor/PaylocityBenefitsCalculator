using Api.Dtos.Employee;

namespace Api.Abstractions
{
    public interface IEmployeeService
    {
        Task<GetEmployeeDto?> Get(int id);

        Task<List<GetEmployeeDto>> GetAll();
    }
}
