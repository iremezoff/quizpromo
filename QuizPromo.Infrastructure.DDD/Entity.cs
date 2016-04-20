using System;

namespace QuizPromo.Infrastructure.DDD
{
    public abstract class Entity<TId> : IEntityWithTypedId<TId> where TId : struct
    {
        public TId Id { get; set; }

        public virtual bool IsTransient()
        {
            return this.Id.Equals(default(TId)) || this.Id is IComparable<TId> && ((IComparable<TId>)this.Id).CompareTo(default(TId)) < 0;
        }
    }

    public interface IEntityWithTypedId<TId>
    {
        TId Id { get; }

        bool IsTransient();
    }

    public abstract class ValueEntity : Entity<int>
    {
        protected bool Equals(ValueEntity other)
        {
            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            return obj is ValueEntity && Equals((ValueEntity)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Id.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("Id = {0}", Id);
        }
    }

    public abstract class DictionaryEntity : ValueEntity
    {
        public string Name { get; set; }
    }
}
