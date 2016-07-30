using System;
using System.Linq.Expressions;
using Teclyn.Core.Security;

namespace Teclyn.Core.Commands
{
    public interface ICommandContextChecker
    {
        bool Check(bool expression, string message);
        bool Check<TProperty>(Expression<Func<TProperty>> property, Predicate<TProperty> expression, string message);
    }
}