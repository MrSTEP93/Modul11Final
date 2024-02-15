using Modul11Final.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul11Final.Services
{
    public class MemoryStorage : IStorage
    {
        /// <summary>
        /// Хранилище сессий
        /// </summary>
        private readonly ConcurrentDictionary<long, Session> allSessions;

        public MemoryStorage()
        {
            allSessions = new ConcurrentDictionary<long, Session>();
        }

        public Session GetSession(long chatId)
        {
            // Возвращаем сессию по ключу, если она существует
            if (allSessions.ContainsKey(chatId))
                return allSessions[chatId];

            // Создаем и возвращаем новую, если такой не было
            var newSession = new Session() { Command = "CountSymbols" };
            allSessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}
