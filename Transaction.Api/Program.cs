using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Data.SqlClient;
using NServiceBus;

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
                var connection = Configuration.GetConnectionString("MeasureDBConnection");
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
                //endpointConfiguration.AuditSagaStateChanges(
                //        serviceControlQueue: "Particular.weightwatchers");
                var routing = transport.Routing();
                routing.RouteToEndpoint(
                    messageType: typeof(Messages.UpdateCard),
                    destination: "Subscriber");

                //routing.RouteToEndpoint(assembly: typeof(Messages.UpdateCard).Assembly, destination: "WeightWatchers.Api");
                var conventions = endpointConfiguration.Conventions();
                conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
                conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");
                //SubscribeToNotifications.Subscribe(endpointConfiguration);

                return endpointConfiguration;
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

