using Api.Abstractions;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class EmployeeRepository<TContext> : IEmployeeRepository
        where TContext : DbContext, IDbContext, new()
    {
        public async Task<Employee?> Get(int id)
        {
            using var context = new TContext();

            return await context.Employees.Include(x => x.Dependents)
                                          .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Employee>> GetAll()
        {
            using var context = new TContext();

            return await context.Employees.Include(x => x.Dependents)
                                          .ToListAsync();
        }
    }
}
