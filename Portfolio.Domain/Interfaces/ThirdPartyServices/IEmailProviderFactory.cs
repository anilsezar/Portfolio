namespace Portfolio.Domain.Interfaces.ThirdPartyServices;

public interface IEmailProviderFactory
{
    IEmailProvider GetProvider(string providerName);
}