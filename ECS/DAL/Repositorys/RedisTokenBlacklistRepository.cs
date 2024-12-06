using ECS.DAL.Interfaces;
using StackExchange.Redis;

namespace ECS.DAL.Repositorys
{
    public class RedisTokenBlacklistRepository : ITokenBlacklistRepository
    {
        private readonly IConnectionMultiplexer _redis;
        public RedisTokenBlacklistRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public async Task AddTokenToBlacklistAsync(string token, DateTime expiration)
        {
            var db = _redis.GetDatabase();
            var expiryTime = expiration - DateTime.UtcNow;

            if (expiryTime <= TimeSpan.Zero)
                expiryTime = TimeSpan.FromSeconds(1); // Đặt thời gian hết hạn tối thiểu

            await db.StringSetAsync(token, "blacklisted", expiryTime);
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            var db = _redis.GetDatabase();
            return await db.KeyExistsAsync(token);
        }
    }
}
