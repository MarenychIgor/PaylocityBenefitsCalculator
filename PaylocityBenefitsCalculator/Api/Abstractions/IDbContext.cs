using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Abstractions
{
    public interface IDbContext
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<Dependent> Dependents { get; set; }
    }
}
