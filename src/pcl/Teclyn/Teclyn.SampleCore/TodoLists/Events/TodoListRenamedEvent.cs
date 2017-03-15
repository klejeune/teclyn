using Teclyn.Core.Events;
using Teclyn.Core.Events.Properties;
using Teclyn.SampleCore.TodoLists.Models;

namespace Teclyn.SampleCore.TodoLists.Events
{
    public class TodoListRenamedEvent : IPropertyEvent<ITodoList, string>
    {
        public void Apply(ITodoList aggregate)
        {
            aggregate.Rename(this);
        }

        public string AggregateId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}