using Api.Abstractions;
using Api.Extensions;
using Api.Models;

namespace Api.DeductionPlugins
{
    public class DependentsDeductionPlugin : IDeductionPlugin
    {
        // TODO: move to config
        private const int NUMBER_OF_PAYCHECKS = 26;
        private const decimal DEPENDENT_MONTLY_BASE_BENEFIT_COST = 600;
        private const int DISCOUNTED_DEPENDENT_AGE_TRESHOLD = 50;
        private const decimal DEPENDENT_AGE_OVER_TRESHOLD_MONTLY_BENEFIT_ADDED_COST = 200;

        public decimal CalculateDeduction(Employee employee, DateTime paycheckDate)
            => (GetDependentsBenefitCost(employee.Dependents, paycheckDate) / NUMBER_OF_PAYCHECKS).Round();

        private decimal GetDependentsBenefitCost(ICollection<Dependent> dependents, DateTime paycheckDate)
        {
            var totalBenefitConst = default(decimal);
            foreach (var dependent in dependents)
            {
                totalBenefitConst += DEPENDENT_MONTLY_BASE_BENEFIT_COST;

                var age = GetDependentAge(dependent.DateOfBirth, paycheckDate);
                if (age >= DISCOUNTED_DEPENDENT_AGE_TRESHOLD)
                {
                    totalBenefitConst += DEPENDENT_AGE_OVER_TRESHOLD_MONTLY_BENEFIT_ADDED_COST;
                }
            }

            return totalBenefitConst * 12;
        }

        // We can't rely on simple age logic calculation, because dependend birthday may be after paycheck date.
        private int GetDependentAge(DateTime dateOfBirth, DateTime paycheckDate)
        {
            var roughAge = paycheckDate.Year - dateOfBirth.Year;

            return paycheckDate.Month < dateOfBirth.Month || (paycheckDate.Month == dateOfBirth.Month && paycheckDate.Day < dateOfBirth.Day)
                ? roughAge - 1
                : roughAge;
        }
    }
}
