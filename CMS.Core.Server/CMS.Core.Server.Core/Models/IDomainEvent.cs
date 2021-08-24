using System;

namespace CMS.Core.Server.Core.Models
{
    public interface IDomainEvent<out TKey>
    {
        long AggregateVersion { get; }
        TKey AggregateId { get; }
        DateTime Timestamp { get; }
    }
}