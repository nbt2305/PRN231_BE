using Microsoft.AspNetCore.Mvc;
using Service;
using Shared.DTO.Mail;

namespace PT1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        //injecting IMailService vào constructor
        public MailController(IMailService _MailService)
        {
            _mailService = _MailService;
        }
        [HttpPost]
        public IActionResult SendMail(MailData mailData)
        {
            _mailService.SendMail(mailData);
            return Ok();
        }
    }
}
