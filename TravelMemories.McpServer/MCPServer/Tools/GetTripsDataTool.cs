using Microsoft.AspNetCore.Authorization;
using ModelContextProtocol.Server;
using System.Text;
using TravelMemories.McpServer.Utilities;
using TravelMemoriesBackend.ApiClient.TripData;
using TravelMemoriesBackend.Contracts.Storage;

namespace TravelMemories.McpServer.MCPServer.Tools
{
    [McpServerToolType]
    [Authorize]
    public class GetTripsDataTool
    {
        private ITripDataClient _tripDataClient;
        private readonly IRequestContextProvider _requestContextProvider;

        public GetTripsDataTool(ITripDataClient tripDataClient,
            IRequestContextProvider requestContextProvider)
        {
            _tripDataClient = tripDataClient;
            _requestContextProvider = requestContextProvider;
        }

        /// <summary>
        /// Gets the total number of trips that you have
        /// </summary>
        /// <returns></returns>
        [McpServerTool]
        public async Task<string> GetTripsCount()
        {
            var token = _requestContextProvider.GetJWTToken();

            var allTripsData = await _tripDataClient.GetAllTripDataAync(token.RawData);
            return $"You have a total of {allTripsData.Count} trips.";
        }

        /// <summary>
        /// Tells you the total number of trips you have travelled so far for a particular year. User will provide a year.
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        [McpServerTool]
        public async Task<string> GetAllTrips(int Year)
        {
            var token = _requestContextProvider.GetJWTToken();

            var allTripsData = await _tripDataClient.GetAllTripDataAync(token.RawData);

            var filteredTrips = allTripsData.Where(x => x.Year == Year);

            var resultTable = new StringBuilder();
            resultTable.AppendLine($"In Year {Year}, You had a total of {filteredTrips.Count()}  Trips");

            resultTable.AppendLine("| TripName | Year |");
            resultTable.AppendLine("|----------|------|");
            foreach (ImageData trip in filteredTrips)
            {
                resultTable.AppendLine($"| {trip.TripTitle} | {trip.Year} |");
            }

            return resultTable.ToString();
        }
    }
}
