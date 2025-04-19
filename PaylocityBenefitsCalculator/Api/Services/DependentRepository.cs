using Api.Abstractions;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class DependentRepository : IDependentRepository
    {
        public async Task<Dependent?> Get(int id)
        {
            using var context = new InMemoryDbContext();

            return await context.Dependents.Include(x => x.Employee)
                                           .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Dependent>> GetAll()
        {
            using var context = new InMemoryDbContext();

            return await context.Dependents.Include(x => x.Employee)
                                           .ToListAsync();
        }
    }
}
