using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CMS.Core.Server.Core.EventBus;
using CMS.Core.Server.Core.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace CMS.Core.Server.Infrastructure.Kafka
{
    public class EventProducer<TA, TKey> : IDisposable,
        IEventProducer<TA, TKey> where TA : IAggregateRoot<TKey>
    {
        private IProducer<TKey, string> _producer;
        private readonly string _topicName;
        private readonly ILogger<EventProducer<TA, TKey>> _logger;

        public EventProducer(string topicBaseName,string kafkaConnectionString,ILogger<EventProducer<TA,TKey>> logger)
        {
            _logger = logger;
            var aggregateType = typeof(TA);

            _topicName = $"{topicBaseName}-{aggregateType.Name}";

            var producerConfig = new ProducerConfig(){BootstrapServers = kafkaConnectionString};
            var producerBuilder = new ProducerBuilder<TKey,string>(producerConfig);
            producerBuilder.SetKeySerializer(new KeySerializer<TKey>());

            _producer = producerBuilder.Build();

        }
        
        public async Task DispatchAsync(TA aggregateRoot)
        {
            if (aggregateRoot == null)
                throw new ArgumentNullException(nameof(aggregateRoot));
            if (!aggregateRoot.Events.Any())
                return;
            _logger.LogInformation($"Publishing {aggregateRoot.Events.Count} events for 'AggregateId' {aggregateRoot.Id}");

            foreach (var @event in aggregateRoot.Events)
            {
                var eventType = @event.GetType();
                var serializedEvent = JsonSerializer.Serialize(@event, eventType);

                var headers = new Headers()
                {
                    {"aggregate", Encoding.UTF8.GetBytes(@event.AggregateId.ToString())},
                    {"type", Encoding.UTF8.GetBytes(eventType.AssemblyQualifiedName)}
                };

                var message = new Message<TKey, string>()
                {
                    Key = @event.AggregateId,
                    Value = serializedEvent,
                    Headers = headers
                };

                await _producer.ProduceAsync(_topicName, message);
            }

        }

        public void Dispose()
        {
            _producer?.Dispose();
            _producer = null;
        }
    }
}