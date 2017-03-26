using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Azure;
using PotatoApi.Generators;
using PotatoApi.Models;

namespace PotatoApi.Controllers
{
    public class CaptainController : ApiController
    {
        private const string CaptainsTable = "captains";

        public IEnumerable<Captain> Get()
        {
            return GetCaptains();
        }

        [HttpGet]
        [Route("captains")]
        public IEnumerable<Captain> GetCaptains()
        {            
            var captains = TableManager.Get<CaptainEntity>(CaptainsTable).ToList();

            return captains.Select(x => new Captain());
        }

        [HttpPut]
        [Route("players")]
        public bool InsertPlayers()
        {
            try
            {
                var players =
                    CsvCaptains.getCaptains().Select(x => new CaptainEntity(x.Item1, x.Item2, x.Item3)).ToList();
                TableManager.Insert(CaptainsTable, players);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Route("captains")]
        [HttpDelete]
        public bool DeletePlayers()
        {
            try
            {
                TableManager.DeleteTable(CaptainsTable);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }        
    }
}
