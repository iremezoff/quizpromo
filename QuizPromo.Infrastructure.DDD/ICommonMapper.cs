using System;

namespace QuizPromo.Infrastructure.DDD
{
    public interface ICommonMapper
    {
        object Map(object source);
        object Map(object source, object destination);
    }

    public interface ICommonMapper<T1, T2> : ICommonMapper
    {
        T2 Map(T1 source);
        T2 Map(T1 source, T2 destination);
    }

    public static class MapperFactoryExtension
    {
        public static ICommonMapper<T, T> GetMapper<T>(this Func<Type, Type, ICommonMapper> factory)
        {
            return factory(typeof(T), typeof(T)) as ICommonMapper<T, T>;
        }

        public static ICommonMapper<TFrom, TTo> GetMapper<TFrom, TTo>(this Func<Type, Type, ICommonMapper> factory)
        {
            return factory(typeof(TFrom), typeof(TTo)) as ICommonMapper<TFrom, TTo>;
        }
    }
}
