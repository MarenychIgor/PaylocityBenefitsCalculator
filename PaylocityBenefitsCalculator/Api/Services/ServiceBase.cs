using Api.Abstractions;
using AutoMapper;

namespace Api.Services
{
    public class ServiceBase<TModel, TDto, TRepository> 
        where TModel : class
        where TDto : class
        where TRepository : IRepository<TModel>
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;

        public ServiceBase(TRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public virtual async Task<TDto?> Get(int id)
        {
            var record = await _repository.Get(id);
            return _mapper.Map<TDto>(record);
        }

        public virtual async Task<List<TDto>> GetAll()
        {
            var records = await _repository.GetAll();
            return _mapper.Map<List<TDto>>(records);
        }
    }
}
