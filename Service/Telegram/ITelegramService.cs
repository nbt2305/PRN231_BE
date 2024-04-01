﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Telegram
{
    public interface ITelegramService
    {
        Task SendMessageAsync(string chatId, string message);
    }
}
