using Microsoft.AspNetCore.Mvc;
using Service;
using Service.LogWork;
using Shared.DTO;

namespace PT1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogWorkController : ControllerBase
    {
        private ILogWorkService _service;

        public LogWorkController(ILogWorkService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        public IActionResult CreateNew(LogWorkDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.CreateNew(token, request));
        }

        [HttpPost("Search")]
        public IActionResult Search(LogWorkDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Search(token, request));
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Delete(token, id));
        }
    }
}
