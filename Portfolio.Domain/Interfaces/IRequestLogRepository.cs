using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Interfaces;

public interface IRequestLogRepository
{
    Task PersistAsync(RequestLog rl);
}
