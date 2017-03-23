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

        [HttpGet]
        public IEnumerable<Player> Get(int count = 1)
        {
            var players = TableManager.Get<PlayerEntity>(PlayersTable).ToList();

            return PlayerGenerator.Generate(count, Seeder.Random(), players);
        }

        [HttpPut]
        public bool Insert()
        {
            try
            {
                var players =
                    FootballData.getPlayers().Select(x => new PlayerEntity(x.Item1, string.Empty, x.Item2)).ToList();
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
        public bool Delete()
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
