using CMS.Core.Server.Core.EventBus;
using CMS.Core.Server.WebApi.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Core.Server.WebApi.Registries
{
    public static class EventConsumerRegistry
    {

        public static IServiceCollection RegisterWorker(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IEventConsumerFactory, EventConsumerFactory>();
            services.AddHostedService(ctx =>
            {
                var factory = ctx.GetRequiredService<IEventConsumerFactory>();
                return new EventsConsumerWorker(factory);
            });
            return services;
        }
        
    }
}