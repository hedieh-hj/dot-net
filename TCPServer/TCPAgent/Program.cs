using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TCPAgent;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddSingleton<ValidationService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();
