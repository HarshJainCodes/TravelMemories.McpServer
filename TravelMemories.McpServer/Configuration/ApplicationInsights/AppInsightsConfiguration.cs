using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace TravelMemories.McpServer.Configuration.ApplicationInsights
{
    public class AppInsightsConfiguration : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = "McpServer";
        }
    }
}
