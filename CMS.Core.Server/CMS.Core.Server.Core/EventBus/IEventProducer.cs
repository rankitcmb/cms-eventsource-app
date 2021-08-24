using System.Threading.Tasks;
using CMS.Core.Server.Core.Models;

namespace CMS.Core.Server.Core.EventBus
{
    public interface IEventProducer<in TA, in TKey>
        where TA : IAggregateRoot<TKey>
    {
        Task DispatchAsync(TA aggregateRoot);
    }
}