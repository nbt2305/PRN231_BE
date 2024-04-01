using Shared.DTO.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
