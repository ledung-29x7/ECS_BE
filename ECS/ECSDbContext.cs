using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;
using ECS.Areas.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace ECS
{
    public class ECSDbContext : DbContext 
    {
        public ECSDbContext(DbContextOptions<ECSDbContext>options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductCategory>()
        .HasNoKey();
        }

        public DbSet<Role> roles { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Departments>  departments { get; set; }
        public DbSet<Client> client {  get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet <ProductCategory> ProductCategory { get; set; }
    }
}
