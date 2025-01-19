using Portfolio.Domain.Interfaces.Repositories.Dtos;
using Portfolio.Domain.Interfaces.ThirdPartyServices.Dtos;

namespace Portfolio.Domain.Interfaces.ThirdPartyServices;

public interface IEmailProvider
{ 
    Task<SendEmailResultDto> SendEmailAsync(EmailDto dto);
}