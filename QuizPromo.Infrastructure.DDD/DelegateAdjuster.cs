using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using QuizPromo.Infrastructure.DDD.Events;

namespace QuizPromo.Infrastructure.DDD
{
    public class DelegateAdjuster
    {
        public static Action<BaseT> CastArgument<BaseT, DerivedT>(Expression<Action<DerivedT>> source) where DerivedT : BaseT
        {
            if (typeof(DerivedT) == typeof(BaseT))
            {
                return (Action<BaseT>)((Delegate)source.Compile());

            }
            ParameterExpression sourceParameter = Expression.Parameter(typeof(BaseT), "source");
            var result = Expression.Lambda<Action<BaseT>>(Expression.Invoke(source, Expression.Convert(sourceParameter, typeof(DerivedT))), sourceParameter);
            return result.Compile();
        }

        public static Action<BaseT> CastArgument<BaseT>(Type eventType, Expression<Action<Event>> source) 
        {
            if (eventType == typeof(BaseT))
            {
                return (Action<BaseT>)((Delegate)source.Compile());

            }
            ParameterExpression sourceParameter = Expression.Parameter(typeof(BaseT), "source");
            var result = Expression.Lambda<Action<BaseT>>(Expression.Invoke(source, Expression.Convert(sourceParameter, eventType)), sourceParameter);
            return result.Compile();
        }

        public static Func<BaseT, Task> CastArgument<BaseT, DerivedT>(Expression<Func<DerivedT, Task>> source) where DerivedT : BaseT
        {
            if (typeof(DerivedT) == typeof(BaseT))
            {
                return (Func<BaseT, Task>)((Delegate)source.Compile());

            }
            ParameterExpression sourceParameter = Expression.Parameter(typeof(BaseT), "source");
            var result = Expression.Lambda<Func<BaseT, Task>>(Expression.Invoke(source, Expression.Convert(sourceParameter, typeof(DerivedT))), sourceParameter);
            return result.Compile();
        }
    }
}
