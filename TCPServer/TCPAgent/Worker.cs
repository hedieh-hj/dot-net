using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TCPAgent
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;        

        private readonly ValidationService _taxValidationService;

        public Worker(ILogger<Worker> logger, ValidationService taxValidationService)
        {
            _logger = logger;
            _taxValidationService = taxValidationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            await _taxValidationService.ListenToClientAsync(stoppingToken);

            _logger.LogInformation("TCP Agent worker started.");
        }
    }


}




