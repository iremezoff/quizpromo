namespace QuizPromo.Infrastructure.DDD.Events
{
    public interface IEventHandler<T> where T : Event
    {
        void Handle(T @event);
    }
}
