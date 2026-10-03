using GamerHub.API.Exceptions;
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

        _ = app.MapPost("/api/players/{id}/scores", async (
            int id, 
            IPlayerService playerService, 
            PostScore score) =>
        {
            try
            {
                var result = await playerService.PostScore(id, score);
                return !result.isSuccess ? Results.BadRequest(result.exceptionViewModel) : Results.Ok();
            }
            catch (TooManyRequestException ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status429TooManyRequests);
            }
            catch (Exception e)
            {
                return Results.InternalServerError(e.Message);
            }
        });
    }
}