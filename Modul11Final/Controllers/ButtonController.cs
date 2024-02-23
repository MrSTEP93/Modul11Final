using Modul11Final.Models;
using Modul11Final.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Modul11Final.Controllers
{
    public class ButtonController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public ButtonController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку {callbackQuery.Data}");
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).Command = callbackQuery.Data;

            // Генерим информационное сообщение
            string commandText = Buttons.GetTextByCommand(callbackQuery.Data, ref Buttons.mainButtons, out string commandHint);

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"Выбрана команда <b>\"{commandText}\"</b>." +
                $"{Environment.NewLine}Вы можете изменить её в главном меню." +
                $"{Environment.NewLine}<b>Подсказка:</b> {commandHint}",
                cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
