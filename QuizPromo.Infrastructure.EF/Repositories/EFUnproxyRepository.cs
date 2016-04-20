//using System;
//using System.Linq;
//using Microsoft.Data.Entity;
//using QuizPromo.Infrastructure.DDD;

//namespace QuizPromo.Infrastructure.EF.Repositories
//{
//    //Бестужев: этот репозиторий - копия обычного репозитория, но одиночные сущности будут возвращены 
//    //в распроксированном виде (доменной объект, а не прокси-класс EF)
//    public class EFUnproxyRepository<TEntity, TId> : RepositoryBase<TEntity, TId>, IUnproxyRepositoryWithTypedId<TEntity, TId>
//        where TEntity : Entity<TId>
//    {
//        private readonly DbContext _context;

//        private DbSet<TEntity> Entities
//        {
//            get { return this._context.Set<TEntity>(); }
//        }

//        public EFUnproxyRepository(IDbSession context)
//            : base(context as DbSessionBase)
//        {
//            var dbSession = context as EFDbSession;
//            if (dbSession == null)
//            {
//                throw new ArgumentException("Uncompatible dbSession, EFDbSession is expecting");
//            }
//            _context = dbSession.Context;
//        }

//        public override IQueryable<TEntity> GetAll()
//        {
//            return Entities.AsQueryable();
//        }

//        public override TEntity GetById(TId id)
//        {
//            var entity = Entities.Find(id);
//            if (entity != null && entity is DictionaryEntity)
//                return UnproxyEntity(entity);
//            return entity;
//        }

//        public override TEntity GetBySpecification(ISimpleSpecification<TEntity> specification)
//        {
//            var entity = GetAll().Where(specification.IsSatisfiedBy()).SingleOrDefault();
//            if (entity != null && entity is DictionaryEntity)
//                return UnproxyEntity(entity);
//            return entity;
//        }

//        protected TEntity UnproxyEntity(TEntity entity)
//        {
//            var proxyCreationEnabled = _context.Configuration.ProxyCreationEnabled;
//            try
//            {
//                _context.Configuration.ProxyCreationEnabled = false;
//                var result = _context.Entry(entity).CurrentValues.ToObject() as TEntity;
//                return result;
//            }
//            finally
//            {
//                _context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
//            }
//        }

//        protected override void SaveCore(TEntity entity)
//        {
//            Entities.Add(entity);
//            if (!entity.IsTransient())
//            {
//                var previousEntity = GetById(entity.Id);
//                _context.Entry(previousEntity).State = EntityState.Detached;
//                _context.Entry(entity).State = EntityState.Modified;
//            }
//        }

//        protected override void EditCore(TEntity entity)
//        {
//            _context.Entry(entity).State = EntityState.Modified;
//        }

//        public override void Delete(TEntity entity)
//        {
//            Entities.Remove(entity);
//        }
//    }
//}