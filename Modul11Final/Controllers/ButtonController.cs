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
            string commandText = String.Empty;
            foreach (var button in Buttons.mainButtons)
            {
                if (callbackQuery.Data == button.Command)
                {
                    commandText = button.Text;
                }
            }

            //string commandText = callbackQuery.Data switch
            //{
            //    "CountMessageLength" => "Считаем символы",
            //    "SummNumbers" => "Складываем числа",
            //    _ => String.Empty
            //};

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"Выбрана команда <b>\"{commandText}\"</b>." +
                $"{Environment.NewLine}Вы можете изменить её в главном меню.", 
                cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
