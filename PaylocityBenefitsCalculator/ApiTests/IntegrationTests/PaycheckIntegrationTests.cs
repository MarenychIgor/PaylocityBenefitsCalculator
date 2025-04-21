using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Paycheck;
using Api.Models;
using ApiTests.Extenstions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ApiTests.IntegrationTests
{
    public class PaycheckIntegrationTests : IntegrationTestBase
    {
        public PaycheckIntegrationTests(WebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task WhenAskedForAPaycheck_ShouldReturnCorrectPaycheck()
        {
            var now = DateTime.UtcNow;
            var response = await HttpClient.GetAsync($"/api/v1/paycheck/1?paycheckDate={now.ToShortDateString()}");
            var paycheck = new GetPaycheckDto
            {
                Date = new DateTime(now.Year, now.Month, now.Day),
                EmployeeFirstName = "LeBron",
                EmployeeLastName = "James",
                GrossAmount = 2900.81M,
                NetAmmount = 2439.27M,
                DeductedAmount = 461.54M
            };

            await response.ShouldReturn(HttpStatusCode.OK, paycheck);
        }

        [Fact]
        public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
        {
            var response = await HttpClient.GetAsync($"/api/v1/paycheck/{int.MinValue}?paycheckDate={DateTime.UtcNow}");
            
            await response.ShouldReturn(HttpStatusCode.NotFound);
        }
    }
}
