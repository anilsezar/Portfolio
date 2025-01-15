using Portfolio.Domain.Entities;
using Portfolio.Domain.Interfaces.Dtos;

namespace Portfolio.Domain.Interfaces;

public interface IRequestLogRepository: IRepository<RequestLog>
{
    List<UncheckedIpDto> GetRowsOfUncheckedIps();
    void UpdateRowWithCityAndCountryInfo(int rowId, string city, string country);
}
