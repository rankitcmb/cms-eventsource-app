using System.Collections.Generic;

namespace CMS.Core.Server.Core.Models
{
    public interface IAggregateRoot<out TKey> : IEntity<TKey>
    {
        long Version { get; }
        IReadOnlyCollection<IDomainEvent<TKey>> Events { get; }
        void ClearEvents();
    }

}