using System;

namespace CMS.Core.Server.Infrastructure.Kafka
{
    public class EventConsumerConfig
    {
        public EventConsumerConfig(string kafkaConnectionString,string topicBaseName,string consumerGroup)
        {
            if (string.IsNullOrWhiteSpace(kafkaConnectionString))
                throw new ArgumentException($"Value cannot be null of whitespace {nameof(kafkaConnectionString)}");
            if (string.IsNullOrWhiteSpace(topicBaseName))
                throw new ArgumentException($"Value cannot be null of whitespace {nameof(topicBaseName)}");
            if (string.IsNullOrWhiteSpace(consumerGroup))
                throw new ArgumentException($"Value cannot be null of whitespace {nameof(consumerGroup)}");

            KafkaConnectionString = kafkaConnectionString;
            TopicBaseName = topicBaseName;
            ConsumerGroup = consumerGroup;
        }

        public string KafkaConnectionString { get; }
        public string TopicBaseName { get; set; }
        public string ConsumerGroup { get; set; }
    }
}