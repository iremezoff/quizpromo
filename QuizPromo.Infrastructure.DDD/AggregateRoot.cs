using System.Collections.Generic;
using QuizPromo.Infrastructure.DDD.Events;

namespace QuizPromo.Infrastructure.DDD
{
    public abstract class AggregateRoot : Entity<int>
    {
        private readonly ICollection<Event> _events = new List<Event>();

        public IEnumerable<Event> GetEvents()
        {
            return _events;
        }

        protected void Apply<T>(T @event) where T : Event
        {
            if (!_events.Contains(@event))
                _events.Add(@event);
        }
    }
}
