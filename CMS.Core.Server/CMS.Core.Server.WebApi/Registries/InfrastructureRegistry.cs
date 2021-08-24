using System;
using CMS.Core.Server.Core;
using CMS.Core.Server.Core.EventBus;
using CMS.Core.Server.Core.Models;
using CMS.Core.Server.Domain.Models;
using CMS.Core.Server.Infrastructure.Kafka;
using CMS.Core.Server.Persistence.EventSource;
using CMS.Core.Server.WebApi.Common;
using CMS.Core.Server.WebApi.Persistence.MSSQL;
using CMS.Core.Server.WebApi.Persistence.MSSQL.EventHandlers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Core.Server.WebApi.Registries
{
    public static class InfrastructureRegistry
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddOnPremiseInfrastructure(config);
            return services
                .AddEventsService<Member, Guid>();
        }
        private static IServiceCollection AddOnPremiseInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeof(MemberEventHandler))
                    .RegisterHandlers(typeof(IRequestHandler<>))
                    .RegisterHandlers(typeof(IRequestHandler<,>))
                    .RegisterHandlers(typeof(INotificationHandler<>));
            }).AddMSSQL(config);

            var kafkaConnStr = config.GetConnectionString("kafka");
            var eventsTopicName = config["eventsTopicName"];
            var groupName = config["eventsTopicGroupName"];
            var consumerConfig = new EventConsumerConfig(kafkaConnStr, eventsTopicName, groupName);

            var eventStoreConnStr = config.GetConnectionString("eventstore");

            return services.AddKafka(consumerConfig)
                .AddEventStore(eventStoreConnStr);
        }

        private static IServiceCollection AddEventsService<TA, TK>(this IServiceCollection services)
            where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton<IEventsService<TA, TK>>(ctx =>
            {
                var eventsProducer = ctx.GetRequiredService<IEventProducer<TA, TK>>();
                var eventsRepo = ctx.GetRequiredService<IEventsRepository<TA, TK>>();

                return new EventsService<TA, TK>(eventsRepo, eventsProducer);
            });
        }
    }
}