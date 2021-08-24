using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace CMS.Core.Server.Persistence.EventSource
{
    public interface IEventStoreConnectionWrapper
    {
        Task<IEventStoreConnection> GetConnectionAsync();
    }
}