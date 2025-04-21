using Api.Abstractions;
using Api.Dtos.Dependent;
using Api.Models;
using AutoMapper;

namespace Api.Services
{
    // Service is generic to avoid code duplication.
    public class DependentService : ServiceBase<Dependent, GetDependentDto, IDependentRepository>, IDependentService
    {
        public DependentService(IDependentRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
