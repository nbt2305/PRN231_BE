using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Shared.DTO;
using Shared.DTO.Request;

namespace PT1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }
        [HttpPost("Create")]
        public IActionResult CreateNew(UserDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.CreateNew(token, request));
        }

        [HttpPost("Update")]
        public IActionResult Update(int id, UserDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Update(token, id, request));
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginRequest request)
        {
            return Ok(_service.Login(request));
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpPost("Search")]
        public IActionResult Search(UserDTO request)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.Search(token, request));
        }
        [HttpGet("{Id}")]
        public IActionResult GetByID(int Id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetById(token, Id));
        }

        [HttpGet("{Id}/Projects")]
        public IActionResult GetProejectsOfUser(int Id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetProjectOfUser(token, Id));
        }

        [HttpGet()]
        public IActionResult GetList(int Id)
        {
            var token = HttpContext.Items["Token"].ToString();
            return Ok(_service.GetList(token));
        }

    }
}
