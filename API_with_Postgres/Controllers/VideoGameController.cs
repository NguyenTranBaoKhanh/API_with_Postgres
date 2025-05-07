using Microsoft.AspNetCore.Mvc;
using WebApplication2;

namespace API_with_Postgres.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        static private List<VideoGame> videoGames = new List<VideoGame>
        {
            new VideoGame
            {
                Id = 1,
                Title = "Spider-man 1",
                Platform = "PS5",
                Developer = "Insomniac Games",
                Publisher = "Sony Interractive Enteraiment"
            },
            new VideoGame
            {
                Id = 2,
                Title = "Spider-man 2",
                Platform = "PS5",
                Developer = "Insomniac Games",
                Publisher = "Sony Interractive Enteraiment"
            },
            new VideoGame
            {
                Id = 3,
                Title = "Spider-man 3",
                Platform = "PS5",
                Developer = "Insomniac Games",
                Publisher = "Sony Interractive Enteraiment"
            },
        };

        [HttpGet]
        public ActionResult<List<VideoGame>> GetVideoGames()
        {
            return Ok(videoGames);
        }

        [HttpGet("{id}")]
        public ActionResult<VideoGame> GetVideoGameById(int id)
        {
            var game = videoGames.FirstOrDefault(g => g.Id == id);
            if (game is null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpPost]
        public ActionResult<VideoGame> AddVideoGame(VideoGame newGame)
        {
            if (newGame is null)
            {
                return BadRequest();
            }
            newGame.Id = videoGames.Max(_ => _.Id) + 1;
            videoGames.Add(newGame);
            return CreatedAtAction(nameof(GetVideoGameById), new {id = newGame.Id}, newGame);
            //return Created(nameof(GetVideoGameById),newGame);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateVideoGame(int id, VideoGame updatedGame)
        {
            var game = videoGames.FirstOrDefault(g => g.Id == id);
            if (game is null)
            {
                return NotFound();
            }

            game.Title = updatedGame.Title;
            game.Developer = updatedGame.Developer;
            game.Platform = updatedGame.Platform;
            game.Publisher = updatedGame.Publisher;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteVideoGame(int id)
        {
            var game = videoGames.FirstOrDefault(g => g.Id == id);
            if (game is null)
            {
                return NotFound();
            }
            videoGames.Remove(game);
            return NoContent();
        }
    }
}
