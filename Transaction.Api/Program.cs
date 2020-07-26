using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using NServiceBus;
using System.Data.SqlClient;

namespace Transaction.Api
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
         .Build();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Transaction");
                    endpointConfiguration.EnableInstallers();
                    var outboxSettings = endpointConfiguration.EnableOutbox();
                    outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
                    outboxSettings.RunDeduplicationDataCleanupEvery(TimeSpan.FromMinutes(15));
                    var recoverability = endpointConfiguration.Recoverability();
                    recoverability.Delayed(
                        customizations: delayed =>
                        {
                            delayed.NumberOfRetries(1);
                            delayed.TimeIncrease(TimeSpan.FromMinutes(4));
                        });

                    recoverability.Immediate(
                        customizations: immediate =>
                        {
                            immediate.NumberOfRetries(2);

                        });

                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                    transport.UseConventionalRoutingTopology()
                        .ConnectionString(Configuration.GetConnectionString("TransportConnection"));

                    var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                    var connection = Configuration.GetConnectionString("TransactionConnection");
                    persistence.SqlDialect<SqlDialect.MsSqlServer>();

                    persistence.ConnectionBuilder(
                        connectionBuilder: () =>
                        {
                            return new SqlConnection(connection);
                        });

                    var subscriptions = persistence.SubscriptionSettings();
                    subscriptions.CacheFor(TimeSpan.FromMinutes(10));
                    endpointConfiguration.SendFailedMessagesTo("error");
                    endpointConfiguration.AuditProcessedMessagesTo("audit");
                    
                    var routing = transport.Routing();
                    routing.RouteToEndpoint(
                        messageType: typeof(Messages.Commands.CreateTransaction),
                        destination: "Transaction");

                    var conventions = endpointConfiguration.Conventions();
                    conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
                    conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

