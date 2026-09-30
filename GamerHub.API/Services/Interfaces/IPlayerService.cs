using GamerHub.API.Models.Dtos;
using GamerHub.API.Exceptions;

namespace GamerHub.API.Services.Interfaces;

public interface IPlayerService
{
    Task<(bool isSuccess, ValidationRequestException? exceptionViewModel)> PostPlayer(PostPlayer player);
}