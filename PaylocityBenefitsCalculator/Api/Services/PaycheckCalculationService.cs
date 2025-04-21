using Api.Abstractions;
using Api.Dtos.Paycheck;
using Api.Extensions;
using Api.Models;

namespace Api.Services
{
    public class PaycheckCalculationService : IPaycheckCalculationService
    {
        // TODO: move to config
        private const int NUMBER_OF_PAYCHECKS = 26;

        private readonly IEmployeeRepository _repository;
        private readonly IDeductionPluginsProvider _pluginProvider;

        public PaycheckCalculationService(IEmployeeRepository repository, IDeductionPluginsProvider pluginsProvider)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _pluginProvider = pluginsProvider ?? throw new ArgumentNullException(nameof(pluginsProvider));
        }

        public async Task<GetPaycheckDto?> Calculate(int employeeId, DateTime paycheckDate)
        {           
            var employee = await _repository.Get(employeeId);
            if(employee == null)
            {
                return null; 
            }

            var deductions = GetDeductionTotal(employee, paycheckDate);
            var grossSalary = (employee.Salary / NUMBER_OF_PAYCHECKS).Round();
            var netSalary = grossSalary - deductions;

            return new GetPaycheckDto
            {
                Date = paycheckDate,
                EmployeeFirstName = employee.FirstName!,
                EmployeeLastName = employee.LastName!,
                GrossAmount = grossSalary,
                NetAmmount = netSalary,
                DeductedAmount = deductions
            };
        }

        private decimal GetDeductionTotal(Employee employee, DateTime paycheckDate)
        {
            var deductions = default(decimal);
            var plugins = _pluginProvider.GetPlugins();
            foreach (var plugin in plugins)
            {
                deductions += plugin.CalculateDeduction(employee, paycheckDate);
            }

            return deductions;
        }
    }
}
