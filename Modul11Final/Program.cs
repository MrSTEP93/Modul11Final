using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modul11Final.Controllers;
using Modul11Final.Services;
using Telegram.Bot;

namespace Modul11Final
{
    internal class Program
    {
        private static string token;

        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            ReadConfig();

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Запуск сервиса...");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        private static void ReadConfig()
        {
            
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
 
            IConfiguration configuration = builder.Build();
            token = configuration["BotConfig:token"];
            //6970731421:AAGJ3U6SaaDH_2EA_kfxMfQ9D9UYHRH28cc
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Ошибка чтения конфигурации");
            } else
            {
                Console.WriteLine("Прочитана конфигурация. " + token);
            }
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddTransient<ButtonController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<DefaultMessageController>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(token));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();

        }
    }
}
