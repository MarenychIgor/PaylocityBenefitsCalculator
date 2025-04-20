using System.Collections.Generic;
using System;
using Xunit;
using Api.Models;
using Api.DeductionPlugins;

namespace ApiTests
{
    public class DependentsDeductionPluginTests : DependentsTestBase
    {
        [Theory]
        [MemberData(nameof(PaycheckAndDependentDobData))]
        public void DependentDoBOnOrBeforePaycheckDate_DeductAdjustedBenefir(DateTime paycheckDate, DateTime dependentDoB)
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

            var expectedDependentsDeduction = GetEmployeeDeductions(dependentsUnder50: 0, dependentsOver50: 1);
            Assert.Equal(expectedDependentsDeduction, result);
        }

        public static IEnumerable<object[]> PaycheckAndDependentDobData =>
            new List<object[]>
            {
                new object[] { new DateTime(2025, 04, 19), new DateTime(1975, 04, 19) },
                new object[] { new DateTime(2025, 05, 19), new DateTime(1975, 04, 19) }
            };
    }
}
