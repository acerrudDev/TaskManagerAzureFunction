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

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        var configuration = context.Configuration;

        TokenValidator.Init(configuration["OpenId_Authority"]);

        services.AddDbContext<TaskDbContext>(options =>
            options.UseSqlServer(connectionString));

        //Repositories Interfaces Scoped
        services.AddScoped<ITaskManagementRepository, TaskManagementRepository>();
        //Services Integration Scoped
        services.AddScoped<ITaskManagementService, TaskManagementService>();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

    })    
    .ConfigureOpenApi()
    .Build();

host.Run();
