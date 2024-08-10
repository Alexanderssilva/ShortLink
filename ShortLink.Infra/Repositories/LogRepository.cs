using ShortLink.Domain;
using ShortLink.Infra.Interfaces;

namespace ShortLink.Infra.Repositories
{
    public class LogRepository(MongoContext context) : ILogRepository
    {

        public async Task InsertLog(Log log) => await context.Logs.InsertOneAsync(log);
    }
}
