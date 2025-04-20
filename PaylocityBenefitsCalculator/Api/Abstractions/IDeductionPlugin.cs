using Api.Models;

namespace Api.Abstractions
{
    public interface IDeductionPlugin
    {
        decimal CalculateDeduction(Employee employee, DateTime paycheckDate);
    }
}
