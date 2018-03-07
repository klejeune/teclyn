using System.Threading.Tasks;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;
using Teclyn.Core.Storage;
using Teclyn.SampleCore.TodoLists.Events;
using Teclyn.SampleCore.TodoLists.Models;
using Teclyn.SampleCore.Todos.Events;

namespace Teclyn.SampleCore.TodoLists.EventHandlers
{
    public class TodoCreatedEventHandler : IEventHandler<TodoCreatedEvent>
    {
        private IEventService _eventService;

        public TodoCreatedEventHandler(IEventService eventService)
        {
            this._eventService = eventService;
        }

        public async Task Handle(TodoCreatedEvent @event)
        {
            await this._eventService.Raise(new TodoAddedToListEvent
            {
                AggregateId = @event.TodoListId,
            });
        }
    }
}