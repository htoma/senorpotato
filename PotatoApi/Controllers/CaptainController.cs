using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Azure;
using PotatoApi.Models;

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

            return captains.Select(Captain.FromEntity);
        }

        [HttpPut]
        [Route("captains")]
        public bool InsertCaptains()
        {
            try
            {
                var captains =
                    CsvCaptains.getCaptains().Select(x => new CaptainEntity(x.Item1, x.Item2, x.Item3)).ToList();
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
