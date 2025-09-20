using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos {
    public record class CreateGameDto(
        [Required][StringLength(15)] string Name,
        int GenreId,
        [Range(0,10000)] decimal Price
        );
}
