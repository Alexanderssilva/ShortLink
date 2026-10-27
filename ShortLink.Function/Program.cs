using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShortLink.Application.Interfaces;
using ShortLink.Application.Services;
using ShortLink.Infra;
using ShortLink.Infra.Interfaces;
using ShortLink.Infra.Repositories;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddSingleton<MongoContext>();
        services.AddScoped<ILinkRepository, LinkRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<ILinkService, LinkService>();
        services.AddScoped<IUserService, UserService>();
    })
    .Build();

host.Run();
