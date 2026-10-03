using System.Text.Json;
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

    public async Task<T?> GetString<T>(string key)
    {
        RedisValue data = await db.StringGetAsync(key);
        if (!data.HasValue)
            return default;

        if (typeof(T) == typeof(string))
            return (T)(object)data.ToString();

        return JsonSerializer.Deserialize<T>(data.ToString());
    }

    public async Task<double> AddSortedSet(string key, string member, double score) 
        => await db.SortedSetIncrementAsync(key, member, score);

    public async Task SetString(string key, object value) 
        => await db.StringSetAsync(key, JsonSerializer.Serialize(value), TimeSpan.FromSeconds(30));
}