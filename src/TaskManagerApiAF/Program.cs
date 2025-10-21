using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManagerApiAF.Data.Context;
using TaskManagerApiAF.Data.Respositories;
using TaskManagerApiAF.Interfaces.IRepositories;
using TaskManagerApiAF.Interfaces.IServices;
using TaskManagerApiAF.Services;
using TaskManagerApiAF.Utils;


#pragma warning disable AZFW0014 // Missing expected registration of ASP.NET Core Integration services
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        var configuration = context.Configuration;

        services.AddDbContext<TaskDbContext>(options =>
            options.UseSqlServer(connectionString));

        //Repositories Interfaces Scoped
        services.AddScoped<ITaskManagementRepository, TaskManagementRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        //Services Integration Scoped
        services.AddScoped<ITaskManagementService, TaskManagementService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenAuthService, TokenAuthService>();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

    })    
    .ConfigureOpenApi()
    .Build();
#pragma warning restore AZFW0014 // Missing expected registration of ASP.NET Core Integration services

host.Run();
