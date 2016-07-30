using System;
using System.Linq.Expressions;

namespace Teclyn.Core.Commands
{
    public interface IParameterChecker
    {
        bool Check(bool expression, string message);
        bool Check<TProperty>(Expression<Func<TProperty>> property, Predicate<TProperty> expression, string message);
        bool Check(string property, bool expression, string message);
    }
}