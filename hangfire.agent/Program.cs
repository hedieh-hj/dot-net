using hangfire.agent.Services;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Builder;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<PostService>();

//builder.Services.AddHostedService<Worker>();

builder.Services.AddHangfire(configuration =>
    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseStorage(new MySqlStorage(
                     "Server=192.168.30.11;Port=51250;User ID=root;Password=Orash7127640;Database=HangfireDB3",
                     new MySqlStorageOptions
                     {
                         TablesPrefix = "Hangfire", 
                         QueuePollInterval = TimeSpan.FromSeconds(15)                         
                     })));

builder.Services.AddHangfireServer();

var host = builder.Build();

host.Run();
