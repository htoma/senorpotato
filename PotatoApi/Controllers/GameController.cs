using System.Web.Http;
using Game;

namespace PotatoApi.Controllers
{
    public class GameController : ApiController
    {
        [Route("game/{id}")]
        [HttpGet]
        public Game.Game Get(int id)
        {
            var game = GameManager.Load(id);
            return game;
        }

        [Route("game")]
        [HttpPost]
        public Game.Game NewGame()
        {
            var manager = new GameManager();
            var game = manager.InitializeGame();
            return game;
        }

        [Route("game/{id}")]
        [HttpDelete]
        public string DeleteGame(int id)
        {
            GameManager.DeleteGame(id);
            return "OK";
        }

        [Route("game")]
        [HttpDelete]
        public string DeleteGames()
        {
            GameManager.Delete();
            return "OK";
        }
    }
}
