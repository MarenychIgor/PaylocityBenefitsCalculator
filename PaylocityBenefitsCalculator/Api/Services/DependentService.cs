using Api.Abstractions;
using Api.Dtos.Dependent;
using AutoMapper;

namespace Api.Services
{
    // Possible to make service generic to reduce code duplication
    public class DependentService : IDependentService
    {
        private readonly IDependentRepository _repository;
        private readonly IMapper _mapper;

        public DependentService(IDependentRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetDependentDto?> Get(int id)
        {
            var dependent = await _repository.Get(id);

            return _mapper.Map<GetDependentDto>(dependent);
        }

        public async Task<List<GetDependentDto>> GetAll()
        {
            var dependents = await _repository.GetAll();

            return _mapper.Map<List<GetDependentDto>>(dependents);
        }
    }
}
