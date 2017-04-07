using System.Net.Http;
using System.Web.Http;
using Game;

namespace PotatoApi.Controllers
{
    public class GameController : ApiController
    {
        [Route("games/{id}")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            return this.Response(() => GameManager.Load(id), null);
        }

        [Route("games")]
        [HttpPost]
        public HttpResponseMessage NewGame()
        {
            return this.Response(GameManager.CreateGame, null);
        }

        [Route("games/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteGame(int id)
        {
            return this.Response(() => GameManager.DeleteGame(id));
        }

        [Route("games")]
        [HttpDelete]
        public HttpResponseMessage DeleteGames()
        {
            return this.Response(() => GameManager.Delete());
        }

        [Route("games/{gameId}/captain/{turn}/{playerId}")]
        [HttpPut]
        public HttpResponseMessage SetCaptain(int gameId, ETurn turn, int playerId)
        {
            return this.Response(() => GameManager.SetCaptain(gameId, turn, playerId));
        }

        [Route("games/{gameId}/confirm/{turn}")]
        [HttpPut]
        public HttpResponseMessage Confirm(int gameId, ETurn turn)
        {
            return this.Response(() => GameManager.Confirm(gameId, turn));
        }

        [Route("games/{gameId}/actioncards/{turn}/{actionCardId}/{playerId}")]
        [HttpPost]
        public HttpResponseMessage AddPlayer(int gameId, ETurn turn, int actionCardId, int playerId)
        {
            return this.Response(() => GameManager.AddPlayer(gameId, turn, actionCardId, playerId));
        }
    }
}
