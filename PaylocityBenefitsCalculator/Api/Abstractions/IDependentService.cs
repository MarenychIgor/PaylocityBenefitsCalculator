using Api.Dtos.Dependent;

namespace Api.Abstractions
{
    public interface IDependentService
    {
        Task<GetDependentDto?> Get(int id);

        Task<List<GetDependentDto>> GetAll();
    }
}
