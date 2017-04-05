using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Azure;
using Game.Generators;
using Game.Utils;

namespace PotatoApi.Controllers
{
    public class PlayersController : ApiController
    {
        public HttpResponseMessage Get()
        {            
            var players = TableManager.Get<PlayerEntity>(TableData.PlayersTable).ToList();
            var captains =  TableManager.Get<CaptainEntity>(TableData.CaptainsTable).ToList();

            return this.Response(() => PlayerGenerator.Generate(Seeder.Random(), players, captains), null);
        }

        [HttpPost]
        [Route("players")]
        public HttpResponseMessage InsertPlayers()
        {
            //note(htoma): API players from FootballData
            //var players =
            //    FootballData.getPlayers().Select(x => new PlayerEntity(x.Item1, string.Empty, x.Item2)).ToList();

            var players =
                CsvPlayers.getPlayers()
                    .Select(x => new PlayerEntity(BlobManager.GetNextId(), x.Item1, x.Item2, x.Item3))
                    .ToList();
            return this.Response(() => TableManager.Insert(TableData.PlayersTable, players));
        }

        [Route("players")]
        [HttpDelete]
        public HttpResponseMessage DeletePlayers()
        {
            return this.Response(() => TableManager.DeleteTable(TableData.PlayersTable));
        }        
    }
}
