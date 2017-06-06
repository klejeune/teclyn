using System.Threading.Tasks;
using System.Windows.Input;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Storage;
using Teclyn.SampleCore.TodoLists.Models;
using Teclyn.SampleCore.Todos.Events;
using Teclyn.SampleCore.Todos.Models;

namespace Teclyn.SampleCore.Todos.Commands
{
    public class CreateTodoCommand : ICommand<ITodo>
    {
        public string Text { get; set; }
        public Id<ITodoList> TodoListId { get; set; }

        [Inject]
        private IRepository<ITodoList> TodoLists { get; set; }

        public bool CheckParameters(IParameterChecker _)
        {
            return true;
        }

        public bool CheckContext(ITeclynContext context, ICommandContextChecker _)
        {
            return true;
        }

        public async Task Execute(ICommandExecutionContext context)
        {
            var list = await this.TodoLists.GetById(this.TodoListId);

            this.Result = await context.GetEventService().Raise(new TodoCreatedEvent
            {
                AggregateId = context.GetIdGenerator().GenerateId(),
                Text = this.Text,
                TodoListId = list.Id,
                TodoListName = list.Name,
            });
        }

        public ITodo Result { get; set; }
    }
}