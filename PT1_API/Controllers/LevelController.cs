using Microsoft.AspNetCore.Mvc;
using Service;

namespace PT1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private ICommonCodeService _service;
        public LevelController(ICommonCodeService service)
        {
            _service = service;
        }
        [HttpGet()]
        public IActionResult GetList(int id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetList(token));
        }
    }
}
