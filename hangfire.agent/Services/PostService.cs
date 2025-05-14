using Newtonsoft.Json;

namespace hangfire.agent.Services
{
    public class PostService(
        ILogger<PostService> logger)
    {
        private readonly ILogger<PostService> _logger = logger;


        public async Task RunPostAgentAsync(CancellationToken stoppingToken)
        {
            try
            {
                stoppingToken.ThrowIfCancellationRequested();

                await HangfireRunAsync();                

                _logger.LogInformation($"---------Hangfire called{DateTime.Now}----------");

            }
            catch (Exception e)
            {
                throw new Exception("exception");
            }
        }

        public async Task HangfireRunAsync()
        {
            _logger.LogInformation("Hangfire Job: Processing ...\n");
        }
    }
}
