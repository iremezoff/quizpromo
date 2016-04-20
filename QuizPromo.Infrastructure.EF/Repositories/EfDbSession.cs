using System;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using QuizPromo.Infrastructure.DDD;
using QuizPromo.Infrastructure.DDD.Events;

namespace QuizPromo.Infrastructure.EF.Repositories
{
    public class EFDbSession : DbSessionBase, IDisposable
    {
        private DbContext _context;

        public EFDbSession(DbContext context)
            : base()
        {
            _context = context;
        }

        public DbContext Context => _context;

        protected override void CommitChangesCore()
        {
            _context.SaveChanges();
        }

        protected override async Task CommitChangesAsyncCore()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
