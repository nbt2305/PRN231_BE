using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Shared.DTO;
using Shared.DTO.Request;

namespace PT1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPositionController : ControllerBase
    {
        private IJobPositionService _service;

        public JobPositionController(IJobPositionService service)
        {
            _service = service;
        }
        [HttpPost("Create")]
        public IActionResult CreateNew(JobPositionDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.CreateNew(token, request));
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, JobPositionDTO request)
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
        public IActionResult Search(JobPositionDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Search(token, request));
        }

    }
}
