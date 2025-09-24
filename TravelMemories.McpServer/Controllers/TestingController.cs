using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;
using ModelContextProtocol.SemanticKernel.Extensions;
using System.Net.Http.Headers;
using TravelMemories.McpServer.Utilities;
using TravelMemoriesBackend.ApiClient.TripData;

namespace TravelMemories.McpServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TestingController : ControllerBase
    {
        private ITripDataClient _tripDataClient;
        private readonly IRequestContextProvider _requestContextProvider;
        private IConfiguration _configuration;

        public TestingController(ITripDataClient tripDataClient,
            IRequestContextProvider requestContextProvider,
            IConfiguration configuration)
        {
            _tripDataClient = tripDataClient;
            _requestContextProvider = requestContextProvider;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<string> TestController(UserPrmopt userPrompt)
        {
            IKernelBuilder kernelBuilder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion("gpt-4o-mini", "https://travel-memories-bot.openai.azure.com/", _configuration["azureOpenAIKey"]);
            var token = _requestContextProvider.GetJWTToken();

            Kernel kernel = kernelBuilder.Build();
            HttpClient mcpHttpClient = new HttpClient();

            mcpHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.RawData);

            await kernel.Plugins.AddMcpFunctionsFromSseServerAsync("TravelMemoriesMCPServer", new Uri("https://localhost:7210/sse"), httpClient: mcpHttpClient);

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            var history = new ChatHistory();
            history.AddUserMessage(userPrompt.Prompt);

            var result = await chatCompletionService.GetChatMessageContentAsync(history, executionSettings: openAIPromptExecutionSettings, kernel: kernel);
            // this will have the kernel, and will call the appropriate tool for the job
            return result.Content;
        }
    }

    public class UserPrmopt
    {
        public string Prompt { get; set; }
    }
}
