using ConferencesManagementDAO.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

public class DatabaseMigrator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseMigrator> _logger;

    public DatabaseMigrator(IServiceProvider serviceProvider, ILogger<DatabaseMigrator> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void MigrateDatabase()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ConferenceManagementDbContext>();
            try
            {
                _logger.LogInformation("Applying database migrations...");
                dbContext.Database.Migrate();
                _logger.LogInformation("Database migration completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}
