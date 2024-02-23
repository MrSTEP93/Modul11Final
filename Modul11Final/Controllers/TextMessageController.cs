using Microsoft.Extensions.Logging;
using Modul11Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Modul11Final.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;

        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            //await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено текстовое сообщение", cancellationToken: ct);

            switch (message.Text)
            {
                case "/start":
                    // Объект, представляющий кноки
                    Buttons.InitButtons(out List<InlineKeyboardButton> buttons, ref Buttons.mainButtons);
                    
                    //var buttons = new List<InlineKeyboardButton>();
                    //foreach (var button in Buttons.mainButtons)
                    //{
                    //    buttons.Add(InlineKeyboardButton.WithCallbackData(button.Text, button.Command));
                    //}

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b> Наш бот умеет:</b>{Environment.NewLine}" +
                        $"* Считать количество символов в полученном сообщении; {Environment.NewLine}" +
                        $"* Вычислить сумму чисел, которые вы пришлете.{Environment.NewLine}" +
                        $"{Environment.NewLine}<b>Выберите желаемое действие:</b>",
                        cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                case "CountMessageLength":
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Считалка", cancellationToken: ct);
                    break;
                default:
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Кажется, здесь надо анализировать команду пользователя", cancellationToken: ct);
                    break;
            }
        }
    }
}
