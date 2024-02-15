using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modul11Final.Controllers;
using Modul11Final.Services;
using Telegram.Bot;

namespace Modul11Final
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddTransient<ButtonController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<DefaultMessageController>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("6970731421:AAGJ3U6SaaDH_2EA_kfxMfQ9D9UYHRH28cc"));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();

        }
    }
}
