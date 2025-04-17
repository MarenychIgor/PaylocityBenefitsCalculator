using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using AutoMapper;

namespace Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, GetEmployeeDto>().ReverseMap();
            CreateMap<Dependent, GetDependentDto>().ReverseMap();
        }
    }
}
