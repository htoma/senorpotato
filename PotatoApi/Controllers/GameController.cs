﻿using System.Web.Http;
using Game;

namespace PotatoApi.Controllers
{
    public class GameController : ApiController
    {
        [Route("games/{id}")]
        [HttpGet]
        public Game.Game Get(int id)
        {
            var game = GameManager.Load(id);
            return game;
        }

        [Route("games")]
        [HttpPost]
        public Game.Game NewGame()
        {
            return GameManager.CreateGame();
        }

        [Route("games/{id}")]
        [HttpDelete]
        public string DeleteGame(int id)
        {
            GameManager.DeleteGame(id);

            //(note(htoma): return HTTP code
            return "OK";
        }

        [Route("games")]
        [HttpDelete]
        public string DeleteGames()
        {
            GameManager.Delete();

            //(note(htoma): return HTTP code
            return "OK";
        }

        [Route("games/{gameId}/{turn}/captain/{playerId}")]
        [HttpPut]
        public string SetCaptain(int gameId, ETurn turn, int playerId)
        {
            if (GameManager.SetCaptain(gameId, turn, playerId))
            {
                //(note(htoma): return HTTP code
                return "OK";
            }
            return "NOK";
        }
    }
}