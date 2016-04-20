using System.Threading.Tasks;

namespace QuizPromo.Infrastructure.DDD.Events
{    
    public interface ICommandSender
    {
        void Send<T>(T command) where T : Command;
        Task SendAsync<T>(T command) where T : Command;
    }
}
