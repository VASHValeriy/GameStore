namespace GameStore.Dtos {
    public record class GameSummaryDto(
        int ID,
        string Name,
        int GenreId,
        string Genre,
        decimal Price
        );
}
