using Api.Abstractions;
using Api.Services;

namespace Api.Extensions
{
    public static class ServicesRegistrationExtensions
    {
        public static void RegisterScoped(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDependentRepository, DependentRepository>();
            services.AddScoped<IDependentService, DependentService>();
            services.AddScoped<IPaycheckCalculationService, PaycheckCalculationService>();
        }

        public static void RegisterSingleton(this IServiceCollection services)
            => services.AddSingleton<InMemoryDbContext>();
    }
}
