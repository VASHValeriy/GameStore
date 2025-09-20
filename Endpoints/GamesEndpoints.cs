using System.Net.Mail;

using GameStore.Entities;
using GameStore.Dtos;
using GameStore.Data;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;
public static class GamesEndpoints {

    const string GetGameEdnpointName = "GetGame";
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app) {

        var group = app.MapGroup("games")
                       .WithParameterValidation();
        // .WithParameterValidation() автоматически проверяет параметры, которые приходят в endpoint (например, FromBody, FromQuery, FromRoute), на соответствие условиям валидации, заданным с помощью атрибутов из System.ComponentModel.DataAnnotations, таких как:
        //  [Required] — обязательное поле
        //  [Range(1, 100)] — число в диапазоне
        //  [StringLength(50)] — длина строки
        //  [EmailAddress] — валидный email;

        // app.MapGet(...) - Регистрирует маршрут(endpoint) для HTTP-запроса GET.
        // То есть сервер теперь будет отвечать на запрос по адресу:
        // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) => 
             await dbContext.Games
                      .Include(game => game.Genre)
                      .Select(game => game.ToGameSummaryDto())
                      .AsNoTracking()
                      .ToListAsync() // .ToListAsync() - делает процесс асинхронным
                      );
        // "games" — это маршрут (path), строка, по которой будет доступен endpoint.
        // () => games — это обработчик запроса (что делать, когда на этот маршрут пришёл запрос).


        // GET/games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => {

            Game? game = await dbContext.Games.FindAsync(id); // GameDto? - вопрос. знак позволяет принимать как валидное значение так и null

            return game is null ?
            Results.NotFound() :
            Results.Ok(game);

        })
            .WithName(GetGameEdnpointName);
        // Post
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => {

            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();


            // Results.CreatedAtRoute(...) - это готовый хелпер-метод в ASP.NET Core для возврата HTTP-ответа 201 Created.
            // Он говорит клиенту: "ресурс создан, и вот где его можно найти".
            return Results.CreatedAtRoute(
                GetGameEdnpointName,
                new { id = game.Id },
                game.ToGameDetailsDto()
                );
        });

        // PUT / GAMES
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => {

            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null) {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                     .CurrentValues
                     .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) => {

            int deleted = await dbContext.Games
                                   .Where(game => game.Id == id)
                                   .ExecuteDeleteAsync();

            if (deleted == 0) return Results.NotFound();

            return Results.NoContent();

        });

        return group;
    }

}