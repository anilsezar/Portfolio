using Portfolio.Domain.Interfaces;

namespace Portfolio.Grpc.Services.VisitorInsightsServices;

public partial class VisitorInsightsService(IRequestLogRepository requestLogRepository) : VisitorInsights.VisitorInsightsBase;