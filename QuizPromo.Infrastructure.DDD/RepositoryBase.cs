using System;
using System.Linq;
using QuizPromo.Infrastructure.DDD.Events;

namespace QuizPromo.Infrastructure.DDD
{
    public abstract class RepositoryBase<TEntity, TId> : IRepositoryWithTypedId<TEntity, TId> where TEntity : Entity<TId> where TId : struct
    {
        private readonly DbSessionBase _dbSession;

        protected RepositoryBase(DbSessionBase dbSession)
        {
            if (dbSession == null)
            {
                throw new ArgumentNullException("dbSession");
            }
            _dbSession = dbSession;
        }

        public abstract TEntity GetById(TId id);

        public abstract IQueryable<TEntity> GetAll();

        public abstract void Delete(TEntity target);

        protected abstract void SaveCore(TEntity entity);

        public void Save(TEntity entity)
        {
            SaveCore(entity);

            ProcessAggregate(entity);
        }

        protected abstract void EditCore(TEntity entity);

        public IQueryable<TEntity> GetAllBySpecification(ISimpleSpecification<TEntity> specification)
        {
            return GetAll().Where(specification.IsSatisfiedBy());
        }

        public virtual TEntity GetBySpecification(ISimpleSpecification<TEntity> specification)
        {
            return GetAll().Where(specification.IsSatisfiedBy()).SingleOrDefault();
        }

        public void Edit(TEntity entity)
        {
            EditCore(entity);

            ProcessAggregate(entity);
        }

        public void Delete(TId id)
        {
            TEntity entity = this.GetById(id);

            if (entity != null)
            {
                Delete(entity);
            }
        }

        private void ProcessAggregate(TEntity entity)
        {
            var aggregate = entity as AggregateRoot;
            if (aggregate == null) return;

            var events = aggregate.GetEvents();

            foreach (var @event in events)
            {
                _dbSession.Publish(@event);
            }
        }
    }
}