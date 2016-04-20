using System.Threading.Tasks;

namespace QuizPromo.Infrastructure.DDD.Events
{
    public interface ICommandHandler<in TCommand> where TCommand : Command
    {
        void Handle(TCommand message);
    }

    public interface IAsyncCommandHandler<in TCommand> where TCommand : Command
    {
        Task HandleAsync(TCommand message);
    }
}
