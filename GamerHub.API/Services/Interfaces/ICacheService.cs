using StackExchange.Redis;

namespace GamerHub.API.Services.Interfaces;

public interface ICacheService
{
    Task SetHash(string key, HashEntry[] values);
}