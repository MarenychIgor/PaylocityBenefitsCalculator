using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Abstractions;
using Api.DeductionPlugins;
using Api.Extensions;
using Api.Models;
using Api.Services;
using ApiTests.Helpers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace ApiTests.UnitTests
{
    public class PaycheckCalculationServiceTests : DependentsBenefitCalculationHelper
    {
        private const int NUMBER_OF_PAYCHECKS = 26;
        private const decimal MONTLY_BASE_BENEFIT_COST = 1000;

        private readonly IEmployeeRepository _repository;
        private readonly IDeductionPluginsProvider _pluginsProvider;

        public PaycheckCalculationServiceTests()
        {
            _repository = Substitute.For<IEmployeeRepository>();
            _pluginsProvider = Substitute.For<IDeductionPluginsProvider>();
        }

        [Fact]
        public async Task NoEmployeeFound_ReturnNull()
        {
            // Arrange
            _repository.Get(Arg.Any<int>()).ReturnsNull();
            var service = GetService();

            // Act
            var result = await service.Calculate(1, DateTime.UtcNow);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task EmployeeHasMultipleDependents_DeductDependentsBenefit()
        {
            var employeeId = 1;
            var yearlySalary = 50000M;
            var dependentsUnder50 = 2;
            var dependentsOver50 = 1;

            var employeeDeduction = (MONTLY_BASE_BENEFIT_COST * 12 / NUMBER_OF_PAYCHECKS).Round();
            var dependentsDeduction = GetEmployeeDeductions(dependentsUnder50, dependentsOver50);
            var salary = (yearlySalary / NUMBER_OF_PAYCHECKS).Round();
            var netSalary = salary - (employeeDeduction + dependentsDeduction);

            var employee = GetEmployeeWithDependents(employeeId, yearlySalary);
            _repository.Get(employeeId).Returns(employee);

            var plugins = new List<IDeductionPlugin> { new EmployeeDeductionPlugin(), new DependentsDeductionPlugin() };
            _pluginsProvider.GetPlugins().Returns(plugins);

            var service = GetService();

            // Act
            var result = await service.Calculate(employeeId, DateTime.UtcNow);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(netSalary, result!.NetAmmount);
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

        private PaycheckCalculationService GetService()
            => new(_repository, _pluginsProvider);
    }
}
