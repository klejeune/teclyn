using Teclyn.Core.Events;
using Teclyn.Core.Events.Properties;
using Teclyn.SampleCore.TodoLists.Models;
using Teclyn.SampleCore.Todos.Models;

namespace Teclyn.SampleCore.Todos.Events
{
    public class TodoTextUpdatedEvent : PropertyEvent<ITodo, string>
    {
        public override void Apply(ITodo aggregate, IEventInformation information)
        {
            aggregate.UpdateText(information.Type(this));
        }
    }
}