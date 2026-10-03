using GamerHub.API.Models.Dtos;
using GamerHub.API.Exceptions;
using GamerHub.API.Services.Interfaces;
using GamerHub.API.Validators;
using StackExchange.Redis;

namespace GamerHub.API.Services;

public class PlayerService : IPlayerService
{
    private readonly ICacheService _cacheService;
    private const string PROFILE_REDIS_KEY = "player:profile:{0}";
    private const string LOCKED_PLAYER_REDIS_KEY = "player:{0}";
    private const string LEADERBOARD_REDIS_KEY = "game:leaderboard:global";
    public PlayerService(ICacheService cacheService) 
        => _cacheService = cacheService;

    public async Task<(bool isSuccess, ValidationRequestException? exceptionViewModel)> PostPlayer(PostPlayer player)
    {
        var validator = new PostPlayerValidator();
        var result = await validator.ValidateAsync(player);

        if (result.IsValid is false && result.Errors.Any())
            return (false, ValidationRequestException.CreateViewModelErrors(result.Errors));
        
        var hash = new HashEntry[]
        {
            new HashEntry("name", player.Name),
            new HashEntry("country", player.Country),
            new HashEntry("avatar", player.AvatarUrl),
        };
            
        await _cacheService.SetHash(string.Format(PROFILE_REDIS_KEY, player.Id), hash);
        return (true, null);
    }

    public async Task<(bool isSuccess, ValidationRequestException? exceptionViewModel)> PostScore(int id, PostScore request)
    {
        var validator = new PostScoreValidator();
        var result = await validator.ValidateAsync(request);
        
        if (result.IsValid is false && result.Errors.Any())
            return (false, ValidationRequestException.CreateViewModelErrors(result.Errors));

        var locked = await _cacheService.GetString<bool>(string.Format(LOCKED_PLAYER_REDIS_KEY, id));
        if (locked)
            throw new TooManyRequestException("Wait 30 seconds");
        
        await _cacheService.AddSortedSet(LEADERBOARD_REDIS_KEY, string.Format(PROFILE_REDIS_KEY, id), request.Score);
        
        await _cacheService.SetString(string.Format(LOCKED_PLAYER_REDIS_KEY, id), true);
        return (true, null);
    }
}