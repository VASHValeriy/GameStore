using GameStore.Data;
using GameStore.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.Controllers {

    [ApiController]
    [Route("genres")]
    public class GenresController : ControllerBase {
        private readonly GameStoreContext _context;

        public GenresController(GameStoreContext context) {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Genre> GetGenres() {
            return _context.Genres.ToList();
        }
    }
}