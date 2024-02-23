using Microsoft.Extensions.Logging;
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
using Telegram.Bot.Types.ReplyMarkups;

namespace Modul11Final.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");

            switch (message.Text)
            {
                case "/start":
                    // Объект, представляющий кноки
                    Buttons.InitButtons(out List<InlineKeyboardButton> buttons, ref Buttons.mainButtons);

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b> Наш бот умеет:</b>{Environment.NewLine}" +
                        $"* Считать количество символов в полученном сообщении; {Environment.NewLine}" +
                        $"* Вычислить сумму ЦЕЛЫХ чисел, которые вы пришлете.{Environment.NewLine}" +
                        $"{Environment.NewLine}<b>Выберите желаемое действие:</b>",
                        cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                default:
                    string command = _memoryStorage.GetSession(message.From.Id).Command;
                    switch (command)
                    {
                        case "SummNumbers":
                            Console.WriteLine($"Приступаем к суммированию чисел: {message.Text}");
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, Summator(message.Text), cancellationToken: ct);
                            break;
                        case "CountMessageLength":
                            Console.WriteLine($"Считаем количество символов в строке: {message.Text}");
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"В вашем сообщении {message.Text.Length} символов", cancellationToken: ct);
                            break;
                        default:
                            Console.WriteLine("Получена нераспознанная команда: " + message.Text);
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Похоже, команда не распознана", cancellationToken: ct);
                            break;
                    }
                    break;
            }
        }

        private static string Summator(string inputString)
        {
            string result = string.Empty;
            int summ = 0;
            string[] splittedStrings = inputString.Split(" ");
            List<int> numbers = new();
            Console.Write("Получены числа: ");
            foreach (string s in splittedStrings)
            {
                if (int.TryParse(s, out int num))
                {
                    numbers.Add(num);
                    Console.Write(num + " ");
                }
                else
                    Console.Write("Parsing error ");
            }
            Console.WriteLine();

            if (numbers.Count > 0)
            {
                foreach (int num in numbers)
                    summ += num;
                result = "Сумма полученных чисел: " + summ.ToString();
            }
            else
                result = "В введенной строке не было целых чисел!";
            return result;
        }
    }
}
