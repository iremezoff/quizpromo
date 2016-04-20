using System.Linq;
using System.Threading.Tasks;

namespace QuizPromo.Infrastructure.DDD.Events.Impl
{
    public class CommandSender : ICommandSender
    {
        private readonly ICommandHandlersFactory _commandHandlersFactory;

        public CommandSender(ICommandHandlersFactory commandHandlersFactory)
        {
            _commandHandlersFactory = commandHandlersFactory;
        }

        public void Send<T>(T command) where T : Command
        {
            var handlers = _commandHandlersFactory.GetHandlers<T>();

            foreach (var handler in handlers)
            {
                handler(command);
            }
        }

        public async Task SendAsync<T>(T command) where T : Command
        {
            var handlers = _commandHandlersFactory.GetAsyncHandlers<T>();

            await Task.WhenAll(handlers.Select(m => m(command)));
        }
    }
}
