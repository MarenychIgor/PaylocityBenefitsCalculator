using Api.Dtos.Paycheck;

namespace Api.Abstractions
{
    public interface IPaycheckCalculationService
    {
        Task<GetPaycheckDto?> Calculate(int employeeId, DateTime paycheckDate);
    }
}
