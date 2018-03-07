using System;
using Teclyn.Core.Commands;
using Teclyn.Core.Commands.Properties;
using Teclyn.Core.Commands.Semantics;
using Teclyn.Core.Security.Context;
using Teclyn.SampleCore.Todos.Events;
using Teclyn.SampleCore.Todos.Models;

namespace Teclyn.SampleCore.Todos.Commands
{
    public class UpdateTodoTextCommand : PropertyCommand<ITodo, TodoTextUpdatedEvent, string>
    {
        public override bool CheckParameters(IParameterChecker _)
        {
            return true;
        }
            
        public override bool CheckContext(ITeclynContext context, ICommandContextChecker _)
        {
            return _.Check(this.NewValue != null, "error");
        }

        public override Func<ITodo, string> PropertyAccessor => todo => todo.Text;

        [RichText]
        public override string NewValue { get; set; }
    }
}