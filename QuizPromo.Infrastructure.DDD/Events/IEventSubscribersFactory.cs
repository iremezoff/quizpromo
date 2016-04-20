using System.Collections.Generic;

namespace QuizPromo.Infrastructure.DDD.Events
{
    public interface IEventSubscribersFactory
    {
        IList<IEventHandler<Event>> GetSubscribers(Event @event);
    }
}
