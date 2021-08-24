﻿using System;

namespace CMS.Core.Server.Core.Models
{
    public abstract class BaseDomainEvent<TA, TKey> : IDomainEvent<TKey>
        where TA : IAggregateRoot<TKey>
    {
        protected BaseDomainEvent() { }

        protected BaseDomainEvent(TA aggregateRoot)
        {
            if (aggregateRoot is null)
                throw new ArgumentNullException(nameof(aggregateRoot));

            this.AggregateVersion = aggregateRoot.Version;
            this.AggregateId = aggregateRoot.Id;
            this.Timestamp = DateTime.UtcNow;
        }

        public long AggregateVersion { get; private set; }
        public TKey AggregateId { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}