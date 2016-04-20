using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuizPromo.Infrastructure.DDD.Events.Impl
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IEventSubscribersFactory _eventSubscribersFactory;
        private readonly IDictionary<Event, ICollection<Action<Event>>> _delayedHandlers = new Dictionary<Event, ICollection<Action<Event>>>();

        public EventPublisher(IEventSubscribersFactory eventSubscribersFactory)
        {
            _eventSubscribersFactory = eventSubscribersFactory;
        }

        public void Publish<T>(T @event) where T : Event
        {
            var handlers = _eventSubscribersFactory.GetSubscribers(@event);

            var remoteHandlers = handlers.Where(IsRemote);
            var localHandlers = handlers.Where(h => !IsRemote(h));

            _delayedHandlers.Add(@event, remoteHandlers.Select<IEventHandler<Event>, Action<Event>>(h => h.Handle).ToList());

            RunHandlers(@event, localHandlers.Select<IEventHandler<Event>, Action<Event>>(h => h.Handle).ToList());
        }

        public void PublishDelayed()
        {
            foreach (var handler in _delayedHandlers)
            {
                RunHandlers(handler.Key, handler.Value);
            }
        }

        private void RunHandlers(Event @event, ICollection<Action<Event>> handlers)
        {
            foreach (var handler in handlers)
            {
                //dispatch on thread pool for added awesomeness
                //var newHandler = handler;
                // TODO Иванов: В оригинальном примере тут используют выполнение обработчиков в разных потоках. Я пока убрал, т.к. не смог протестировать
                //ThreadPool.QueueUserWorkItem(x => handler(@event));
                handler(@event);
            }
        }

        private bool IsRemote(IEventHandler<Event> h)
        {
            return false;
        }
    }
}
