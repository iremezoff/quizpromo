using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QuizPromo.Infrastructure.DDD
{
    /// <summary>
    /// Используется для составления выражения фильтрации с последующей трансляцией в sql-код, 
    /// поэтому использование кастомных c#-методов (т.е. определенных в своем коде, а не базовые функции типа Max  и т.п.) не допускается. Для этих целей используйте ISpecification
    /// </summary>
    /// <typeparam name="T">Тип данных</typeparam>
    public interface ISimpleSpecification<T>
    {
        Expression<Func<T, bool>> IsSatisfiedBy();
    }

    public interface ILinqSpecification<T>
    {
        IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates);
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.OrElse(expr1.Body,
                ParameterRebinder.ReplaceParameters(
                    expr1.Parameters.Select((f, i) => new { f, s = expr2.Parameters[i] }).ToDictionary(p => p.s, p => p.f),
                    expr2.Body));
            return Expression.Lambda<Func<T, bool>>(invokedExpr, expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.AndAlso(expr1.Body,
                ParameterRebinder.ReplaceParameters(
                    expr1.Parameters.Select((f, i) => new { f, s = expr2.Parameters[i] }).ToDictionary(p => p.s, p => p.f),
                    expr2.Body));
            return Expression.Lambda<Func<T, bool>>(invokedExpr, expr1.Parameters);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr1)
        {
            var invokedExpr = Expression.Not(expr1.Body);
            return Expression.Lambda<Func<T, bool>>(invokedExpr, expr1.Parameters);
        }
    }

    internal class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }

    public class CombineSpecification<T> : ISimpleSpecification<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public CombineSpecification(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        public Expression<Func<T, bool>> IsSatisfiedBy()
        {
            return _expression;
        }
    }

    public static class SimpleSpecificationBuilder
    {

        public static ISimpleSpecification<T> Or<T>(this ISimpleSpecification<T> spec1,
                                                            ISimpleSpecification<T> spec2)
        {
            return new CombineSpecification<T>(spec1.IsSatisfiedBy().Or(spec2.IsSatisfiedBy()));
        }

        public static ISimpleSpecification<T> And<T>(this ISimpleSpecification<T> spec1,
                                                            ISimpleSpecification<T> spec2)
        {
            return new CombineSpecification<T>(spec1.IsSatisfiedBy().And(spec2.IsSatisfiedBy()));
        }
    }

    public static class SpecificationHelper
    {
        public static bool IsSatisfiedBy<T>(this ISimpleSpecification<T> specification, T value)
        {
            var validator = specification.IsSatisfiedBy().Compile();
            return validator.Invoke(value);
        }
    }
}