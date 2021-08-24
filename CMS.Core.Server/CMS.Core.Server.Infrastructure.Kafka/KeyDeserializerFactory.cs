﻿using System;
using Confluent.Kafka;

namespace CMS.Core.Server.Infrastructure.Kafka
{
    internal class KeyDeserializerFactory
    {
        public IDeserializer<TKey> Create<TKey>()
        {
            var tk = typeof(TKey);
            if (tk == typeof(Guid))
                return (dynamic)new GuidDeserializer();
            throw new ArgumentOutOfRangeException($"invalid type: {tk}");
        }
    }
}