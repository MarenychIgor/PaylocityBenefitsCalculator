using Api.Models;

namespace Api.Abstractions
{
    // Point of future deduction logic extension.
    // If you need to add new benefit - implement this interface, and that's it.
    public interface IDeductionPlugin
    {
        decimal CalculateDeduction(Employee employee, DateTime paycheckDate);
    }
}
