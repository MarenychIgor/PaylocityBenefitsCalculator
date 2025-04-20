using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Abstractions;
using Api.Extensions;
using Api.Models;
using Api.Services;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace ApiTests
{
    public class PaycheckCalculationServiceTests
    {
        private const int NUMBER_OF_PAYCHECKS = 26;
        private const decimal MONTLY_BASE_BENEFIT_COST = 1000;
        private const decimal SALARY_ABOVE_TRESHOLD_COEFFICIENT = 0.02M;
        private const decimal DEPENDENT_MONTLY_BASE_BENEFIT_COST = 600;
        private const decimal DEPENDENT_AGE_OVER_TRESHOLD_MONTLY_BENEFIT_ADDED_COST = 200;

        private readonly IEmployeeRepository _repository;

        public PaycheckCalculationServiceTests()
            => _repository = Substitute.For<IEmployeeRepository>();

        [Fact]
        public async Task NoEmployeeFound_ReturnNull()
        {
            // Arrange
            _repository.Get(Arg.Any<int>()).ReturnsNull();
            var service = new PaycheckCalculationService(_repository);

            // Act
            var result = await service.Calculate(1, DateTime.UtcNow);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SalaryBelowTreshold_DeductBaseBenefit()
        {
            // Arrange
            var employeeId = 1;
            var yearlySalary = 70000M;
            
            var deduction = (MONTLY_BASE_BENEFIT_COST * 12 / NUMBER_OF_PAYCHECKS).Round();
            var salary = (yearlySalary / NUMBER_OF_PAYCHECKS).Round();
            var netSalary = salary - deduction;

            var employee = new Employee { Id = employeeId, Salary = yearlySalary };
            _repository.Get(employeeId).Returns(employee);
            var service = new PaycheckCalculationService(_repository);

            // Act
            var result = await service.Calculate(employeeId, DateTime.UtcNow);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(salary, result!.GrossAmount);
            Assert.Equal(netSalary, result.NetAmmount);
            Assert.Equal(deduction, result.DeductedAmount);
        }

        [Fact]
        public async Task SalaryAboveTreshold_DeductAdjustedBaseBenefit()
        {
            // Arrange
            var employeeId = 1;
            var yearlySalary = 90000M;

            var yearlyDeduction = (MONTLY_BASE_BENEFIT_COST * 12) + (yearlySalary * SALARY_ABOVE_TRESHOLD_COEFFICIENT);
            var deduction = (yearlyDeduction / NUMBER_OF_PAYCHECKS).Round();
            var salary = (yearlySalary / NUMBER_OF_PAYCHECKS).Round();
            var netSalary = salary - deduction;

            var employee = new Employee { Id = employeeId, Salary = yearlySalary };
            _repository.Get(employeeId).Returns(employee);
            var service = new PaycheckCalculationService(_repository);

            // Act
            var result = await service.Calculate(employeeId, DateTime.UtcNow);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(salary, result!.GrossAmount);
            Assert.Equal(netSalary, result.NetAmmount);
            Assert.Equal(deduction, result.DeductedAmount);
        }

        [Fact]
        public async Task EmployeeHasTwoDependents_DeductDependentsBenefit()
        {
            var employeeId = 1;
            var yearlySalary = 50000M;
            var dependentsUnder50 = 2;
            var dependentsOver50 = 1;
            var employeeDeductionName = "Employee benefit cost";
            var dependentsDeductionName = "Dependents benefit cost";

            var employeeDeduction = (MONTLY_BASE_BENEFIT_COST * 12 / NUMBER_OF_PAYCHECKS).Round();
            var dependentsDeduction = GetEmployeeDeductions(dependentsUnder50, dependentsOver50);
            var salary = (yearlySalary / NUMBER_OF_PAYCHECKS).Round();
            var netSalary = salary - (employeeDeduction + dependentsDeduction);

            var employee = GetEmployeeWithDependents(employeeId, yearlySalary);
            _repository.Get(employeeId).Returns(employee);

            var service = new PaycheckCalculationService(_repository);

            // Act
            var result = await service.Calculate(employeeId, DateTime.UtcNow);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(netSalary, result!.NetAmmount);

            var employeeDeductionTranscript = result.Deductions.Single(x => x.Name.Equals(employeeDeductionName, StringComparison.OrdinalIgnoreCase));
            Assert.Equal(employeeDeduction, employeeDeductionTranscript.Amount);

            var dependentsDeductionTranscript = result.Deductions.Single(x => x.Name.Equals(dependentsDeductionName, StringComparison.OrdinalIgnoreCase));
            Assert.Equal(dependentsDeduction, dependentsDeductionTranscript.Amount);
        }

        [Theory]
        [MemberData(nameof(PaycheckAndDependentDobData))]
        public async Task DependentDoBOnOrBeforePaycheckDate_DeductAdjustedBenefir(DateTime paycheckDate, DateTime dependentDoB)
        {
            // Arrange
            var employeeId = 1;
            var yearlySalary = 50000M;

            var dependent = new Dependent
            {
                Id = 1,
                DateOfBirth = dependentDoB,
                EmployeeId = employeeId
            };
            
            var employee = new Employee
            { 
                Id = employeeId,
                Salary = yearlySalary,
                Dependents = new List<Dependent> { dependent }
            };
            _repository.Get(employeeId).Returns(employee);

            var service = new PaycheckCalculationService(_repository);

            // Act
            var result = await service.Calculate(employeeId, paycheckDate);

            // Assert
            Assert.NotNull(result);

            var expectedDependentsDeduction = GetEmployeeDeductions(dependentsUnder50: 0, dependentsOver50: 1);
            var dependentTranscript = result!.Deductions.Single(x => x.Name.Equals("Dependents benefit cost", StringComparison.OrdinalIgnoreCase));
            Assert.Equal(expectedDependentsDeduction, dependentTranscript.Amount);
        }

        private Employee GetEmployeeWithDependents(int employeeId, decimal yearlySalary)
        {
            var dependents = new List<Dependent>
            {
                new ()
                {
                    Id = 1,
                    DateOfBirth = new DateTime(2000, 05, 18),
                    Relationship = Relationship.Child,
                    EmployeeId = employeeId
                },
                new ()
                {
                    Id = 2,
                    DateOfBirth = new DateTime(2003, 07, 05),
                    Relationship = Relationship.Child,
                    EmployeeId = employeeId
                },
                new ()
                {
                    Id = 3,
                    DateOfBirth = new DateTime(1970, 12, 30),
                    Relationship = Relationship.Spouse,
                    EmployeeId = employeeId
                }
            };

            return new Employee
            {
                Id = employeeId,
                Salary = yearlySalary,
                Dependents = dependents
            };
        }

        private decimal GetEmployeeDeductions(int dependentsUnder50, int dependentsOver50)
        {
            var dependentsUnder50MontlyDeduction = DEPENDENT_MONTLY_BASE_BENEFIT_COST * 12 / NUMBER_OF_PAYCHECKS;
            var dependentsUnder50Deduction = dependentsUnder50MontlyDeduction * dependentsUnder50;
            var dependentsOver50MontlyDeduction = (DEPENDENT_MONTLY_BASE_BENEFIT_COST + DEPENDENT_AGE_OVER_TRESHOLD_MONTLY_BENEFIT_ADDED_COST) * 12 / NUMBER_OF_PAYCHECKS;
            var dependentsOver50Deduction = dependentsOver50MontlyDeduction * dependentsOver50;
            var totalDependentsDeduction = (dependentsUnder50Deduction + dependentsOver50Deduction).Round();
            
            return totalDependentsDeduction;
        }

        public static IEnumerable<object[]> PaycheckAndDependentDobData =>
            new List<object[]>
            {
                new object[] { new DateTime(2025, 04, 19), new DateTime(1975, 04, 19) },
                new object[] { new DateTime(2025, 05, 19), new DateTime(1975, 04, 19) }
            };
    }
}
