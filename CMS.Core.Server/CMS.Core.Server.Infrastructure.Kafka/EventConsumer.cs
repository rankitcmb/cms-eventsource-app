using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CMS.Core.Server.Core;
using CMS.Core.Server.Core.EventBus;
using CMS.Core.Server.Core.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace CMS.Core.Server.Infrastructure.Kafka
{
    public class EventConsumer<TA,TKey>: IDisposable, IEventConsumer<TA,TKey> where TA : IAggregateRoot<TKey>
    {
        private IConsumer<TKey, string> _consumer;
        private readonly IEventSerializer _eventDeserializer;
        private readonly ILogger<EventConsumer<TA, TKey>> _logger;

        public event EventReceivedHandler<TKey> EventReceived;

        public EventConsumer(IEventSerializer eventDeserializer, EventConsumerConfig config, ILogger<EventConsumer<TA, TKey>> logger)
        {
            _eventDeserializer = eventDeserializer;
            _logger = logger;

            var aggregateType = typeof(TA);
            var consumerConfig = new ConsumerConfig()
            {
                GroupId = config.ConsumerGroup,
                BootstrapServers = config.KafkaConnectionString,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };

            var consumerBuilder = new ConsumerBuilder<TKey,string>(consumerConfig);
            var keyDeserializerFactory = new KeyDeserializerFactory();
            consumerBuilder.SetKeyDeserializer(keyDeserializerFactory.Create<TKey>());

            _consumer = consumerBuilder.Build();

            var topic = $"{config.TopicBaseName}-{aggregateType.Name}";
            _consumer.Subscribe(topic);
        }

        public Task ConsumeAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                var topics = string.Join(",", _consumer.Subscription);
                _logger.LogInformation("started Kafka consumer {ConsumerName} on {ConsumerTopic}", _consumer.Name, topics);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = _consumer.Consume(stoppingToken);
                        if (cr.IsPartitionEOF)
                            continue;

                        var messageTypeHeader = cr.Message.Headers.First(h => h.Key == "type");
                        var eventType = Encoding.UTF8.GetString(messageTypeHeader.GetValueBytes());

                        var @event = _eventDeserializer.Deserialize<TKey>(eventType, cr.Message.Value);
                        if (null == @event)
                            throw new SerializationException($"unable to deserialize event {eventType} : {cr.Message.Value}");

                        await OnEventReceived(@event);
                    }
                    catch (OperationCanceledException ex)
                    {
                        _logger.LogWarning(ex, "consumer {ConsumerName} on {ConsumerTopic} was stopped: {StopReason}", _consumer.Name, topics, ex.Message);
                        OnConsumerStopped();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"an exception has occurred while consuming a message: {ex.Message}");
                        OnExceptionThrown(ex);
                    }
                }
            }, stoppingToken);
        }
        protected virtual Task OnEventReceived(IDomainEvent<TKey> e)
        {
            var handler = EventReceived;
            return handler?.Invoke(this, e);
        }

        public delegate void ConsumerStoppedHandler(object sender);
        public event ConsumerStoppedHandler ConsumerStopped;

        protected virtual void OnConsumerStopped()
        {
            var handler = ConsumerStopped;
            handler?.Invoke(this);
        }


        public delegate void ExceptionThrownHandler(object sender, Exception e);
        public event ExceptionThrownHandler ExceptionThrown;
        protected virtual void OnExceptionThrown(Exception e)
        {
            var handler = ExceptionThrown;
            handler?.Invoke(this, e);
        }

        public void Dispose()
        {
            _consumer?.Dispose();
            _consumer = null;
        }
    }
}