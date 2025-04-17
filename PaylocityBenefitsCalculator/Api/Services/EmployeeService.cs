using Api.Abstractions;
using Api.Dtos.Employee;
using AutoMapper;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetEmployeeDto?> Get(int id)
        {
            var employee = await _employeeRepository.Get(id);

            return employee != null ? 
                _mapper.Map<GetEmployeeDto>(employee) :
                null;
        }

        public async Task<List<GetEmployeeDto>> GetAll()
        {
            var employees = await _employeeRepository.GetAll();
            
            return _mapper.Map<List<GetEmployeeDto>>(employees);
        }
    }
}
