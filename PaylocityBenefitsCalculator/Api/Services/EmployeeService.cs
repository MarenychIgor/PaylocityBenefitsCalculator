using Api.Abstractions;
using Api.Dtos.Employee;
using Api.Models;
using AutoMapper;

namespace Api.Services
{
    // Service is generic to avoid code duplication.
    public class EmployeeService : ServiceBase<Employee, GetEmployeeDto, IEmployeeRepository>, IEmployeeService
    {
        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
