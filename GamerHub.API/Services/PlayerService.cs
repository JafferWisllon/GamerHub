using GamerHub.API.Models.Dtos;
using GamerHub.API.Exceptions;
using GamerHub.API.Services.Interfaces;
using GamerHub.API.Validators;
using StackExchange.Redis;

namespace GamerHub.API.Services;

public class PlayerService : IPlayerService
{
    private readonly ICacheService _cacheService;
    private const string PROFILE_REDIS_KEY = "player:profile:";
    public PlayerService(ICacheService cacheService) 
        => _cacheService = cacheService;

    public async Task<(bool isSuccess, ValidationRequestException? exceptionViewModel)> PostPlayer(PostPlayer player)
    {
        var validator = new PostPlayerValidator();
        var result = await validator.ValidateAsync(player);

        if (result.IsValid is false && result.Errors.Any())
            return (false, ValidationRequestException.CreateViewModelErrors(result.Errors));
            
        var hashKey = $"{PROFILE_REDIS_KEY}{player.Id}";
        var hash = new HashEntry[]
        {
            new HashEntry("name", player.Name),
            new HashEntry("country", player.Country),
            new HashEntry("avatar", player.AvatarUrl),
        };
            
        await _cacheService.SetHash(hashKey, hash);
        return (true, null);
    }
}