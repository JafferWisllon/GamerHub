using GamerHub.API.Models.Dtos;
using GamerHub.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GamerHub.API.Endpoints;

public static class PlayerEndpoints
{
    public static void MapPlayersEndpoints(this WebApplication app)
    {
        _ = app.MapPost("/api/players", async (
            IPlayerService playerService,
            [FromBody] PostPlayer request) 
            =>
        {
            var result = await playerService.PostPlayer(request);
            return !result.isSuccess ? Results.BadRequest(result.exceptionViewModel) : Results.Ok();
        });
    }
}