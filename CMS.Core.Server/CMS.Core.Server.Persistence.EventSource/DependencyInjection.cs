using System;
using CMS.Core.Server.Core;
using CMS.Core.Server.Core.Models;
using CMS.Core.Server.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CMS.Core.Server.Persistence.EventSource
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, string connectionString)
        {
            return services.AddSingleton<IEventStoreConnectionWrapper>(ctx =>
            {
                var logger = ctx.GetRequiredService<ILogger<EventStoreConnectionWrapper>>();
                return new EventStoreConnectionWrapper(new Uri(connectionString), logger);
            }).AddEventsRepository<Member, Guid>();
        }

        private static IServiceCollection AddEventsRepository<TA, TK>(this IServiceCollection services)
            where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton<IEventsRepository<TA, TK>>(ctx =>
            {
                var connectionWrapper = ctx.GetRequiredService<IEventStoreConnectionWrapper>();
                var eventDeserializer = ctx.GetRequiredService<IEventSerializer>();
                return new EventsRepository<TA, TK>(connectionWrapper, eventDeserializer);
            });
        }
    }
}