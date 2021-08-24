using System.Threading.Tasks;
using CMS.Core.Server.Core.Models;

namespace CMS.Core.Server.Core
{
    public interface IEventsRepository<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        Task AppendAsync(TA aggregateRoot);
        Task<TA> RehydrateAsync(TKey key);
    }
}