using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace GameStore.Dtos {
    public record class GameDetailsDto(
        int ID,
        string Name,
        int GenreId,
        decimal Price
        );
}
