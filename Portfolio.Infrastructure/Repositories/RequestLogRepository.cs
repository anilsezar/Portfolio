using Portfolio.Domain.Entities;
using Portfolio.Domain.Interfaces;
using Portfolio.Domain.Interfaces.Dtos;
using Serilog;

namespace Portfolio.Infrastructure.Repositories;

public class RequestLogRepository(PortfolioDbContext dbContext): BaseRepository<RequestLog>(dbContext), IRequestLogRepository
{
    public List<UncheckedIpDto> GetRowsOfUncheckedIps()
    {
        return dbContext.RequestLog
            .Where(x => x.ClientIp != "" &&
                        x.City == "" &&
                        x.Country == "")
            .Select(x => new UncheckedIpDto
            {
                Id = x.Id,
                ClientIp = x.ClientIp
            })
            .ToList();
    }

    public void UpdateRowWithCityAndCountryInfo(int rowId, string city, string country)
    {
        var rowToUpdate = GetById(rowId);
        
        if (rowToUpdate == null)
        {
            Log.Error("Request log with id {Id} not found, what happened?", rowId);
            return;
        }
        
        rowToUpdate.UpdateLocation(city, country);

        dbContext.SaveChanges();
    }
}
