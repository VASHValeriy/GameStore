using GameStore.Data;
using GameStore.Dtos;
using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);

string allowReactApp = "AllowReactApp";

builder.Services.AddCors(options => {
    options.AddPolicy(allowReactApp,
        policy => {
            policy.WithOrigins("https://vashvaleriy.github.io") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        );
});

// Регистрация контроллеров
builder.Services.AddControllers();

// Это прямое подключенние к DB, что является плохой практикой
// В идеале нужно настраивать всё через JSON && IConfiguration interface
//var connectionString = "Data Source = GameStore.db"; 

// DB connect through JSON
var connectionString = builder.Configuration.GetConnectionString("GameStore"); 
builder.Services.AddSqlServer<GameStoreContext>(connectionString);

var app = builder.Build();

app.UseCors(allowReactApp);

app.MapGamesEndpoints();

app.MapControllers();

await app.MigrateDbAsync();

app.Run();
