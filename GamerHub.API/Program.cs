using GamerHub.API.Endpoints;
using GamerHub.API.Services;
using GamerHub.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddScoped<ICacheService, CacheSerice>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

var app = builder.Build();
if (app.Environment.IsDevelopment()) 
    app.MapOpenApi();

app.UseHttpsRedirection();
app.MapPlayersEndpoints();
app.Run();