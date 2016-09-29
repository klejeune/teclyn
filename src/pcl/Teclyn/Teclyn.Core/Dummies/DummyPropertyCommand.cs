using System;
using Teclyn.Core.Commands;
using Teclyn.Core.Commands.Properties;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Dummies
{
    public class DummyPropertyCommand : PropertyCommand<IDummyAggregate, DummyPropertyEvent, string>
    {
        public override bool CheckParameters(IParameterChecker _)
        {
            return true;
        }

        public override bool CheckContext(ITeclynContext context, ICommandContextChecker _)
        {
            return true;
        }

        public override Func<IDummyAggregate, string> PropertyAccessor => aggregate => aggregate.Property;
    }
}