using Portfolio.Domain.Interfaces.ThirdPartyServices;

namespace Portfolio.Grpc.Services.SendEmailToAdmin.Providers;

public class EmailProviderFactory : IEmailProviderFactory
{
    private readonly Dictionary<string, IEmailProvider> _providers;

    public EmailProviderFactory(IEnumerable<IEmailProvider> providers)
    {
        _providers = providers.ToDictionary(p => p.GetType().Name, p => p);
    }

    public IEmailProvider GetProvider(string providerName)
    {
        return _providers.TryGetValue(providerName, out var provider)
            ? provider
            : throw new InvalidOperationException($"No email provider found for {providerName}");
    }
}
