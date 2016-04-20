using System.Threading.Tasks;

namespace QuizPromo.Infrastructure.DDD
{
    public interface IDbSession
    {
        void CommitChanges();

        Task CommitChangesAsync();
    }
}
