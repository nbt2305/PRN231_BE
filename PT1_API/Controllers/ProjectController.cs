using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Shared.DTO;
using Shared.DTO.Request;

namespace PT1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private IProjectService _service;

        public ProjectController(IProjectService service)
        {
            _service = service;
        }
        [HttpPost("Create")]
        public IActionResult CreateNew(ProjectDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.CreateNew(token, request));
        }

        [HttpPost("Search")]
        public IActionResult Search(ProjectSearch request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Search(token, request));
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, ProjectDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Update(token, id, request));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetById(token, id));
        }

        [HttpGet("Report")]
        public IActionResult Report()
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetReport(token));
        }

        [HttpGet("{id}/Phases")]
        public IActionResult GetPhasesByProjectId(int id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetPhasesInProject(token, id));
        }

        [HttpGet("")]
        public IActionResult GetAll(int id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetList(token));
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Delete(token, id));
        }

    }
}
