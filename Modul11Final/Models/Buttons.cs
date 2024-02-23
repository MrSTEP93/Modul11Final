using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Modul11Final.Models
{
    public class Button
    {
        public string Command { get; }
        public string Text { get; }
        public string Hint { get;  }

        public Button(string command, string text, string hint)
        {
            Command = command;
            Text = text;
            Hint = hint;
        }
    }

    public static class Buttons
    {
        public static List<Button> mainButtons = new() {
            new ("CountMessageLength", "Считаем символы", "Просто пришлите мне любой текст"),
            new ("SummNumbers", "Складываем числа", "Для суммирования чисел пришлите их ОДНИМ сообщением, разделив ПРОБЕЛОМ")
        };

        public static string GetTextByCommand(string callbackQueryData, ref List<Button> buttonList, out string hint)
        {
            string result = String.Empty;
            hint = String.Empty;
            foreach (Button button in buttonList)
            {
                if (callbackQueryData == button.Command)
                {
                    result = button.Text;
                    hint = button.Hint;
                }
            }
            return result;
        }

        public static void InitButtons(out List<InlineKeyboardButton> result, ref List<Button> buttonList)
        {
            result = new List<InlineKeyboardButton>();
            foreach (var button in buttonList)
            {
                result.Add(InlineKeyboardButton.WithCallbackData(button.Text, button.Command));
            }
        }
    }


}
