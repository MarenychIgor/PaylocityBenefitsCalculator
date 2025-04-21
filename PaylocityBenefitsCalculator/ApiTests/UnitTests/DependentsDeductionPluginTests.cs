using System.Collections.Generic;
using System;
using Xunit;
using Api.Models;
using Api.DeductionPlugins;
using ApiTests.Helpers;

namespace ApiTests.UnitTests
{
    public class DependentsDeductionPluginTests : DependentsBenefitCalculationHelper
    {
        [Theory]
        [MemberData(nameof(DependentBirthdayBeforePaycheckData))]
        [MemberData(nameof(DependentBirthdayAfterPaycheckData))]
        public void DependentBirthdayAndPaycheckDate_DeductCorrectAmmount(DateTime dependentDoB, DateTime paycheckDate, int dependentsUnder50, int dependentsOver50)
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

            var plugin = new DependentsDeductionPlugin();

            // Act
            var result = plugin.CalculateDeduction(employee, paycheckDate);

            // Assert

            var expectedDependentsDeduction = GetEmployeeDeductions(dependentsUnder50, dependentsOver50);
            Assert.Equal(expectedDependentsDeduction, result);
        }

        public static IEnumerable<object[]> DependentBirthdayBeforePaycheckData =>
            new List<object[]>
            {
                new object[] { new DateTime(1975, 04, 19), new DateTime(2025, 04, 19), 0, 1 },
                new object[] { new DateTime(1975, 04, 19), new DateTime(2025, 05, 19), 0, 1 }
            };

        public static IEnumerable<object[]> DependentBirthdayAfterPaycheckData =>
            new List<object[]>
            {
                new object[] { new DateTime(1975, 05, 19), new DateTime(2025, 04, 19), 1, 0 },
                new object[] { new DateTime(1975, 04, 20), new DateTime(2025, 04, 19), 1, 0 }
            };
    }
}
