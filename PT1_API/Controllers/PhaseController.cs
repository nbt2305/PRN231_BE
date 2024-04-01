using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Phases;
using Shared.DTO;

namespace PT1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhaseController : ControllerBase
    {
        private IPhaseService _service;

        public PhaseController(IPhaseService service)
        {
            _service = service;
        }
        [HttpPost("Create")]
        public IActionResult CreateNew(PhaseDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.CreateNew(token, request));
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, PhaseDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Update(token, id, request));
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetList(token));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetById(token, id));
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Delete(token, id));
        }

        [HttpPost("Search")]
        public IActionResult Search(PhaseDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Search(token, request));
        }
    }
}
