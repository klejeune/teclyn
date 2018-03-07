using Teclyn.Core;
using Teclyn.SampleCore.Todos.Models;

namespace Teclyn.SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var teclyn = new TeclynApi(typeof(ITodo).Assembly);
        }
    }
}
