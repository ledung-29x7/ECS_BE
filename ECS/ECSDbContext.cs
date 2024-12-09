using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;
using ECS.Areas.Units.Models;
using ECS.Areas.Client.Models;
using Microsoft.EntityFrameworkCore;
using ECS.Areas.EmployeeService.Models;
using ECS.Areas.EmployeeService.Controllers;

namespace ECS
{
    public class ECSDbContext : DbContext 
    {
        public ECSDbContext(DbContextOptions<ECSDbContext>options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Role> roles { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Departments>  departments { get; set; }
        public DbSet<ImageTable> imageTables { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<CallHistory> callHistories {get; set;} 
    }
}
