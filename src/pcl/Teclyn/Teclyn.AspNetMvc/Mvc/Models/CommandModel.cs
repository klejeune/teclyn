using Teclyn.Core.Commands;

namespace Teclyn.AspNetMvc.Mvc.Models
{
    public class CommandModel
    {
        public ICommand Command { get; set; }
        public string CommandType { get; set; }
        public string ReturnUrl { get; set; }
    }
}