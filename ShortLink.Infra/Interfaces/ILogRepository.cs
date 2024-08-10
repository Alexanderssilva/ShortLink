using ShortLink.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortLink.Infra.Interfaces
{
    public interface ILogRepository
    {
        Task InsertLog(Log log);
    }
}
