using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Azure;
using Game;
using Game.Generators;

namespace PotatoApi.Controllers
{
    public class CaptainController : ApiController
    {        
        public IEnumerable<Captain> Get()
        {
            return GetCaptains();
        }

        [HttpGet]
        [Route("captains")]
        public IEnumerable<Captain> GetCaptains()
        {            
            var captains = TableManager.Get<CaptainEntity>(TableData.CaptainsTable).ToList();

            return captains.Select(PlayerGenerator.FromEntity);
        }

        [HttpPost]
        [Route("captains")]
        public bool InsertCaptains()
        {
            try
            {
                var captains =
                    CsvCaptains.getCaptains()
                        .Select(x => new CaptainEntity(BlobManager.GetNextId(), x.Item1, x.Item2, x.Item3))
                        .ToList();
                TableManager.Insert(TableData.CaptainsTable, captains);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Route("captains")]
        [HttpDelete]
        public bool DeleteCaptains()
        {
            try
            {
                TableManager.DeleteTable(TableData.CaptainsTable);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }        
    }
}
