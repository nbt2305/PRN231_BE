using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Shared.DTO;
using Shared.DTO.Request;

namespace PT1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }
        [HttpPost("Create")]
        public IActionResult CreateNew(DepartmentDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.CreateNew(token, request));
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, DepartmentDTO request)
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

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Delete(token, id));
        }

        [HttpPost("Search")]
        public IActionResult Search(DepartmentDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Search(token, request));
        }
        [HttpGet("GetDetail/{Id}")]
        public IActionResult GetDetail(int Id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetPositionsByDepartment(token, Id));
        }
    }
}
