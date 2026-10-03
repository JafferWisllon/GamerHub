using StackExchange.Redis;

namespace GamerHub.API.Services.Interfaces;

public interface ICacheService
{
    Task SetHash(string key, HashEntry[] values);
    Task<T?> GetString<T>(string key);
    Task<double> AddSortedSet(string key, string member, double score);
    Task SetString(string key, object value);
}