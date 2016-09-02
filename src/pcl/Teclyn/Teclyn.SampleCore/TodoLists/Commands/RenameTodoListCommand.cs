using System;
using Teclyn.Core.Commands;
using Teclyn.Core.Commands.Properties;
using Teclyn.Core.Security.Context;
using Teclyn.SampleCore.TodoLists.Events;
using Teclyn.SampleCore.TodoLists.Models;

namespace Teclyn.SampleCore.TodoLists.Commands
{
    [Remote]
    public class RenameTodoListCommand : PropertyCommand<ITodoList, TodoListRenamedEvent, string>
    {
        public override bool CheckParameters(IParameterChecker _)
        {
            return true;
        }

        public override bool CheckContext(ITeclynContext context, ICommandContextChecker _)
        {
            return true;
        }

        public override Func<ITodoList, string> PropertyAccessor => todoList => todoList.Name;
    }
}