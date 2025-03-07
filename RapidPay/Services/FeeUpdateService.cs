using Microsoft.EntityFrameworkCore;
using RapidPay.Data;

namespace RapidPay.Services
{
    /// <summary>
    /// Background service to periodically update transaction fees.
    /// </summary>
    public class FeeUpdateService : BackgroundService
    {
        private readonly ILogger<FeeUpdateService> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeUpdateService"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public FeeUpdateService(ILogger<FeeUpdateService> logger, IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Main method that executes the background service.
        /// </summary>
        /// <param name="stoppingToken">Token to signal stopping the service.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("FeeUpdateService is starting.");

            // Register a callback to log when the service is stopping
            stoppingToken.Register(() => _logger.LogInformation("FeeUpdateService is stopping."));

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Ensure the database is ready before starting the main loop
                while (context.Database.GetPendingMigrations().Any() || !context.Database.CanConnect())
                {
                    _logger.LogInformation("Preparing database...");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            // Main loop to update the fee periodically
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("FeeUpdateService is working.");

                try
                {
                    // Create a new scope to get the required services
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var feeService = scope.ServiceProvider.GetRequiredService<IFeeService>();

                        // Update the fee and log the new value
                        var newFee = await feeService.UpdateFeeAsync();
                        _logger.LogInformation($"Fee updated to {Math.Round(newFee, 2)}.");
                    }
                }
                catch (Exception ex)
                {
                    // Log any errors that occur during the fee update
                    _logger.LogError(ex, "An error occurred while updating the fee.");
                }

                // Wait for an hour before the next update
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }

            _logger.LogInformation("FeeUpdateService has stopped.");
        }
    }
}