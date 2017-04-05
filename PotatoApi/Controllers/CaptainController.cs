using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Azure;
using Game.Generators;

namespace PotatoApi.Controllers
{
    public class CaptainController : ApiController
    {        
        [HttpGet]
        [Route("captains")]
        public HttpResponseMessage GetCaptains()
        {            
            var captains = TableManager.Get<CaptainEntity>(TableData.CaptainsTable).ToList();

            return this.Response(() => captains.Select(PlayerGenerator.FromEntity), null);
        }

        [HttpPost]
        [Route("captains")]
        public HttpResponseMessage InsertCaptains()
        {
            var captains =
                CsvCaptains.getCaptains()
                    .Select(x => new CaptainEntity(BlobManager.GetNextId(), x.Item1, x.Item2, x.Item3))
                    .ToList();
            return this.Response(() => TableManager.Insert(TableData.CaptainsTable, captains));
        }

        [Route("captains")]
        [HttpDelete]
        public HttpResponseMessage DeleteCaptains()
        {
            return this.Response(() => TableManager.DeleteTable(TableData.CaptainsTable));
        }        
    }
}
