using System.Web.Http;
using Game;

namespace PotatoApi.Controllers
{
    public class GameController : ApiController
    {
        [Route("game")]
        [HttpPost]
        public Game.Game NewGame()
        {
            var manager = new GameManager();
            var game = manager.InitializeGame();
            return game;
        }
    }
}
