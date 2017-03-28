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
        public IEnumerable<Player> Get()
        {
            return GetPlayers();
        }

        [HttpGet]
        [Route("players")]
        public IEnumerable<Player> GetPlayers()
        {            
            var players = TableManager.Get<PlayerEntity>(TableData.PlayersTable).ToList();
            var captains =  TableManager.Get<CaptainEntity>(TableData.CaptainsTable).ToList();

            return PlayerGenerator.Generate(Seeder.Random(), players, captains);
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
                TableManager.Insert(TableData.PlayersTable, players);
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
                TableManager.DeleteTable(TableData.PlayersTable);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }        
    }
}
