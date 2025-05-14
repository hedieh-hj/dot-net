using hangfire.agent.Services;
using Hangfire;

namespace ota.post.invoice.agent
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly PostService _post_service;

        public Worker(ILogger<Worker> logger, PostService post_service)
        {
            _logger = logger;
            _post_service = post_service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            stoppingToken.ThrowIfCancellationRequested();                        

            await Task.Run(() =>
            {
                RecurringJob.AddOrUpdate("send-invoice-0018", () => _post_service.RunPostAgentAsync(stoppingToken), "*/1 * * * *");
            }, stoppingToken);

            _logger.LogInformation("Post work started.");
        }
    }
}
