using GameStore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GameStore.Data {

    // Фабрика для создания DbContext во время разработки (Design-Time)
    public class GameStoreContextFactory : IDesignTimeDbContextFactory<GameStoreContext> {
        // Метод, который вызывается инструментами EF Core для создания контекста
        public GameStoreContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<GameStoreContext>();
            // Указываем EF Core использовать SQLite и прямой путь к базе данных
            optionsBuilder.UseSqlServer("Data Source=GameStore.db");

            // После настройки переходим к созданию экземпляра GameStoreContext с указанными опциями
            return new GameStoreContext(optionsBuilder.Options);
        }
    }
    

// Основной контекст БД, через который работаем с таблицами
public class GameStoreContext : DbContext {
        // Создаём конструктор, который принимате в себя опции конфигурации (подключения, провайдеры и т.д)
        public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options) { }

        public DbSet<Game> Games => Set<Game>();

        public DbSet<Genre> Genres => Set<Genre>();

        // Метод для доп. настройки конфигурации модели ( отношений, огрнаичней, конфигураций)
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Genre>().HasData(
                new { Id = 1, Name = "Fighting"},
                new { Id = 2, Name = "RP"},
                new { Id = 3, Name = "Sports"},
                new { Id = 4, Name = "Racing"},
                new { Id = 5, Name = "Kids and Family"}
                );

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
                .Property(g => g.Price)
                .HasColumnType("decimal(18,4)");

        }

    }

}
