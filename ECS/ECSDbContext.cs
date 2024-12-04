using Microsoft.EntityFrameworkCore;

namespace ECS
{
    public class ECSDbContext : DbContext 
    {
        public ECSDbContext(DbContextOptions<ECSDbContext>options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
