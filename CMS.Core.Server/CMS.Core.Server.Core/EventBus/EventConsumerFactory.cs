using System.Threading;
using System.Threading.Tasks;
using CMS.Core.Server.Core.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Core.Server.Core.EventBus
{
    public class EventConsumerFactory : IEventConsumerFactory
    {
        private readonly IServiceScopeFactory scopeFactory;

        public EventConsumerFactory(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public IEventConsumer Build<TA, TKey>() where TA : IAggregateRoot<TKey>
        {
            using var scope = scopeFactory.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<IEventConsumer<TA, TKey>>();

            async Task OnEventReceived(object s, IDomainEvent<TKey> e)
            {
                var @event = EventReceivedFactory.Create((dynamic)e);

                using var innerScope = scopeFactory.CreateScope();
                var mediator = innerScope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(@event, CancellationToken.None);
            }
            consumer.EventReceived += OnEventReceived;

            return consumer;
        }
    }
}