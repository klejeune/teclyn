using System;
using System.Linq.Expressions;

namespace Teclyn.Core.Queries
{
    public interface IQueryContextChecker
    {
        bool Check(bool expression, string message);
        bool Check<TProperty>(Expression<Func<TProperty>> property, Predicate<TProperty> expression, string message);
    }
}