using GamerHub.API.Services.Interfaces;
using StackExchange.Redis;

namespace GamerHub.API.Services;

public class CacheSerice : ICacheService
{
    private readonly IDatabase db;
    private readonly IConfiguration _configuration;

    public CacheSerice(IConfiguration configuration)
    {
        _configuration = configuration;
        var redis = ConnectionMultiplexer.Connect(_configuration.GetSection("Redis").GetSection("Connection").Value);
        db = redis.GetDatabase();
    }

    public async Task SetHash(string key, HashEntry[] value) 
        => await db.HashSetAsync(key, value);
}