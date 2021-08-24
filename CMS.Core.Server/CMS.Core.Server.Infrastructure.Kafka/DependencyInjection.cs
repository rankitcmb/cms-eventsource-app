using System;
using CMS.Core.Server.Core.EventBus;
using CMS.Core.Server.Core.Models;
using CMS.Core.Server.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CMS.Core.Server.Infrastructure.Kafka
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddKafka(this IServiceCollection service,
            EventConsumerConfig eventConsumerConfig)
        {
            return service.AddSingleton(eventConsumerConfig)
                .AddSingleton(typeof(IEventConsumer<,>), typeof(EventConsumer<,>))
                .AddKafkaEventProducer<Member,Guid>(eventConsumerConfig);
        }

        private static IServiceCollection AddKafkaEventProducer<TA, TK>(this IServiceCollection services, EventConsumerConfig configuration)
            where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton<IEventProducer<TA, TK>>(ctx =>
            {
                var logger = ctx.GetRequiredService<ILogger<EventProducer<TA, TK>>>();
                return new EventProducer<TA, TK>(configuration.TopicBaseName, configuration.KafkaConnectionString, logger);
            });
        }
    }
}