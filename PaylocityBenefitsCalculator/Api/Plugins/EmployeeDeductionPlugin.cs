using Api.Abstractions;
using Api.Extensions;
using Api.Models;

namespace Api.DeductionPlugins
{
    public class EmployeeDeductionPlugin : IDeductionPlugin
    {
        // TODO: move to config
        private const int NUMBER_OF_PAYCHECKS = 26;
        private const decimal MONTLY_BASE_BENEFIT_COST = 1000;
        private const decimal YEARLY_SALARY_TRESHOLD_VALUE = 80000;
        private const decimal SALARY_ABOVE_TRESHOLD_COEFFICIENT = 0.02M;

        public decimal CalculateDeduction(Employee employee, DateTime _)
            => (GetBaseBenefitCost(employee) / NUMBER_OF_PAYCHECKS).Round();

        private static decimal GetBaseBenefitCost(Employee employee)
        {
            var baseBenefitCost = default(decimal);
            if (employee.Salary > YEARLY_SALARY_TRESHOLD_VALUE)
            {
                baseBenefitCost = employee.Salary * SALARY_ABOVE_TRESHOLD_COEFFICIENT;
            }

            return MONTLY_BASE_BENEFIT_COST * 12 + baseBenefitCost;
        }
    }
}
