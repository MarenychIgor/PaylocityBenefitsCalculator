using System;
using Api.DeductionPlugins;
using Api.Extensions;
using Api.Models;
using Xunit;

namespace ApiTests.UnitTests
{
    public class EmployeeDeductionPluginTests
    {
        private const int NUMBER_OF_PAYCHECKS = 26;
        private const decimal MONTLY_BASE_BENEFIT_COST = 1000;
        private const decimal SALARY_ABOVE_TRESHOLD_COEFFICIENT = 0.02M;

        [Fact]
        public void SalaryBelowTreshold_DeductBaseBenefit()
        {
            // Arrange
            var employee = new Employee { Id = 1, Salary = 70000M };
            var deduction = (MONTLY_BASE_BENEFIT_COST * 12 / NUMBER_OF_PAYCHECKS).Round();

            var plugin = new EmployeeDeductionPlugin();

            // Act
            var result = plugin.CalculateDeduction(employee, DateTime.UtcNow);

            // Assert
            Assert.Equal(deduction, result);
        }

        [Fact]
        public void SalaryAboveTreshold_DeductAdjustedBaseBenefit()
        {
            // Arrange
            var yearlySalary = 90000M;
            var yearlyDeduction = MONTLY_BASE_BENEFIT_COST * 12 + yearlySalary * SALARY_ABOVE_TRESHOLD_COEFFICIENT;
            var deduction = (yearlyDeduction / NUMBER_OF_PAYCHECKS).Round();
            var employee = new Employee { Id = 1, Salary = yearlySalary };

            var plugin = new EmployeeDeductionPlugin(); ;

            // Act
            var result = plugin.CalculateDeduction(employee, DateTime.UtcNow);

            // Assert
            Assert.Equal(deduction, result);
        }
    }
}
