using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Security.Context;
using Teclyn.SampleCore.TodoLists.Events;
using Teclyn.SampleCore.TodoLists.Models;

namespace Teclyn.SampleCore.TodoLists.Commands
{
    public class CreateTodoListCommand : ICommand<ITodoList>
    {
        public string Name { get; set; }

        public bool CheckParameters(IParameterChecker _)
        {
            return _.Check(nameof(this.Name), !string.IsNullOrWhiteSpace(this.Name), "Please provide a name for your new Todo List.");
        }

        public bool CheckContext(ITeclynContext context, ICommandContextChecker _)
        {
            return true;
        }

        public async Task Execute(ICommandExecutionContext context)
        {
            this.Result = await context.GetEventService().Raise(new TodoListCreatedEvent
            {
                AggregateId = context.GetIdGenerator().GenerateId(),
                Name = this.Name,
                Date = context.GetTimeService().Now(),
            });
        }

        public ITodoList Result { get; private set; }
    }
}