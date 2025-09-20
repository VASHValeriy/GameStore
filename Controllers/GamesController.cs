using GameStore.Data;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.Controllers {

    [ApiController]
    [Route("games")]
    public class GamesController : ControllerBase {
        private readonly GameStoreContext _context;

        public GamesController(GameStoreContext context) {
            _context = context;
        }

        [HttpGet("filters")]
        public IActionResult GetGames([FromQuery] int? id, [FromQuery] string name, [FromQuery] int? genre) {
            try {
                var query = _context.Games
                    .Include(g => g.Genre)
                    .AsQueryable();

                if (id.HasValue)
                    query = query.Where(g => g.Id == id.Value);

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(g => g.Name.Contains(name));

                if (genre.HasValue)
                    query = query.Where(g => g.GenreId == genre.Value);

                var result = query
                    .ToList()
                    .Select(g => g.ToGameSummaryDto());
                return Ok(result);
            } catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }

        }

    }
}
