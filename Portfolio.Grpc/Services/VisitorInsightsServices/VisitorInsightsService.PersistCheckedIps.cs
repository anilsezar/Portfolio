using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Serilog;

namespace Portfolio.Grpc.Services.VisitorInsightsServices;

public partial class VisitorInsightsService
{
    public override Task<Empty> PersistCheckedIps(PersistCheckedIpsRequest request, ServerCallContext context)
    {
        Log.Information("Request to log: {Log}",JsonSerializer.Serialize(request));
        
        Log.Information("✅ Received {IpCount} ips for handling", request.CheckedIps.Count);

        RemoveUnwantedRows(request);

        UpdateLocationInfo(request);
        
        return Task.FromResult(new Empty());
    }
    
    private void RemoveUnwantedRows(PersistCheckedIpsRequest request)
    {
        var unwantedRowsIds = request.CheckedIps
            .Where(x => x.Operation == DbOperationForThisRow.Delete)
            .Select(x => x.EntityId)
            .ToList();
        
        requestLogRepository.RemoveRows(unwantedRowsIds);
    }
    
    private void UpdateLocationInfo(PersistCheckedIpsRequest request)
    {
        var ipsToUpdate = request.CheckedIps.Where(x => x.Operation == DbOperationForThisRow.Update).ToList();
        foreach (var ip in ipsToUpdate)
            requestLogRepository.UpdateRowWithCityAndCountryInfo(ip.EntityId, ip.City, ip.Country);
    }
}
