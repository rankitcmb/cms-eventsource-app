namespace CMS.Core.Server.Core.Models
{
    public interface IEntity<out TKey>
    { 
        TKey Id { get; }
    }
}