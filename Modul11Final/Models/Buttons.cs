using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul11Final.Models
{
    public class Button
    {
        public string Command { get; }
        public string Text { get; }

        public Button(string command, string text)
        {
            Command = command;
            Text = text;
        }
    }

    public static class Buttons
    {
        public readonly static List<Button> mainButtons = new() {
            new ("CountMessageLength", "Считаем символы"),
            new ("SummNumbers", "Складываем числа")
        };
    }

}
