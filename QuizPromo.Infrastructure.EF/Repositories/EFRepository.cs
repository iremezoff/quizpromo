using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Data.Entity;
using QuizPromo.Infrastructure.DDD;

namespace QuizPromo.Infrastructure.EF.Repositories
{
    public class EFRepository<TEntity, TId> : RepositoryBase<TEntity, TId>
        where TEntity : Entity<TId> where TId : struct
    {
        private readonly DbContext _context;

        private DbSet<TEntity> Entities => _context.Set<TEntity>();

        private static readonly MethodInfo GenericMethodPattern =
            ((Func<IQueryable<TEntity>, LambdaExpression, IQueryable<TEntity>>)ApplyIncluding<int>).Method
                .GetGenericMethodDefinition();

        private List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> _includedFuncs;

        public EFRepository(IDbSession context)
            : base(context as DbSessionBase)
        {
            var dbSession = context as EFDbSession;
            if (dbSession == null)
            {
                throw new ArgumentException("Uncompatible dbSession, EFDbSession is expecting");
            }
            _context = dbSession.Context;

            var lambdaExpressions = _context.Model.GetIncludedForeignEntities<TEntity>();

            _includedFuncs = lambdaExpressions.Select(Queryable).ToList();
        }

        public override IQueryable<TEntity> GetAll()
        {
            return _includedFuncs.Aggregate(Entities.AsQueryable(), (current, func) => func(current));
        }

        private Func<IQueryable<TEntity>, IQueryable<TEntity>> Queryable(LambdaExpression lambda)
        {
            var methodInfo = GenericMethodPattern.MakeGenericMethod(lambda.ReturnType);

            var firstDelegate =
                methodInfo.CreateDelegate(typeof(Func<IQueryable<TEntity>, LambdaExpression, IQueryable<TEntity>>))
                    as
                    Func<IQueryable<TEntity>, LambdaExpression, IQueryable<TEntity>>;

            if (firstDelegate == null)
            {
                throw new InvalidCastException();
            }

            return entities => firstDelegate(entities, lambda);
        }

        private static IQueryable<TEntity> ApplyIncluding<TProperty>(IQueryable<TEntity> dbSet, LambdaExpression lambda)
        {
            return dbSet.Include(lambda as Expression<Func<TEntity, TProperty>>);
        }

        public override TEntity GetById(TId id)
        {
            return Entities.FirstOrDefault(e => e.Id.Equals(id));
        }

        protected override void SaveCore(TEntity entity)
        {
            Entities.Add(entity);

            if (!entity.IsTransient())
            {
                var previousEntity = GetById(entity.Id);
                _context.Entry(previousEntity).State = EntityState.Detached;
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        protected override void EditCore(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public override void Delete(TEntity entity)
        {
            Entities.Remove(entity);
        }
    }
}
