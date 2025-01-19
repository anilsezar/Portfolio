namespace Portfolio.Domain.Interfaces.ThirdPartyServices.Dtos;

public class SendEmailResultDto
{
    public required bool IsItSentSuccessfully { get; init; }
    public required string ErrorMessage { get; init; }
}