using System;
using System.Linq;
using System.Web.Http;
using Azure;

namespace PotatoApi.Controllers
{
    public class AzureController : ApiController
    {
        private const string PlayersTable = "players";

        [Route("azure/players")]
        [HttpPut]
        public bool InsertPlayers()
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

        [Route("azure/players")]
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
