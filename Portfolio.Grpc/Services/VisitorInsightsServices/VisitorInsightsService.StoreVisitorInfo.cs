using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Constants;
using Serilog;

namespace Portfolio.Grpc.Services.VisitorInsightsServices;

public partial class VisitorInsightsService
{
    // todo: I wanna use MediatR here
    public override Task<Empty> StoreVisitorInfo(StoreVisitorInfoRequest r, ServerCallContext context)
    {
        Log.Information("Request to log: {Log}",JsonSerializer.Serialize(r));

        requestLogRepository.Create(
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