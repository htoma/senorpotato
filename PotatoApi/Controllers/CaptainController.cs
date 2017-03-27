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

            return captains.Select(x => new Captain
            {
                Affected = (Captain.EAffected)Enum.Parse(typeof(Captain.EAffected), x.Affected),
                Skill = (ESkill)Enum.Parse(typeof(ESkill), x.Skill),
                Value = x.Value
            });
        }

        [HttpPut]
        [Route("captains")]
        public bool InsertCaptains()
        {
            try
            {
                var captains =
                    CsvCaptains.getCaptains().Select(x => new CaptainEntity(x.Item1, x.Item2, x.Item3)).ToList();
                TableManager.Insert(CaptainsTable, captains);
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
