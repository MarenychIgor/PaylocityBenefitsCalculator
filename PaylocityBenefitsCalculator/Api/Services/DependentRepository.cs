using Api.Abstractions;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class DependentRepository<TContext> : IDependentRepository
        where TContext : DbContext, IDbContext, new()
    {
        public async Task<Dependent?> Get(int id)
        {
            using var context = new TContext();

            return await context.Dependents.Include(x => x.Employee)
                                           .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Dependent>> GetAll()
        {
            using var context = new TContext();

            return await context.Dependents.Include(x => x.Employee)
                                           .ToListAsync();
        }
    }
}
