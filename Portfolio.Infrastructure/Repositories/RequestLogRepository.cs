using Portfolio.Domain.Entities;
using Portfolio.Domain.Interfaces;

namespace Portfolio.Infrastructure.Repositories;

public class RequestLogRepository(PortfolioDbContext dbContext): IRequestLogRepository
{
    public async Task PersistAsync(RequestLog rl)
    {
        await dbContext.RequestLog.AddAsync(rl);
        await dbContext.SaveChangesAsync();
    }
}