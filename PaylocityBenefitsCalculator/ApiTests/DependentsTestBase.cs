using Api.Extensions;

namespace ApiTests
{
    public class DependentsTestBase
    {
        private const int NUMBER_OF_PAYCHECKS = 26;
        private const decimal DEPENDENT_MONTLY_BASE_BENEFIT_COST = 600;
        private const decimal DEPENDENT_AGE_OVER_TRESHOLD_MONTLY_BENEFIT_ADDED_COST = 200;

        internal decimal GetEmployeeDeductions(int dependentsUnder50, int dependentsOver50)
        {
            var dependentsUnder50MontlyDeduction = DEPENDENT_MONTLY_BASE_BENEFIT_COST * 12 / NUMBER_OF_PAYCHECKS;
            var dependentsUnder50Deduction = dependentsUnder50MontlyDeduction * dependentsUnder50;
            var dependentsOver50MontlyDeduction = (DEPENDENT_MONTLY_BASE_BENEFIT_COST + DEPENDENT_AGE_OVER_TRESHOLD_MONTLY_BENEFIT_ADDED_COST) * 12 / NUMBER_OF_PAYCHECKS;
            var dependentsOver50Deduction = dependentsOver50MontlyDeduction * dependentsOver50;
            var totalDependentsDeduction = (dependentsUnder50Deduction + dependentsOver50Deduction).Round();

            return totalDependentsDeduction;
        }
    }
}
