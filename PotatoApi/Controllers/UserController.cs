using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Game;

namespace PotatoApi.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            return await Reply(async () =>
            {
                var users = new[] {new User
                {
                    Id = 1,
                    Name = "Senor Potato"
                }};

                return Ok(users);
            });
        }
    }
}