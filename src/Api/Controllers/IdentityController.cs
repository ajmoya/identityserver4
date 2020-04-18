using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [Authorize("esFull")]
        public IActionResult Get()
        {
            var toni = from c in User.Claims select new {c.Type, c.Value};
            var toni2 = User.Claims.Select(x => new {x.Type, x.Value});

            return new JsonResult(toni);
        }
    }
}