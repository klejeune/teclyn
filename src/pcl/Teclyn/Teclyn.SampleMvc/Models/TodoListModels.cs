using System.Collections.Generic;
using Teclyn.SampleCore.TodoLists.Models;
using Teclyn.SampleCore.Todos.Models;

namespace Teclyn.SampleMvc.Models
{
    public class TodoListModel
    {
        public ITodoList TodoList { get; set; }
        public IEnumerable<ITodo> Todos { get; set; }
    }
}