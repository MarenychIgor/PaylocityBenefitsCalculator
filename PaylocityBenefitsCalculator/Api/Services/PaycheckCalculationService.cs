using Api.Abstractions;
using Api.Dtos.Paycheck;
using Api.Extensions;
using Api.Models;

namespace Api.Services
{
    public class PaycheckCalculationService : IPaycheckCalculationService
    {
        private const int NUMBER_OF_PAYCHECKS = 26;
        private const decimal MONTLY_BASE_BENEFIT_COST = 1000;
        private const decimal YEARLY_SALARY_TRESHOLD_VALUE = 80000;
        private const decimal SALARY_ABOVE_TRESHOLD_COEFFICIENT = 0.02M;
        private const decimal DEPENDENT_MONTLY_BASE_BENEFIT_COST = 600;
        private const int DISCOUNTED_DEPENDENT_AGE_TRESHOLD = 50;
        private const decimal DEPENDENT_AGE_OVER_TRESHOLD_MONTLY_BENEFIT_ADDED_COST = 200;
        
        private readonly IEmployeeRepository _repository;

        public PaycheckCalculationService(IEmployeeRepository repository)
            => _repository = repository;

        public async Task<GetPaycheckDto?> Calculate(int employeeId, DateTime paycheckDate)
        {
            var employee = await _repository.Get(employeeId);
            if(employee == null)
            {
                return null; 
            }

            var baseBenefitCost = (GetBaseBenefitCost(employee) / NUMBER_OF_PAYCHECKS).Round();
            var dependentsBenefitCost = (GetDependentsBenefitCost(employee.Dependents, paycheckDate) / NUMBER_OF_PAYCHECKS).Round();

            var deductedAmount = baseBenefitCost + dependentsBenefitCost;
            var grossSalary = (employee.Salary / NUMBER_OF_PAYCHECKS).Round();
            var netSalary = grossSalary - deductedAmount;

            return new GetPaycheckDto
            {
                Date = paycheckDate,
                EmployeeFirstName = employee.FirstName!,
                EmployeeLastName = employee.LastName!,
                GrossAmount = grossSalary,
                NetAmmount = netSalary,
                Deductions = GetDeductionTranscript(baseBenefitCost, dependentsBenefitCost),
                DeductedAmount = deductedAmount
            };
        }

        private decimal GetBaseBenefitCost(Employee employee)
        {
            var baseBenefitCost = default(decimal);
            if (employee.Salary > YEARLY_SALARY_TRESHOLD_VALUE)
            {
                baseBenefitCost = employee.Salary * SALARY_ABOVE_TRESHOLD_COEFFICIENT;
            }

            return MONTLY_BASE_BENEFIT_COST * 12 + baseBenefitCost;
        }

        private decimal GetDependentsBenefitCost(ICollection<Dependent> dependents, DateTime paycheckDate)
        {
            var totalBenefitConst = default(decimal);
            foreach (var dependent in dependents)
            {
                totalBenefitConst += DEPENDENT_MONTLY_BASE_BENEFIT_COST;

                var age = GetDependentAge(dependent.DateOfBirth, paycheckDate);
                if(age >= DISCOUNTED_DEPENDENT_AGE_TRESHOLD)
                {
                    totalBenefitConst += DEPENDENT_AGE_OVER_TRESHOLD_MONTLY_BENEFIT_ADDED_COST;
                }
            }

            return totalBenefitConst * 12;
        }

        private List<Deduction> GetDeductionTranscript(decimal baseBenefitCost, decimal dependentsBenefitCost)
        {
            var deductions = new List<Deduction> { new() { Name = "Employee benefit cost", Amount = baseBenefitCost } };
            if(dependentsBenefitCost != default)
            {
                deductions.Add(new Deduction { Name = "Dependents benefit cost", Amount = dependentsBenefitCost });
            }

            return deductions;
        }

        private int GetDependentAge(DateTime dateOfBirth, DateTime paycheckDate)
        {
            var roughAge = paycheckDate.Year - dateOfBirth.Year;

            return paycheckDate.Month > dateOfBirth.Month || (paycheckDate.Month == dateOfBirth.Month && paycheckDate.Day >= dateOfBirth.Day)
                ? roughAge + 1
                : roughAge;
        }
    }
}
