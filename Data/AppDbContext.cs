using Microsoft.EntityFrameworkCore;
using HrLeaveManagement.Models;

namespace HrLeaveManagement.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
}
