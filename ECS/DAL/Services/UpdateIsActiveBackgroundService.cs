
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Services
{
    public class UpdateIsActiveBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UpdateIsActiveBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ECSDbContext>();

                    await dbContext.Database.ExecuteSqlRawAsync("EXEC UpdateExpiredProductServices");

                }

                // Chạy mỗi ngày
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
