using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMS.Core.Server.Core.EventBus;
using CMS.Core.Server.Domain.Models;
using Microsoft.Extensions.Hosting;

namespace CMS.Core.Server.WebApi.Worker
{
    public class EventsConsumerWorker : BackgroundService
    {
        private readonly IEventConsumerFactory _eventConsumerFactory;

        public EventsConsumerWorker(IEventConsumerFactory eventConsumerFactory)
        {
            _eventConsumerFactory = eventConsumerFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IEnumerable<IEventConsumer> consumers = new[]
            {
                _eventConsumerFactory.Build<Member, Guid>(),
            };
            var tc = Task.WhenAll(consumers.Select(c => c.ConsumeAsync(stoppingToken)));
            await tc;
        }
    }
}