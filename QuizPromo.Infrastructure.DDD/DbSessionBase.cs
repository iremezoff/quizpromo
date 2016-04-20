using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using QuizPromo.Infrastructure.DDD.Events;

[assembly: InternalsVisibleTo("UGSK.K3.Infrastructure.Test")]

namespace QuizPromo.Infrastructure.DDD
{
    public abstract class DbSessionBase : IDbSession
    {
        private readonly IEventPublisher _eventPublisher;

        protected DbSessionBase()
        {
            //_eventPublisher = eventPublisher;
        }

        public void Publish(Event @event)
        {
            _eventPublisher.Publish(@event);
        }

        public void CommitChanges()
        {
            CommitChangesCore();

            _eventPublisher.PublishDelayed();
        }

        public async Task CommitChangesAsync()
        {
            await CommitChangesAsyncCore();

            _eventPublisher.PublishDelayed();
        }

        protected abstract void CommitChangesCore();
        protected abstract Task CommitChangesAsyncCore();
    }    
}