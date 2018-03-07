using System;
using System.Linq;
using System.Linq.Expressions;

namespace Teclyn.Core.Domains
{
    public interface IAccessController<TAggregate>
    {
        Expression<Func<TAggregate, bool>> ControlExpression { get; set; }
    }
}