﻿using Account.Data;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Account.Handler
{
    class Program
    {
        private readonly IConfiguration configuration;

        //public IConfiguration _Configuration { get; }
        public Program(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        static async Task Main()
        {
            Console.Title = "Transaction";

            var endpointConfiguration = new EndpointConfiguration("Account");

            endpointConfiguration.EnableOutbox();
            var connection = ConfigurationManager.AppSettings["AccountConnection"];
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString("host= localhost:5672;username=guest;password=guest");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            //var routing = transport.Routing();
            //routing.RouteToEndpoint(
            //    messageType: typeof(Messages.Commands.DoTransaction),
            //    destination: "Account");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());
           
            containerSettings.ServiceCollection.AddDbContext<AccountContext>(options =>
                        options.UseSqlServer(connection));

            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new MappingProfile());
            //});

            //IMapper mapper = mappingConfig.CreateMapper();
            //containerSettings.ServiceCollection.AddSingleton(mapper);

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}