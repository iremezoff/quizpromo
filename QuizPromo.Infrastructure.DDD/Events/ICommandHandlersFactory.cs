using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizPromo.Infrastructure.DDD.Events
{
    public interface ICommandHandlersFactory
    {
        IList<Action<IMessage>> GetHandlers<TCommand>() where TCommand : Command;
        IList<Func<IMessage, Task>> GetAsyncHandlers<TCommand>() where TCommand : Command;
    }
}
