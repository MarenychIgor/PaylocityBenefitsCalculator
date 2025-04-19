using Api.Abstractions;
using Api.Dtos.Employee;
using AutoMapper;

namespace Api.Services
{
    // Possible to make service generic to reduce code duplication
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetEmployeeDto?> Get(int id)
        {
            var employee = await _repository.Get(id);

            return _mapper.Map<GetEmployeeDto>(employee);
        }

        public async Task<List<GetEmployeeDto>> GetAll()
        {
            var employees = await _repository.GetAll();
            
            return _mapper.Map<List<GetEmployeeDto>>(employees);
        }
    }
}
