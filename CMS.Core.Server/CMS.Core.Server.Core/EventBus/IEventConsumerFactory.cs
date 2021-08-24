using CMS.Core.Server.Core.Models;

namespace CMS.Core.Server.Core.EventBus
{
    public interface IEventConsumerFactory
    {
        IEventConsumer Build<TA, TKey>() where TA : IAggregateRoot<TKey>;
    }
}