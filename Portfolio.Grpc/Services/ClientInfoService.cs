using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Portfolio.Domain.Constants;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Interfaces;
using Serilog;

namespace Portfolio.Grpc.Services;

public class ClientInfoService(IRequestLogRepository repository) : ClientInfo.ClientInfoBase
{
    // todo: I wanna use MediatR here
    public override Task<Empty> StoreInfo(ClientInfoRequest r, ServerCallContext context)
    {
        Log.Information("Request to log: {Log}",JsonSerializer.Serialize(r));

        repository.PersistAsync(
            new RequestLog
            {
                AcceptLanguage = r.Language,
                UserAgent = r.UserAgent,
                Platform = r.Platform,
                Referrer = r.Referrer,
                DoNotTrack = r.DoNotTrack,
                Connection = r.Connection,
                Resolution = r.Resolution,
                DeviceMemory = r.DeviceMemory,
                OnLine = r.OnLine,
                HardwareConcurrency = r.HardwareConcurrency,
                Webdriver = r.Webdriver,
                CookieEnabled = r.CookieEnabled,
                MaxTouchPoints = r.MaxTouchPoints,

                ClientIp = r.IpAddress,
                City = DefaultValues.EmptyForString,
                Country = DefaultValues.EmptyForString,
                Extras = r.Extras
            }
        );
        
        return Task.FromResult(new Empty());
    }
}