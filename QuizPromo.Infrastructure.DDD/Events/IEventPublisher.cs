namespace QuizPromo.Infrastructure.DDD.Events
{
    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : Event;
        void PublishDelayed();
    }
}
