using ECS;
using ECS.DAL.Interfaces;
using ECS.DAL.Repositorys;
using ECS.MappingProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
           .Build();
builder.Services.AddDbContext<ECSDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ECSConnection")));
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]));

builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ITokenBlacklistRepository,RedisTokenBlacklistRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IProductServiceRepository, ProductServiceRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();



builder.Services.AddScoped<IProductReponsitory , ProductReponsitory>();
builder.Services.AddScoped<IProductCategoryReponsitory, ProductCategoryReponsitory>();
builder.Services.AddScoped<ICallHistoryReponsitory , CallHistoryReponsitory>();
builder.Services.AddScoped<IOrderReponsitory, OrderReponsitory>();
builder.Services.AddScoped<IOrderDetailReponsitory , OrderDetailReponsitory>();


builder.Services.AddAutoMapper(typeof(AuthenticationProfile));
builder.Services.AddAutoMapper(typeof(DepartmentProfile));
builder.Services.AddAutoMapper(typeof(RoleProfile));
builder.Services.AddAutoMapper(typeof(ClientProfile));
builder.Services.AddAutoMapper(typeof(ProductProfile));

//Phan quyen
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager", "Admin"));
    options.AddPolicy("ServicePolicy", policy => policy.RequireRole("Service", "Manager", "Admin"));
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var tokenBlacklistRepository = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistRepository>();
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (await tokenBlacklistRepository.IsTokenBlacklistedAsync(token))
                {
                    context.Fail("This token has been blacklisted");
                }
            }
        };
    });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
