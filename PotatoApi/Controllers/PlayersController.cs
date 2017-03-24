using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Azure;
using PotatoApi.Generators;
using PotatoApi.Models;

namespace PotatoApi.Controllers
{
    public class PlayersController : ApiController
    {
        private const string PlayersTable = "players";

        public IEnumerable<Player> Get()
        {
            return GetPlayers();
        }

        [HttpGet]
        [Route("players")]
        public IEnumerable<Player> GetPlayers()
        {            
            var players = TableManager.Get<PlayerEntity>(PlayersTable).ToList();

            return PlayerGenerator.Generate(Seeder.Random(), players);
        }

        [HttpPut]
        [Route("players")]
        public bool InsertPlayers()
        {
            try
            {
                //note(htoma): API players from FootballData
                //var players =
                //    FootballData.getPlayers().Select(x => new PlayerEntity(x.Item1, string.Empty, x.Item2)).ToList();

                var players = CsvPlayers.getPlayers().Select(x => new PlayerEntity(x.Item1, x.Item2, x.Item3)).ToList();
                TableManager.Insert(PlayersTable, players);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Route("players")]
        [HttpDelete]
        public bool DeletePlayers()
        {
            try
            {
                TableManager.DeleteTable(PlayersTable);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }        
    }
}
