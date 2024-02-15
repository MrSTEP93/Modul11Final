using Modul11Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul11Final.Services
{
    public interface IStorage
    {
        Session GetSession(long chatID);
    }
}
