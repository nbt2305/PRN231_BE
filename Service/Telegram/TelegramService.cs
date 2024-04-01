using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Service.Telegram
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient _botClient;

        public TelegramService(string botToken)
        {
            _botClient = new TelegramBotClient(botToken);
        }
        public async Task SendMessageAsync(string chatId, string message)
        {
            await _botClient.SendTextMessageAsync(chatId, message);
        }
    }
}
