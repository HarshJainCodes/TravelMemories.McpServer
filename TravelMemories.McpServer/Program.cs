using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelMemories.McpServer.Configuration.ApplicationInsights;
using TravelMemories.McpServer.Utilities;
using TravelMemoriesBackend.ApiClient.DependencyConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRequestContextProvider, RequestContextProvider>();

builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

builder.Services.AddSingleton<ITelemetryInitializer, AppInsightsConfiguration>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// this is coming from the backend side which has exposed the api clients for this MCP server to call
builder.Services.AddApiClients(builder.Configuration["BACKEND_URL"]);   // this URL can be configured based on the environment

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(x => x.Cookie.Name = "travelMemoriestoken")
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["IssuerSigningKeySecretText"])),
        ValidateIssuer = false,
        ValidateAudience = false,
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                context.Token = authHeader.Substring("Bearer ".Length).Trim();
                return Task.CompletedTask;
            }
            context.Token = context.Request.Cookies["travelMemoriestoken"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("TraveMemories.McpServer", policy =>
    {
        policy.WithOrigins("https://localhost:5173", "https://memories.harshjain17.com").AllowCredentials().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddMcpServer().WithHttpTransport(o =>
{
    o.PerSessionExecutionContext = true;
}).WithToolsFromAssembly();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("TraveMemories.McpServer");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/.well-known/oauth-authorization-server", () =>
{
    return Results.Json(new
    {
        issuer = "https://localhost:5001",
        authorization_endpoint = "https://localhost:5001/oauth/authorize",
        token_endpoint = "https://localhost:5001/oauth/token",
        scopes_supported = new[] { "openid", "profile", "email" },
        response_types_supported = new[] { "code" },
        grant_types_supported = new[] { "authorization_code", "refresh_token" },
        code_challenge_methods_supported = new[] { "S256" }
    });
});

app.MapMcp().RequireAuthorization();

app.Run();
