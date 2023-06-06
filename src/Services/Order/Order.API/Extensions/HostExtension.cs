using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Order.API.Extensions
{
    public static class HostExtension
    {
        public static IServiceProvider MigrateDatabase<TContext>(this IServiceProvider serviceProvider,Action<TContext,IServiceProvider> seeder, int? retry = 0)
            where TContext : DbContext
        {
            int retryForAvailability = retry.Value;
            var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();
            var context = serviceProvider.GetService<TContext>();

            try
            {
                logger.LogInformation("Migration Started");
                InvokeSeeder(seeder,context, serviceProvider);
            }
            catch (SqlException ex)
            {

                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase(serviceProvider, seeder, retryForAvailability);
                }
            }

            return serviceProvider;

        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider serviceProvider) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, serviceProvider);
        }
    }
}
