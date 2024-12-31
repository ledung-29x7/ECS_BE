using ECS.Areas.Admin.Models;
using ECS.Areas.Authen.Models;
using ECS.Areas.Units.Models;
using ECS.Areas.Client.Models;
using ECS.Areas.EmployeeService.Models;
using Microsoft.EntityFrameworkCore;
using ECS.Dtos;

namespace ECS
{
    public class ECSDbContext : DbContext 
    {
        public ECSDbContext(DbContextOptions<ECSDbContext>options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductCategory>().HasNoKey();
            modelBuilder.Entity<ProductWithImagesDTO>().HasNoKey();
            modelBuilder.Entity<EmployeeWithImagesDTO>().HasNoKey();
            modelBuilder.Entity<EmployeeWorkListDto>().HasNoKey();
            modelBuilder.Entity<EmployeeAvailable>().HasNoKey();
            modelBuilder.Entity<ClientDto>().HasNoKey();
            modelBuilder.Entity<EmployeeDto>().HasNoKey();
            modelBuilder.Entity<RawEmployeeResult>().HasNoKey();
            modelBuilder.Entity<RawProductResult>().HasNoKey();
            modelBuilder.Entity<GetOrderDetalByOrderId>().HasNoKey();
            modelBuilder.Entity<RawProductResultByClientIdDto>().HasNoKey();

        }

        public DbSet<Role> roles { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Departments>  departments { get; set; }
        public DbSet<ImageTable> imageTables { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<ProductService> productServices { get; set; }
        public DbSet <ProductCategory> ProductCategory { get; set; }
        public DbSet<CallHistory> callHistory { get; set; }
        public DbSet<Order> order { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<ProductWithImagesDTO> productWithImagesDTOs { get; set; }
        public DbSet<EmployeeService> employeeService { get; set; }
        public DbSet<EmployeeWorkListDto> employeeWorkListDTOs { get; set;}
        public DbSet<EmployeeProductCategory> employeeProductCategories { get; set; }
        public DbSet<CallStatus> callStatuses { get; set; }
        public DbSet<EmployeeAvailable> employeeAvailables { get; set; }
        public DbSet<ProductStatus> productStatuses { get; set; }
        public DbSet<ClientDto> clientDtos { get; set; }
        public DbSet<EmployeeDto> employeeDtos { get; set; }
        public DbSet<Contact> contacts { get; set; }
        public DbSet<GetOrderDetalByOrderId> getOrderDetalByOrderIds { get; set; }
        public DbSet<RawProductResultByClientIdDto> rawProductResultByClientIdDtos { get; set; }
    }
}
