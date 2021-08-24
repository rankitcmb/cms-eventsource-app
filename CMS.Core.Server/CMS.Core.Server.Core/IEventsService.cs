using System.Threading.Tasks;
using CMS.Core.Server.Core.Models;

namespace CMS.Core.Server.Core
{
    public interface IEventsService<TA, TKey> 
        where TA : class, IAggregateRoot<TKey>
    {
        Task PersistAsync(TA aggregateRoot);
        Task<TA> RehydrateAsync(TKey key);
    }
}